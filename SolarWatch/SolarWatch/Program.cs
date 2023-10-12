using System.Net.Mime;
using SolarWatch.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISolarDataProvider, OpenSolarMapApi>();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();

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

// using static System.Net.Mime.MediaTypeNames;
app.UseStatusCodePages(MediaTypeNames.Text.Plain, "Status Code Page: {0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();