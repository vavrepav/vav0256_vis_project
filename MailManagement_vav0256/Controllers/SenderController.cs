using Microsoft.AspNetCore.Mvc;
using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.DTOs.Sender;

namespace MailManagement_vav0256.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SenderController : ControllerBase
    {
        private readonly ISenderService _senderService;

        public SenderController(ISenderService senderService)
        {
            _senderService = senderService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!IsAuthorized())
            {
                return Unauthorized();
            }

            var senders = _senderService.GetAllSenders();
            return Ok(senders);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            if (!IsAuthorized())
            {
                return Unauthorized();
            }

            var sender = _senderService.GetSenderById(id);
            if (sender == null)
                return NotFound();

            return Ok(sender);
        }

        [HttpPost]
        public IActionResult Create([FromBody] SenderCreateDto senderDto)
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var sender = _senderService.CreateSender(senderDto);
            return CreatedAtAction(nameof(GetById), new { id = sender.Id }, sender);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] SenderUpdateDto senderDto)
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var updated = _senderService.UpdateSender(id, senderDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var deleted = _senderService.DeleteSender(id);
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