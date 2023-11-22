using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.IntergroupRelations;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Models.Request;

namespace Tusur.Ais.Controllers;

public class EntityController : Controller
{
    private readonly ApplicationDbContext _context;

    public EntityController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("add-teacher-department")]
    public async Task<IActionResult> AddTeacherDepartment([FromBody] CreateTeacherDepartmentRequestModel model)
    {
        var foundTeacher = await _context.Teachers
            .FirstOrDefaultAsync(t => t.Id == model.TeacherId);

        if (foundTeacher is null)
        {
            return BadRequest($"Teacher ${foundTeacher} not found in database");
        }
        
        var foundDepartment = await _context.Departments
            .FirstOrDefaultAsync(d => d.Name == model.DepartmentName && d.FacultyName == model.FacultyName);

        if (foundDepartment is null)
        {
            return BadRequest($"Department ${foundDepartment} not found in database");
        }

        var foundTeacherDepartment = await _context.TeacherDepartments
            .FirstOrDefaultAsync(t => t.TeacherId == foundTeacher.Id 
                                 && t.DepartmentId == foundDepartment.Id);

        if (foundTeacherDepartment is not null)
        {
            return BadRequest($"TeacherDepartment ${foundTeacherDepartment} already added in database");
        }

        var teacherDepartment = new TeacherDepartment
        { 
            TeacherId = foundTeacher.Id,
            DepartmentId = foundDepartment.Id,
            Teacher = foundTeacher.User,
            Department = foundDepartment
        };
        
        var result = await _context.AddAsync(teacherDepartment);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-secretary")]
    public async Task<IActionResult> AddSecretary([FromBody] CreateSecretaryRequestModel model)
    {
        var foundUser = await _context.Secretaries
            .FirstOrDefaultAsync(s => s.UserId == model.UserId);
        
        if (foundUser is not null)
        {
            return BadRequest($"Secretary ${foundUser} already added in database");
        }

        var secretary = new Secretary
        { 
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            User = foundUser.User
        };
    
        var result = await _context.AddAsync(secretary);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-education-department")]
    public async Task<IActionResult> AddEducationDepartment([FromBody] CreateEducationDepartmentRequestModel model)
    {
        var foundUser = await _context.EducationDepartments
            .FirstOrDefaultAsync(s => s.UserId == model.UserId);
        
        if (foundUser is not null)
        {
            return BadRequest($"EducationDepartment ${foundUser} already added in database");
        }

        if (foundUser != null)
        {
            var educationDepartment = new EducationDepartment
            { 
                Id = Guid.NewGuid(),
                UserId = foundUser.UserId,
                User = foundUser.User
            };
        
            var result = await _context.AddAsync(educationDepartment);
            if (result.State != EntityState.Added)
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        await _context.SaveChangesAsync();
        
        return Ok();
    }
}