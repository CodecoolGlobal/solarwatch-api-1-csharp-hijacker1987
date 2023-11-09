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
using SolarWatchMvp.Repository;

var builder = WebApplication.CreateBuilder(args);
var solarApiKey = builder.Configuration["Api:ServiceApiKey"];
var iA = builder.Configuration["IssueAudience"];
var iS = builder.Configuration["IssueSign"];
var connection = builder.Configuration["ConnectionString"];
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
AddDbContext();
AddServices();
AddAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //Added by default
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
//Docker run command: docker run -d -p 8080:80 -e ASPNETCORE_ENVIRONMENT=Development solarwatchapp

// using static System.Net.Mime.MediaTypeNames;
app.UseStatusCodePages(MediaTypeNames.Text.Plain, "Status Code Page: {0}");

app.UseHttpsRedirection();
app.UseRouting();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => solarApiKey);

AddRoles();
AddAdmin();

app.Run();

void AddDbContext()
{
    builder.Services.AddDbContext<CityApiContext>(options =>
        options.UseSqlServer(connection));
    builder.Services.AddDbContext<UserContext>(options =>
        options.UseSqlServer(connection));
}

void AddServices()
{
    builder.Services.AddControllers(
        options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services
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
                    Encoding.UTF8.GetBytes(iS)
                ),
            };
        });
    builder.Services
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
        .AddEntityFrameworkStores<UserContext>();
}

void AddAppServices()
{
    builder.Services.AddSwaggerGen(option =>
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
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
    builder.Services.AddSingleton<ISolarDataProvider, OpenSolarMapApi>();
    builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
    builder.Services.AddScoped<ICityRepository, CityRepository>();
    builder.Services.AddScoped<ISunTimeRepository, SunTimeRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddDbContext<CityApiContext>();
    builder.Services.AddDbContext<UserContext>();
}

void AddRoles()
{
    using var scope = app.Services.CreateScope(); // RoleManager is a scoped service, therefore we need a scope instance to access it
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var tAdmin = CreateAdminRole(roleManager);
    tAdmin.Wait();

    var tUser = CreateUserRole(roleManager);
    tUser.Wait();
}

async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("Admin")); //The role string should better be stored as a constant or a value in appsettings
}

async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("User")); //The role string should better be stored as a constant or a value in appsettings
}

void AddAdmin()
{
    var tAdmin = CreateAdminIfNotExists();
    tAdmin.Wait();
}

async Task CreateAdminIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
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
