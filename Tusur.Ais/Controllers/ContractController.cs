using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data.Entities.UniversityStructure;
using Tusur.Ais.Models.Request;

namespace Tusur.Ais.Controllers;

public class ContractController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContractController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("add-signatory")]
    public async Task<IActionResult> AddSignatory([FromBody] CreateSignatoryRequestModel model)
    {
        if (model.Name == "" || model.LastName == "" || model.Patronymic == "" || model.JobTitle == "")
        {
            return BadRequest("Not all fields are filled in");
        }

        var signatory = new Signatory
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            LastName = model.LastName,
            Patronymic = model.Patronymic,
            JobTitle = model.JobTitle
        };

        var result = await _context.AddAsync(signatory);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("add-proxy")]
    public async Task<IActionResult> AddProxy([FromBody] CreateSignatoryRequestModel model)
    {
        var foundSignatory = await _context.Signatories
            .FirstOrDefaultAsync(s => s.Name == model.Name && s.LastName == model.LastName && 
                                      s.JobTitle == model.JobTitle && s.Patronymic == model.Patronymic);

        if (foundSignatory is null)
        {
            return BadRequest($"Not found signatory ${foundSignatory} in database");
        }

        var foundProxy = await _context.Proxies
            .FirstOrDefaultAsync(p => p.Number == model.Number);

        if (foundProxy is not null)
        {
            return BadRequest($"Proxy number with number ${foundProxy} already added in database");
        }

        var proxy = new Proxy
        {
            Id = Guid.NewGuid(),
            SignatoryId = foundSignatory.Id,
            DateStart = model.DateStart,
            Number = model.Number,
            Signatory = foundSignatory
        };
        
        var result = await _context.AddAsync(proxy);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();

        return Ok();
    }
}