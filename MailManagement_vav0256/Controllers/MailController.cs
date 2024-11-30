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

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!IsAuthorized())
            {
                return Unauthorized();
            }

            var mails = _mailService.GetAllMails();
            return Ok(mails);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            if (!IsAuthorized())
            {
                return Unauthorized();
            }

            var mail = _mailService.GetMailById(id);
            if (mail == null)
                return NotFound();

            return Ok(mail);
        }

        [HttpPost]
        public IActionResult Create([FromBody] MailCreateDto mailDto)
        {
            if (!IsAuthorized("Receptionist"))
            {
                return Forbid();
            }

            var mail = _mailService.CreateMail(mailDto);
            return CreatedAtAction(nameof(GetById), new { id = mail.Id }, mail);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] MailUpdateDto mailDto)
        {
            if (!IsAuthorized("Receptionist"))
            {
                return Forbid();
            }

            var updated = _mailService.UpdateMail(id, mailDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!IsAuthorized("Receptionist"))
            {
                return Forbid();
            }

            var deleted = _mailService.DeleteMail(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private bool IsAuthorized(string requiredRole = null)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            var authData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeader));
            var role = authData.Split(':')[1];

            if (requiredRole != null)
                return role == requiredRole;

            return true;
        }
    }
}