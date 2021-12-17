using Core.DbContexts;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Core.Controllers;

[ApiController]
[Route("[controller]")]
public class DiseaseController : ControllerBase
{

    private readonly ILogger<DiseaseController> _logger;

    public DiseaseController(ILogger<DiseaseController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "asd")]
    public async Task<IActionResult> GetAsync()
    {
        var context = new InsuranceContext();
        var consumer = await context.Set<Consumer>().AsQueryable().Include(x => x.DiseaseHistories)
            .FirstAsync(x => x.Id == new Guid("e76b23b8-c667-4f4d-a498-f7a8b89f818b"));
        return Ok(consumer?.DiseaseHistories.First().Description);
    }
}
