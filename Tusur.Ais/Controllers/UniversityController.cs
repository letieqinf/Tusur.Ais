using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.UniversityStructure;
using Tusur.Ais.Models.Request;

namespace Tusur.Ais.Controllers;

public class UniversityController : Controller
{
    private readonly ApplicationDbContext _context;

    public UniversityController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("add-recruitment-year")]
    public async Task<IActionResult> AddRecruitmentYear([FromBody] CreateRecruitmentYearRequestModel model, 
        [FromBody] CreateFacultyRequestModel facultyModel)
    {
        var foundEducationPlan = await _context.StudyPlans
            .FirstOrDefaultAsync(e => e.FacultyName == facultyModel.FacultyName);

        if (foundEducationPlan is null)
        {
            return BadRequest($"EducationPlan ${foundEducationPlan} not found in database");
        }
        
        var foundStudyPlan = await _context.StudyPlans
            .FirstOrDefaultAsync(e => e.FacultyName == foundEducationPlan.FacultyName);

        if (foundStudyPlan is null)
        {
            return BadRequest($"StudyPlan ${foundStudyPlan} not found in database");
        }

        var foundRecruitmentYear = await _context.RecruitmentYears
            .FirstOrDefaultAsync(r => r.Year.Year == model.Year.Year);
        
        if (foundRecruitmentYear is not null)
        {
            return BadRequest($"RecruitmentYear ${foundRecruitmentYear.Year.Year} already added in database");
        }

        var recruitmentYear = new RecruitmentYear
        {
            Id = Guid.NewGuid(),
            StudyPlanId = foundStudyPlan.Id, 
            Year = model.Year,
            StudyPlan = foundStudyPlan
        };
        
        var result = await _context.AddAsync(recruitmentYear);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-study-profile")]
    public async Task<IActionResult> AddStudyProfile([FromBody] CreateStudyProfileRequestModel model)
    {
        var foundStudyField = await _context.StudyFields
            .FirstOrDefaultAsync(s => s.Name == model.StudyFieldName);
        
        if (foundStudyField is null)
        {
            return BadRequest($"StudyField ${model.Name} doesn't added in database");
        }

        var foundStudyProfile = await _context.StudyProfiles
            .FirstOrDefaultAsync(s => s.StudyFieldName == foundStudyField.Name && s.Name == model.Name);

        if (foundStudyProfile is not null)
        {
            return BadRequest($"StudyProfile ${foundStudyField.Name} already added in database");
        }
        
        var studyProfile = new StudyProfile
        {
            Id = Guid.NewGuid(),
            StudyFieldName = foundStudyField.Name,
            Name = model.Name,
            StudyField = foundStudyField
        };
        
        var result = await _context.AddAsync(studyProfile);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _context.SaveChangesAsync();
        
        return Ok();
    }

    
    [HttpPost]
    [Route("add-study-plan")]
    public async Task<IActionResult> AddStudyPlan([FromBody] CreateStudyPlanRequestModel model)
    {
        var foundFaculty = await _context.Faculties
            .FirstOrDefaultAsync(s => s.Name == model.FacultyName);

        if (foundFaculty is null)
        {
            return BadRequest($"Faculty ${model.FacultyName} doesn't added in database");
        }
        
        var foundStudyProfile = await _context.StudyProfiles
            .FirstOrDefaultAsync(s => s.Name == model.StudyProfileName);

        if (foundStudyProfile is null)
        {
            return BadRequest($"StudyProfile ${foundStudyProfile?.Name} doesn't added in database");
        }
        
        var foundStudyPlan = await _context.StudyPlans
            .FirstOrDefaultAsync(s => s.FacultyName == foundFaculty.Name);

        if (foundStudyPlan is not null)
        {
            return BadRequest($"StudyPlan ${foundStudyPlan} already added in database");
        }

        var studyPlan = new StudyPlan
        {
            Id = Guid.NewGuid(),
            StudyProfileId = foundStudyProfile.Id, 
            FacultyName = foundFaculty.Name,
            //Form = forms // ??
            StudyProfile = foundStudyProfile,
            Faculty = foundFaculty,
        };
        
        var result = await _context.AddAsync(studyPlan);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-study-field")]
    public async Task<IActionResult> AddStudyField([FromBody] CreateStudyFieldRequestModel model)
    {
        var foundStudyField = await _context.StudyFields
            .FirstOrDefaultAsync(s => s.Name == model.StudyFieldName);

        if (foundStudyField is not null)
        {
            return BadRequest($"StudyField ${foundStudyField.Name} already added in database");
        }
        
        var studyField = new StudyField
        {
            Name = model.StudyFieldName
        };
        
        var result = await _context.AddAsync(studyField);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost]
    [Route("add-faculty")]
    public async Task<IActionResult> AddFaculty([FromBody] CreateFacultyRequestModel model)
    {
        var foundFaculty = await _context.Faculties
            .FirstOrDefaultAsync(f => f.Name == model.FacultyName);

        if (foundFaculty is not null)
        {
            return BadRequest($"Faculty ${foundFaculty.Name} already added in database");
        }
        
        var faculty = new Faculty
        {
            Name = model.FacultyName
        };
        
        var result = await _context.AddAsync(faculty);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost("add-department")]
    public async Task<IActionResult> AddDepartment([FromBody] CreateFacultyRequestModel facultyModel, 
        [FromBody] CreateDepartmentRequestModel departmentModel)
    {
        var foundFaculty = await _context.Faculties
            .FirstOrDefaultAsync(f => f.Name == facultyModel.FacultyName);

        if (foundFaculty is null)
        {
            return BadRequest($"Not found ${facultyModel.FacultyName} in database");
        }

        var foundDepartment = await _context.Departments
            .FirstOrDefaultAsync(d => d.FacultyName == foundFaculty.Name && d.Name == departmentModel.Name);

        if (foundDepartment is not null)
        {
            return BadRequest($"Department ${foundDepartment.Name} already added in database");
        }

        var department = new Department
        {
            Id = Guid.NewGuid(),
            FacultyName = foundFaculty.Name,
            Name = departmentModel.Name,
            Faculty = foundFaculty
        };
        
        var result = await _context.AddAsync(department);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost("add-group")]
    public async Task<IActionResult> AddGroup([FromBody] CreateRecruitmentYearRequestModel recruitmentYear, 
        [FromBody] CreateDepartmentRequestModel department, [FromBody] CreateGroupRequestModel groupModel)
    {
        var foundRecruitmentYear = await _context.RecruitmentYears
            .FirstOrDefaultAsync(r => r.Year.Year == recruitmentYear.Year.Year);

        if (foundRecruitmentYear is null)
        {
            return BadRequest($"RecruitmentYear ${recruitmentYear} not found in database");
        }
        
        var foundDepartment = await _context.Departments
            .FirstOrDefaultAsync(d => d.Name == department.Name);

        if (foundDepartment is null)
        {
            return BadRequest($"Department ${department.Name} not found in database");
        }
        
        var foundGroup = await _context.Groups
            .FirstOrDefaultAsync(g => g.RecruitmentYearId == foundRecruitmentYear.Id 
                                 && g.DepartmentId == foundDepartment.Id && g.Name == groupModel.Name);

        if (foundGroup is not null)
        {
            return BadRequest($"Group ${foundGroup.Name} already added in database");
        }
        
        var group = new Group
        {
            Id = Guid.NewGuid(),
            RecruitmentYearId = foundRecruitmentYear.Id,
            DepartmentId = foundDepartment.Id,
            Name = groupModel.Name,
            RecruitmentYear = foundRecruitmentYear,
            Department = foundDepartment
        };
        
        var result = await _context.AddAsync(group);
        if (result.State != EntityState.Added)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}