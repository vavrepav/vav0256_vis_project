using Microsoft.AspNetCore.Mvc;
using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.DTOs.Mail;

namespace MailManagement_vav0256.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IUserService _userService;

        public MailController(IMailService mailService, IUserService userService)
        {
            _mailService = mailService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var (isAuthorized, email, role) = IsAuthorized();
            if (!isAuthorized)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }

            IEnumerable<MailReadDto> mails;
            if (role is "Receptionist" or "Administrator")
            {
                mails = _mailService.GetAllMails();
            }
            else
            {
                var user = _userService.GetUserByEmail(email);
                mails = _mailService.GetMailsByRecipientId(user.Id);
            }

            return Ok(mails);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var (isAuthorized, email, role) = IsAuthorized();
            if (!isAuthorized)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }

            var mail = _mailService.GetMailById(id);
            if (mail == null)
                return NotFound();

            if (role is not ("Receptionist" or "Administrator")) return Ok(mail);
            var user = _userService.GetUserByEmail(email);
            if (mail.RecipientId == user.Id) return Ok(mail);
            Response.StatusCode = 403;
            return new EmptyResult();

        }

        [HttpPost]
        public IActionResult Create([FromBody] MailCreateDto mailDto)
        {
            var (isAuthorized, email, role) = IsAuthorized("Receptionist");
            if (!isAuthorized)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }

            var receptionist = _userService.GetUserByEmail(email);
            if (receptionist == null)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }

            var mail = _mailService.CreateMail(mailDto, receptionist.Id);
            return CreatedAtAction(nameof(GetById), new { id = mail.Id }, mail);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] MailUpdateDto mailDto)
        {
            var (isAuthorized, _, role) = IsAuthorized("Receptionist");
            if (!isAuthorized)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }

            var updated = _mailService.UpdateMail(id, mailDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/Claim")]
        public IActionResult ClaimMail(Guid id, [FromBody] MailClaimDto claimDto)
        {
            var (isAuthorized, _, role) = IsAuthorized("Receptionist");
            if (!isAuthorized)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }

            var success = _mailService.ClaimMail(claimDto, id);
            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var (isAuthorized, _, role) = IsAuthorized("Receptionist");
            if (!isAuthorized)
            {
                Response.StatusCode = 403;
                return new EmptyResult();
            }

            var deleted = _mailService.DeleteMail(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private (bool isAuthorized, string email, string role) IsAuthorized(string? requiredRole = null)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.StatusCode = 403;
                return (false, null, default)!;
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            try
            {
                var authData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeader));
                var parts = authData.Split(':');
                if (parts.Length < 2)
                {
                    Response.StatusCode = 403;
                    return (false, null, default)!;
                }

                var email = parts[0];
                var role = parts[1];

                if (requiredRole == null || role.Equals(requiredRole, StringComparison.OrdinalIgnoreCase) ||
                    role == "Administrator") return (true, email, role);
                Response.StatusCode = 403;
                return (false, null, default)!;

            }
            catch
            {
                Response.StatusCode = 403;
                return (false, null, default)!;
            }
        }
    }
}