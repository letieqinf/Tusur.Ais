using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data.Entities.CompanyStructure;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Extensions;
using Tusur.Ais.Models.Defaults;
using Tusur.Ais.Models.Request;
using Tusur.Ais.Models.Response;

namespace Tusur.Ais.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/applications")]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ApplicationController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("contracts")]
        public async Task<IActionResult> GetContracts()
        {
            var jwtTokenClaims = Request.GetJwtTokenClaims();
            var email = jwtTokenClaims.GetEmailFromClaims();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var contracts = new List<GetContractsResponseModel>();

            if (await _userManager.IsInRoleAsync(user, UserRoles.Student))
                contracts = await GetStudentContracts(user, contracts);
            
            var contractApplications = new List<Contract>();

            if (await _userManager.IsInRoleAsync(user, UserRoles.Teacher))
            {
                var applications = _context.Applications
                    .Where(a => a.Status == ApplicationStatuses.Sent)
                    .Select(a => a.Id);
                
                foreach (var application in applications)
                {
                    //return Ok(application);
                    var contract = await _context.Contracts.FirstOrDefaultAsync(c => c.ApplicationId == application);
                    if (contract != null)
                    {
                        contractApplications.Add(contract);

                        // if (contract!.TeacherId != user.Id)
                        //     contractApplications.Add(contract);
                    }
                }
                
                return Ok(contractApplications);
            }


            if (contractApplications is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(contractApplications);
        }

        private async Task<List<GetContractsResponseModel>> GetStudentContracts(User user, List<GetContractsResponseModel> contracts)
        {
            var group = await _context.GetStudentGroupAsync(user);
            var year = await _context.GetStudentRecruitmentYearAsync(user);
            var studyProfile = await _context.GetStudentStudyProfileAsync(user);
            var studyForm = await _context.GetStudentStudyFormAsync(user);

            var applications = _context.StudentApplications
                .Where(sa => sa.StudentId == user.Id)
                .Select(sa => sa.ApplicationId);

            foreach (var application in applications)
            {
                var app = await _context.Applications.FindAsync(application);
                var contract = await _context.Contracts.FirstOrDefaultAsync(c => c.ApplicationId == application);
                var teacher = await _context.Users.FirstOrDefaultAsync(t => t.Id == contract!.TeacherId);
                var signatory = await _context.Signatories.FirstOrDefaultAsync(s => s.Id == contract!.SignatoryId);
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == contract!.CompanyId);
                var contactFace = await _context.ContactFaces.FirstOrDefaultAsync(c => c.Id == contract!.ContactFaceId);

                contracts.Add(new GetContractsResponseModel
                {
                    ContractId = contract!.ApplicationId,
                    Number = contract!.Number,
                    Date = contract!.Date,
                    StudentDescription = new GetContractsResponseModel.Student
                    {
                        Name = user.Name!,
                        LastName = user.LastName!,
                        Patronymic = user.Patronymic,
                        Group = group.Name,
                        StudyField = studyProfile.StudyFieldName,
                        StudyProfile = studyProfile.Name,
                        StudyForm = studyForm,
                        RecruitmentYear = year.Year
                    },
                    TeacherDescription = new GetContractsResponseModel.Teacher
                    {
                        Name = teacher!.Name!,
                        LastName = teacher!.LastName!,
                        Patronymic = teacher!.Patronymic
                    },
                    SignatoryDescription = new GetContractsResponseModel.Signatory
                    {
                        Name = signatory!.Name,
                        LastName = signatory.LastName,
                        Patronymic = signatory.Patronymic,
                        JobTitle = signatory.JobTitle!
                    },
                    CompanyDescription = new GetContractsResponseModel.Company
                    {
                        Inn = company!.Inn,
                        Name = company!.Name,
                        ShortName = company!.ShortName,
                        Address = company!.Address,
                        DirectorName = company!.DirectorName,
                        DirectorLastName = company!.DirectorLastName,
                        DirectorPatronymic = company!.DirectorPatronymic,
                        Status = company!.Status
                    },
                    ContactFaceDescription = new GetContractsResponseModel.ContactFace
                    {
                        Name = contactFace!.Name,
                        LastName = contactFace!.LastName,
                        Patronymic = contactFace.Patronymic,
                        JobTitle = contactFace.JobTitle
                    },
                    ApplicationStatus = app!.Status
                });
            }

            return contracts;
        }

        [Authorize(Roles = UserRoles.Student)]
        [HttpGet]
        [Route("contracts/get-contract-basics")]
        public async Task<IActionResult> GetContractBasics()
        {
            var jwtTokenClaims = Request.GetJwtTokenClaims();
            var email = jwtTokenClaims.GetEmailFromClaims();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            IEnumerable<User> teachers;

            try
            {
                var group = await _context.GetStudentGroupAsync(user);

                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == group.DepartmentId);
                if (department is null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                teachers = await _context.GetDepartmentTeachersAsync(department);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            if (teachers is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var output = teachers.ToList()
                            .Select(t => new GetContractBasicsResponseModel
                            {
                                UserName = t.UserName,
                                FullName = new GetContractBasicsResponseModel.Information 
                                { 
                                    Name = t.Name!, 
                                    LastName = t.LastName!, 
                                    Patronymic = t.Patronymic 
                                }
                            });

            return Ok(output);
        }
        
        [HttpPost]
        [Route("approve-contract")]
        public async Task<IActionResult> ApproveContract([FromBody] ApproveContractRequestModel model)
        {
            var foundApplication = await _context.Applications
                .FirstOrDefaultAsync(t => t.Id == model.ApplicationId);

            if (foundApplication is null)
            {
                return BadRequest($"Application ${foundApplication} not found in database");
            }
            
            if (foundApplication.Status is ApplicationStatuses.Sent) 
            {
                foundApplication.Status = ApplicationStatuses.ApprovedByTeacher;
            }

            //return Ok("good");
            
            if (foundApplication.Contract?.Company is { Status: CompanyConfirmationStatuses.InProcess }) 
            {
                foundApplication.Contract.Company.Status = CompanyConfirmationStatuses.Confirmed;
            }
        
            _context.Update(foundApplication); 
            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPost]
        [Route("sending-for-revision")]
        public async Task<IActionResult> SendingForRevision([FromBody] ApproveContractRequestModel model)
        {
            var foundApplication = await _context.Applications
                .FirstOrDefaultAsync(t => t.Id == model.ApplicationId);

            if (foundApplication is null)
            {
                return BadRequest($"Application ${foundApplication} not found in database");
            }

            foundApplication.Status = ApplicationStatuses.Editing;
            _context.Update(foundApplication); 
            await _context.SaveChangesAsync();

            return Ok();
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("contracts/create-contract")]
        public async Task<IActionResult> CreateContract([FromBody] CreateContractRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teacher = await _userManager.FindByNameAsync(model.TeacherUserName);
            if (teacher is null)
                return BadRequest(ModelState);

            EntityEntry result;

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Inn == model.Inn);
            if (company is null)
            {
                company = new Company
                {
                    Id = Guid.NewGuid(),
                    Inn = model.Inn,
                    Name = model.CompanyName,
                    ShortName = model.CompanyShortName,
                    Address = model.CompanyAddress,
                    PhoneNumber = model.CompanyPhoneNumber,
                    Email = model.CompanyEmail,
                    DirectorName = model.CompanyDirectorName,
                    DirectorLastName = model.CompanyDirectorLastName,
                    DirectorPatronymic = model.CompanyDirectorPatronymic,
                    Status = CompanyConfirmationStatuses.InProcess
                };

                result = await _context.AddAsync(company);
                if (result.State != EntityState.Added)
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var contactFace = new ContactFace
            {
                Id = Guid.NewGuid(),
                CompanyId = company.Id,
                Name = model.ContactFaceName,
                LastName = model.ContactFaceLastName,
                Patronymic = model.ContactFacePatronymic,
                JobTitle = model.ContactFaceJobTitle,
                PhoneNumber = model.ContactFacePhoneNumber, 
                Email = model.ContactFaceEmail
            };
            result = await _context.AddAsync(contactFace);
            if (result.State != EntityState.Added)
                return StatusCode(StatusCodes.Status500InternalServerError);

            Signatory signatory;

            try
            {
                signatory = await _context.GetSignatory();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            var application = new Application
            {
                Id = Guid.NewGuid(),
                Status = ApplicationStatuses.Sent
            };
            result = await _context.AddAsync(application);
            if (result.State != EntityState.Added)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var contract = new Contract
            {
                ApplicationId = application.Id,
                CompanyId = company.Id,
                ContactFaceId = contactFace.Id,
                SignatoryId = signatory.Id,
                TeacherId = teacher.Id
            };

            result = await _context.AddAsync(contract);
            if (result.State != EntityState.Added)
                return StatusCode(StatusCodes.Status500InternalServerError);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // 0 Показать все документы
        // 2 Преподаватель
        // 2.1 Получение договора
        // 2.2 Подтверждение договора и подтверждение компании
        // 3 Учебное управление
        // 3.1 Подтверждение и присвоение уникального номера
        // 4 Секретарь
        // ЗДЕСЬ ДАЕТСЯ WORD-ДОКУМЕНТ
        // далее только изменение статуса
    }
}
