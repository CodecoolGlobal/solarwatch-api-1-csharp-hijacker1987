using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Controllers;
using SolarWatch.Data;
using SolarWatch.Repository;
using SolarWatch.Service;
using SolarWatch.Service.Authentication;

namespace SolarWatch;

public class Startup
{
    private readonly IConfiguration _configuration;
    
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        
        services.AddSingleton<ISolarDataProvider, OpenSolarMapApi>();
        services.AddSingleton<IJsonProcessor, JsonProcessor>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ISunTimeRepository, SunTimeRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        
        var connection = _configuration["ConnectionString"];
        
        services.AddDbContext<UsersContext>(options => options.UseSqlServer(connection));
        
        var iA = _configuration["IssueAudience"];
        var iS = _configuration["IssueSign"];
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = iA,
                    ValidAudience = iA,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(iS!)
                    ),
                };
            });
        
        services
            .AddIdentityCore<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>() //Enable Identity roles 
            .AddEntityFrameworkStores<UsersContext>();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, UsersContext usersContext)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        // using static System.Net.Mime.MediaTypeNames;
        app.UseStatusCodePages(MediaTypeNames.Text.Plain, "Status Code Page: {0}");

        app.UseHttpsRedirection();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;
            }
            else
            {
                await next();
            }
        });
        
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        if (env.IsEnvironment("Test"))
        {
            await usersContext.Database.EnsureCreatedAsync();
            await usersContext.Database.MigrateAsync(); 
        }
        
        var solarApiKey = _configuration["Api:ServiceApiKey"];
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", () => solarApiKey);
        });
        
        await AddRoles(app);        
    }
    
    async Task AddRoles(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope(); // RoleManager is a scoped service, therefore we need a scope instance to access it
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var adminRoleName = _configuration["Roles:Admin"];
        var userRoleName = _configuration["Roles:User"];

        var tAdmin = CreateAdminRole(roleManager, adminRoleName!);
        tAdmin.Wait();

        var tUser = CreateUserRole(roleManager, userRoleName!);
        tUser.Wait();
        
        await CreateAdminIfNotExists(userManager);
    }

    async Task CreateAdminRole(RoleManager<IdentityRole> roleManager, string adminRoleName)
    {
        await roleManager.CreateAsync(new IdentityRole(adminRoleName!)); //value in appSettings
    }

    async Task CreateUserRole(RoleManager<IdentityRole> roleManager, string userRoleName)
    {
        await roleManager.CreateAsync(new IdentityRole(userRoleName!)); //value in appSettings
    }
    
    async Task CreateAdminIfNotExists(UserManager<IdentityUser> userManager)
    {
        var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
        if (adminInDb == null)
        {
            var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
            var adminCreated = await userManager.CreateAsync(admin, "admin123");

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}

//Docker run command: docker run -d -p 8080:80 -e ASPNETCORE_ENVIRONMENT=Development solarwatch
