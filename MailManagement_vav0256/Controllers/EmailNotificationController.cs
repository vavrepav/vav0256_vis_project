using Microsoft.AspNetCore.Mvc;
using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.DTOs.EmailNotification;

namespace MailManagement_vav0256.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailNotificationController : ControllerBase
    {
        private readonly IEmailNotificationService _emailNotificationService;

        public EmailNotificationController(IEmailNotificationService emailNotificationService)
        {
            _emailNotificationService = emailNotificationService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var notifications = _emailNotificationService.GetAllNotifications();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var notification = _emailNotificationService.GetNotificationById(id);
            if (notification == null)
                return NotFound();

            return Ok(notification);
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
                return role == requiredRole || role == "Administrator";

            return true;
        }
    }
}