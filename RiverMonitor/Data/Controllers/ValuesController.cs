using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace RiverMonitor.Data.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateValue([FromBody] Value newValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Authentication token verification
            var authToken = Request.Headers["Authorization"];
            if (authToken != "123456789") //  Comparison with a valid authentication token
            {
                return Unauthorized(); // If the token is not valid, Unauthorized is returned
            }

            // Verify that a station with the given ID exists
            var station = await _context.Stations.FindAsync(newValue.StationId);
            if (station == null)
            {
                return BadRequest("StationId does not exist.");
            }

            //Control for flood and drought warnings
            bool isFloodWarning = newValue.Val >= station.FloodWarningValue;
            bool isDroughtWarning = newValue.Val <= station.DroughtWarniValue;

            _context.Values.Add(newValue);
            await _context.SaveChangesAsync();

            if (isFloodWarning)
            {
                await SendWarningEmail(
                    $"{station.Title} - Flood Warning Alert",
                    $"Flood warning level exceeded at station '{station.Title}'. Current level: {newValue.Val}.",
                    "test@example.com" // Test email for Papercut
                );
            }

            if (isDroughtWarning)
            {
                await SendWarningEmail(
                    $"{station.Title} - Drought Warning Alert",
                    $"Drought warning level exceeded at station '{station.Title}'. Current level: {newValue.Val}.",
                    "test@example.com" // Test email for Papercut
                );
            }

         
            return CreatedAtAction(nameof(GetValue), new { id = newValue.Id.ToString() }, newValue);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _context.Values.FindAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }

        // Functions for sending emails to Papercut
        private async Task SendWarningEmail(string subject, string body, string recipient)
        {
            var smtp = new SmtpClient
            {
                Host = "127.0.0.1", // Papercut running on localhost
                Port = 25, // Port for SMTP servers (typically)
                EnableSsl = false, // Test servers usually do not require SSL
                Credentials = null // Usually no authentication is needed for test servers
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("no-reply@example.com", "River Monitoring"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(new MailAddress(recipient));

            await smtp.SendMailAsync(mailMessage); // Sending an email to Papercut
        }
    }
}

