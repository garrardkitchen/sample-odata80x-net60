using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<BloggingContext>();
builder.Services.AddTransient<AuthorRepository>();

builder.Services.AddDBConext(options => options.DefaultPageSize = 5 );
builder.Services.AddControllers(options => options.EnableEndpointRouting = true
    ).AddOData(options =>
    {
        options.AddRouteComponents("odata", GetEdmModel()).Filter().Count().Expand().Select().OrderBy().SetMaxTop(5);
        options.EnableAttributeRouting = true;
        options.RouteOptions.EnableControllerNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "OK");

//app.MapPost("/author", async (Author author, BloggingContext context, AuthorRepository reps) =>
//{
//    var created = await reps.Add(author);

//    return Results.Created($"/author/{author.AuthorId}", author);
//});

// openAPI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseODataBatching();

app.UseRouting();

// needed!!
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.Run();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EntitySet<Author>("author");
    return modelBuilder.GetEdmModel();
}