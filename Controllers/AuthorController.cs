using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

public class AuthorController : ODataController
{
    private readonly AuthorRepository repo;

    public AuthorController(AuthorRepository repo)
    {
        this.repo = repo;
    }

    // Get ~/Authors
    [HttpGet]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxTop = 1, PageSize = 100, MaxExpansionDepth = 5)]
    //[EnableQuery]
    public async Task<IActionResult> GetAsync()
    {
        var result = await repo.GetAll();
        return Ok(result.AsQueryable());
    }

    // param must be "key"

    // GET ~/Authors('10001')
    [EnableQuery]
    public async Task<IActionResult> GetAsync(string key)
    {
        var author = await repo.TryGetById(int.Parse(key));

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }
}