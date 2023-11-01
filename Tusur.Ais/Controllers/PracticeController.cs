using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.UniversityStructure;
using Tusur.Ais.Models.Request;

namespace Tusur.Ais.Controllers;

public class PracticeController : Controller
{
    private readonly ApplicationDbContext _context;

    public PracticeController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("add-practice-kind")]
    public async Task<IActionResult> AddPracticeKind([FromBody] CreatePracticesRequestModel model)
    {
        var foundPracticeKind = await _context.PracticeKinds
            .FirstOrDefaultAsync(p => p.Name == model.PracticeKindName);

        if (foundPracticeKind is not null)
        {
            return BadRequest($"PracticeKind ${foundPracticeKind.Name} already added in database");
        }

        var practiceKind = new PracticeKind
        { 
             Name = model.PracticeKindName
        };
        
        var result = await _context.AddAsync(practiceKind);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-practice-type")]
    public async Task<IActionResult> AddPracticeType([FromBody] CreatePracticesRequestModel model)
    {
        var foundPracticeKind = await _context.PracticeKinds
            .FirstOrDefaultAsync(p => p.Name == model.PracticeKindName);

        if (foundPracticeKind is null)
        {
            return BadRequest($"PracticeKind ${foundPracticeKind?.Name} not found in database");
        }
        
        var foundPracticeType = await _context.PracticeTypes
            .FirstOrDefaultAsync(p => p.Name == model.PracticeTypeName);

        if (foundPracticeType is not null)
        {
            return BadRequest($"PracticeType ${foundPracticeType.Name} already added in database");
        }

        var practiceType = new PracticeType
        { 
            Id = Guid.NewGuid(),
            PracticeKindName = foundPracticeKind.Name,
            Name = model.PracticeTypeName,
            PracticeKind = foundPracticeKind
        };
        
        var result = await _context.AddAsync(practiceType);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-practice")]
    public async Task<IActionResult> AddPractice([FromBody] CreatePracticeRequestModel model)
    {
        var foundRecruitmentYear = await _context.RecruitmentYears
            .FirstOrDefaultAsync(r => r.Year.Year == model.Year.Year);

        if (foundRecruitmentYear is null)
        {
            return BadRequest($"RecruitmentYear ${foundRecruitmentYear} not found in database");
        }
        
        var foundPracticeType = await _context.PracticeTypes
            .FirstOrDefaultAsync(p => p.Name == model.PracticeTypeName);

        if (foundPracticeType is null)
        {
            return BadRequest($"PracticeType ${foundPracticeType} not found in database");
        }
        
        var foundPractice = await _context.Practices
            .FirstOrDefaultAsync(p => p.RecruitmentYearId == foundRecruitmentYear.Id 
                                 && p.PracticeTypeId == foundPracticeType.Id && p.Semester == model.Semester);

        if (foundPractice is not null)
        {
            return BadRequest($"Practice ${foundPractice} already added in database");
        }

        var practice = new Practice
        { 
            Id = Guid.NewGuid(),
            RecruitmentYearId = foundRecruitmentYear.Id,
            PracticeTypeId = foundPracticeType.Id,
            Semester = model.Semester,
            RecruitmentYear = foundRecruitmentYear,
            PracticeType = foundPracticeType,
        };
        
        var result = await _context.AddAsync(practice);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost]
    [Route("add-practice-period")]
    public async Task<IActionResult> AddPracticePeriod([FromBody] CreatePracticePeriodRequestModel model)
    {
        var foundPracticeType = await _context.PracticeTypes
            .FirstOrDefaultAsync(p => p.Name == model.PracticeTypeName);

        if (foundPracticeType is null)
        {
            return BadRequest($"PracticeType ${foundPracticeType?.Name} already added in database");
        }
        
        var foundRecruitmentYear = await _context.RecruitmentYears
            .FirstOrDefaultAsync(r => r.Year.Year == model.Year.Year);
        
        if (foundRecruitmentYear is null)
        {
            return BadRequest($"RecruitmentYear ${foundRecruitmentYear} already added in database");
        }
        
        var foundPractice = await _context.Practices
            .FirstOrDefaultAsync(p => p.PracticeTypeId == foundPracticeType.Id 
                                      && p.RecruitmentYearId == foundRecruitmentYear.Id && p.Semester == model.Semester);

        if (foundPractice is null)
        {
            return BadRequest($"Practice ${foundPractice} not found in database");
        }
        
        var foundPracticePeriod = await _context.PracticePeriods
            .FirstOrDefaultAsync(p => p.PracticeId == foundPractice.Id);

        if (foundPracticePeriod is not null)
        {
            return BadRequest($"PracticePeriod ${foundPracticePeriod} already added in database");
        }

        var practicePeriod = new PracticePeriod
        { 
            Id = Guid.NewGuid(),
            PracticeId = foundPractice.Id,
            StartDate = model.DateStart,
            EndDate = model.DateEnd,
            Practice = foundPractice
        };
        
        var result = await _context.AddAsync(practicePeriod);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-practice-contract")]
    public async Task<IActionResult> AddPracticeContract([FromBody] CreatePracticeContractRequestModel model)
    {
        var foundContract = await _context.Contracts
            .FirstOrDefaultAsync(c => c.Number == model.Number);

        if (foundContract is null)
        {
            return BadRequest($"Contract ${foundContract} not found in database");
        }
        
        var foundPracticeType = await _context.PracticeTypes
            .FirstOrDefaultAsync(p => p.Name == model.PracticeTypeName);

        if (foundPracticeType is null)
        {
            return BadRequest($"PracticeType ${foundPracticeType?.Name} already added in database");
        }
        
        var foundRecruitmentYear = await _context.RecruitmentYears
            .FirstOrDefaultAsync(r => r.Year.Year == model.Year.Year);
        
        if (foundRecruitmentYear is null)
        {
            return BadRequest($"RecruitmentYear ${foundRecruitmentYear} already added in database");
        }
        
        var foundPractice = await _context.Practices
            .FirstOrDefaultAsync(p => p.PracticeTypeId == foundPracticeType.Id 
                                      && p.RecruitmentYearId == foundRecruitmentYear.Id && p.Semester == model.Semester);

        if (foundPractice is null)
        {
            return BadRequest($"Practice ${foundPractice} already added in database");
        }
        
        var foundPracticeContract = await _context.PracticeContracts
            .FirstOrDefaultAsync(p => p.PracticeId == foundPractice.Id 
                                 && p.ContractId == foundContract.ApplicationId);

        if (foundPracticeContract is null)
        {
            return BadRequest($"PracticeContract ${foundPracticeContract} already added in database");
        }

        var practiceContract = new PracticeContract
        { 
            ContractId = foundContract.ApplicationId,
            PracticeId = foundPractice.Id,
            Contract = foundContract,
            Practice = foundPractice
        };
        
        var result = await _context.AddAsync(practiceContract);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}