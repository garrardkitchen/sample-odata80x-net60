using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.ComponentModel.DataAnnotations;

[ODataAttributeRouting]
[Route("odata/author")]
public class AuthorController : ODataController
{
    private readonly AuthorRepository repo;

    
    public AuthorController(AuthorRepository repo)
    {
        this.repo = repo;
    }

    
    [HttpPost("({key})/blog({keyBlog})/approval")]
    [EnableQuery]
    //public async Task<IActionResult> PostAsync([FromODataUri][Required] string key, [FromODataUri][Required] string keyBlog, [FromBody][Required] Approval approvalBody)
    public async Task<IActionResult> PostAsync(string key, string keyBlog, [FromBody] Approval approvalBody)
    {
        return Ok();
    }

    //[HttpPost("odata/author({key})")]
    [HttpPost("({key})")]
    [EnableQuery]
    //public async Task<IActionResult> PostAsync([FromODataUri][Required] string key, [FromBody][Required] Approval approvalBody)
    public async Task<IActionResult> PostAsync(string key, [FromBody] Approval approvalBody)
    {
        return Ok();
    }

    //[HttpPost("")]
    //public async Task<IActionResult> PostAsync([FromBody][Required] Author approvalBody)
    //{
    //    return Ok();
    //}

    // Get ~/Authors
    [HttpGet("all")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxTop = 1, PageSize = 100, MaxExpansionDepth = 5)]
    //[EnableQuery]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await repo.GetAll();
        return Ok(result.AsQueryable());
    }

    // param must be "key"

    // GET ~/Authors('10001')
    [HttpGet("({key})")]
    [EnableQuery]
    public async Task<IActionResult> GetAsyncById(string key)
    {
        var author = await repo.TryGetById(int.Parse(key));

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [HttpGet("({key})/foo({key2})/baa")]
    [EnableQuery]
    public async Task<IActionResult> GetAsync3(string key, string key2)
    {
        var author = await repo.TryGetById(int.Parse(key));

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    // POST /Books(1)/Rate
    // Body has { Rating: 7 }
    // This is bound Action. The action is bound to the Books entity set.
    //[EnableQuery]
    [HttpPost]
    //public IActionResult Rate([FromODataUri] int key, ODataActionParameters parameters)
    public IActionResult Rate(int key, ODataActionParameters parameters)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        int rating = (int)parameters["Rating"];

        if (rating < 0)
        {
            return BadRequest();
        }

        return Ok();
    }

}