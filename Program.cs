using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<BloggingContext>();
builder.Services.AddTransient<AuthorRepository>();

builder.Services.AddDBConext(  options => options.DefaultPageSize = 5 );

// openAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "OK");

app.MapPost("/author", async (Author author, BloggingContext context, AuthorRepository reps) =>
{
    var created = await reps.Add(author);

    return Results.Created($"/author/{author.AuthorId}", author);
});

app.MapGet("/author", async (AuthorRepository repo) => Results.Ok(await repo.GetAll()));

// openAPI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();
