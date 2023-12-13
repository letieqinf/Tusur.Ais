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

            var contracts = await user.GetContractsAsync(_userManager, _context);

            return Ok(contracts);
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

        [Authorize(Roles = $"{UserRoles.Teacher}, {UserRoles.EducationDepartment}, {UserRoles.Secretary}")]
        [HttpPost]
        [Route("approve-contract")]
        public async Task<IActionResult> ApproveContract([FromBody] ApproveContractRequestModel model)
        {
            var jwtTokenClaims = Request.GetJwtTokenClaims();
            var roles = jwtTokenClaims.GetRolesFromClaims();

            if (roles.Contains(UserRoles.Teacher))
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

                if (foundApplication.Contract?.Company is { Status: CompanyConfirmationStatuses.InProcess })
                {
                    foundApplication.Contract.Company.Status = CompanyConfirmationStatuses.Confirmed;
                }

                _context.Update(foundApplication);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else if (roles.Contains(UserRoles.EducationDepartment))
            {
                try
                {
                    // Поиск документа в базе данных по его идентификатору
                    var application = await _context.Applications.FirstOrDefaultAsync(a => a.Id == model.ApplicationId);

                    // Проверка, найден ли документ
                    if (application == null)
                    {
                        return NotFound("Документ не найден");
                    }

                    // Проверка статуса документа перед утверждением, если не отправлен на согласование - ошибка
                    if (application.Status != ApplicationStatuses.Sent)
                    {
                        return BadRequest("Нельзя утвердить документ в текущем статусе");
                    }

                    // Добавление номера и даты договора к документу
                    application.Contract.Number = model.ContractNumber;
                    application.Contract.Date = model.ContractDate;
                    // Изменение статуса документа на подтвержденный уч. управлением
                    application.Status = ApplicationStatuses.ApprovedByEducationDepartment;

                    // Сохранение изменений в базе данных
                    await _context.SaveChangesAsync();

                    return Ok($"Документ успешно утвержден. Индивидуальный номер договора: {model.ContractNumber}");
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
            else if (roles.Contains(UserRoles.Secretary))
            {
                try
                {
                    // Взял id дока из модели, налачал искать документ в бд по его идентификатору
                    var application = await _context.Applications.FirstOrDefaultAsync(a => a.Id == model.ApplicationId);

                    // Проверка, найден ли документ
                    if (application == null)
                    {
                        return NotFound("Документ не найден");
                    }

                    // Проверка статуса документа перед утверждением, если согласован руководителем учебного управления
                    if (application.Status == ApplicationStatuses.ApprovedByEducationDepartment)
                    {
                        // Изменение статуса документа на принятый секретарем
                        application.Status = ApplicationStatuses.AcceptedBySecretary;
                    }
                    // Если он уже был принят секретарем
                    else if (application.Status == ApplicationStatuses.AcceptedBySecretary)
                    {
                        // Изменение статуса документа на отправлен в центр карьеры
                        application.Status = ApplicationStatuses.SentToCareerCenter;
                    }
                    else
                    {
                        return BadRequest("Нельзя утвердить документ в текущем статусе");
                    }

                    // Сохранение изменений в бд
                    await _context.SaveChangesAsync();

                    return Ok($"Документ успешно утвержден.");
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }

            return Forbid();
        }

        [Authorize(Roles = UserRoles.Teacher)]
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

        [Authorize(Roles = UserRoles.Student)]
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
