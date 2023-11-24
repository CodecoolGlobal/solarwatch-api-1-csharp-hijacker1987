using SolarWatchMinimal.Controllers;
using SolarWatchMinimal.Controllers.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();                   //Returning HTML with adding Razor

builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddSingleton<ISolarDataProvider, OpenSolarMapApi>();
builder.Services.AddLogging();

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/", () => "Hello Zsófi!");
//app.MapGet("/hello", () => "Hello World!");
string SomeMessage() => "Hello World with custom API.";
app.MapGet("/hello", SomeMessage).WithName("WelcomeMessage").WithGroupName("Greetings").ExcludeFromDescription();;;    //Customizing OpenAPI

app.MapGet("/todos", () => new { TodoItem = "Learn about routing", Complete = false });
//app.MapPost("/todos", () => Results.Ok());
//app.MapPost("/todos", (Todo todo) => todo.Name);    //Model binding - with Razor the endpoint: /page/todos
app.MapPost("/todos", (Todo todo) =>
    {
        // Itt lehet a logika, amit a "/todos" végpontra érkező POST kérések esetén hajt végre az alkalmazás.
        // Például, adatbázisba mentés, stb.
        // A "todo" változó a végponton érkező HTTP kérés testéből (body) származó adatokat tartalmazza.
    })
    .ProducesProblem(401)
    .Produces<Todo>(201);

//app.MapGet("/hello/{name}", (string name) => $"Hello {name}");
app.MapGet("/hello/{name}", (HttpContext ctx) => $"Hello {ctx.Request.RouteValues["name"]}");

//app.MapGet("/hello/{name:int}", (string name) => $"Hello {name}"); //Uses numbers as endpoints
app.MapGet("/dobad", () => int.Parse("this is not an int"));

app.MapControllers();
app.MapGet("/SolarWatch/GetCurrent/{name}", async (HttpContext ctx, string name) =>
{
    try
    {
        var clientFactory = ctx.RequestServices.GetRequiredService<IHttpClientFactory>();
        var client = clientFactory.CreateClient();

        var response = await client.GetAsync($"http://localhost:5016//SolarWatch/GetCurrent?name={name}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            ctx.Response.StatusCode = 200;
            await ctx.Response.WriteAsync(result);
        }
        else
        {
            ctx.Response.StatusCode = (int)response.StatusCode;
            await ctx.Response.WriteAsync($"Error: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        ctx.Response.StatusCode = 500; // Internal Server Error
        await ctx.Response.WriteAsync($"Error: {ex.Message}");
    }
});

if (app.Environment.IsDevelopment())
{
    app.MapGet("/OnlyInDev",
        () => "This can only be accessed when the app is running in development.");
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.EnvironmentName == "TestEnvironment")
{
    app.MapGet("/OnlyInTestEnvironment", () => "TestEnvironment");
}

app.MapRazorPages();        //App using Razor

app.Run();

class Todo
{
    public string Name {get;set;}
    public bool IsComplete {get;set;}
}
