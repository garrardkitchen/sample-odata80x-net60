using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Author> Authors { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
        Console.WriteLine(DbPath);
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BlobContext;Trusted_Connection=True;MultipleActiveResultSets=true");

        //.UseSqlite($"Data Source={DbPath}");

    //
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}

public class Author
{
    public int AuthorId { get; set; }

    public string Username { get; set; }

    //public string Created { get; set; }
}

public class AuthorRepository 
{
    private readonly BloggingContext context;
    private readonly IOptions<DbOptions> options;

    public AuthorRepository(BloggingContext context, IOptions<DbOptions> options)
    {
        this.context = context;
        this.options = options;
    }

    public async Task<Author> Add(Author author)
    {
        var created = await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
        return (Author)created.Entity;
    }

    public async Task<List<Author>> GetAll ()
    {

        return options.Value.DefaultPageSize != 0 ? 
            await context.Authors.Take(options.Value.DefaultPageSize).ToListAsync() : await context.Authors.ToListAsync();
    }

    public async Task<Author> TryGetById(int key)
    {
        return await context.Authors?.FirstOrDefaultAsync(x => x.AuthorId == key);
    }
}

public class DbOptions
{
    public int DefaultPageSize { get; set; } = 2;

}

public static class Foo
{
    public static void AddDBConext(this IServiceCollection services, Action<DbOptions>? options = null)
    {       
        services.Configure(options);
    }
} 