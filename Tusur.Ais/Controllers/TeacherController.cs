using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.IntergroupRelations;
using Tusur.Ais.Models.Defaults;
using Tusur.Ais.Models.Request;

namespace Tusur.Ais.Controllers;

public class TeacherController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeacherController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    //Доделать запрос получения договоров: выборка происходит по статусу документа ApplicationStatuses.Signed в сущности Application и Id преподавателя в договоре
    
    [HttpPost]
    [Route("approve-contract")]
    public async Task<IActionResult> ApproveContract([FromBody] ApproveContractRequestModel model)
    {
        var foundStudent = await _context.Students.FirstOrDefaultAsync(t => t.Id == model.StudentId);

        if (foundStudent is null)
        {
            return BadRequest($"Student ${foundStudent} not found in database");
        }

        var foundStudentApplication = await _context.StudentApplications
            .FirstOrDefaultAsync(t => t.StudentId == foundStudent.Id);

        if (foundStudentApplication is null)
        {
            return BadRequest($"Student Application ${foundStudentApplication} not found in database");
        }

        var application = foundStudentApplication.Application;

        if (application.Status is ApplicationStatuses.Sent) // тут enum же, так что хз сработает или нет
        {
            application.Status = ApplicationStatuses.ApprovedByTeacher;
        }

        if (application.Contract?.Company is { Status: CompanyConfirmationStatuses.InProcess }) // тут enum же, так что хз сработает или нет
        {
            application.Contract.Company.Status = CompanyConfirmationStatuses.Confirmed;
        }
        
        _context.Update(application); 
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("sending-for-revision")]
    public async Task<IActionResult> SendingForRevision([FromBody] ApproveContractRequestModel model)
    {
        var foundStudent = await _context.Students.FirstOrDefaultAsync(t => t.Id == model.StudentId);

        if (foundStudent is null)
        {
            return BadRequest($"Student ${foundStudent} not found in database");
        }

        var foundStudentApplication = await _context.StudentApplications
            .FirstOrDefaultAsync(t => t.StudentId == foundStudent.Id);

        if (foundStudentApplication is null)
        {
            return BadRequest($"Student Application ${foundStudentApplication} not found in database");
        }

        var application = foundStudentApplication.Application;
        application.Status = ApplicationStatuses.Editing;
        
        _context.Update(application); 
        await _context.SaveChangesAsync();

        return Ok();
    }
}