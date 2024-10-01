using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Models;
using System.Threading.Tasks;

namespace RRS.Api.Controllers;

public class TestController : BaseController
{
    private readonly IAppDbContext _dbContext;
    public TestController(IAppDbContext dbContext, UserManager<AppUser> userManager) : base(userManager)
    {
        _dbContext = dbContext;
    }

    //[HttpPost(nameof(CreateTest))]
    //public async Task<ActionResult<TestObj>> CreateTest([FromBody] TestObj obj, CancellationToken ct)
    //{
    //    await _dbContext.testObjs.AddAsync(obj, ct);
    //    await _dbContext.SaveChangesAsync(ct);

    //    return CreatedAtAction(nameof(CreateTest), obj);
    //}

    //[HttpGet(nameof(GetTest))]
    //public async Task<ActionResult<List<TestObj>>> GetTest(CancellationToken ct)
    //{
    //    var projects = await _dbContext.testObjs
    //        .AsNoTracking()
    //        .Select(a => a.Name)
    //        .ToListAsync(ct);

    //    return Ok(projects);
    //}я
}
