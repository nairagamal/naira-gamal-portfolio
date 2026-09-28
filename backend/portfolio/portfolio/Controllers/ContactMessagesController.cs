using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio.DTOs;
using portfolio.Interfaces;
using portfolio.Services;

namespace portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactMessagesController : ControllerBase
    {
        private readonly IContactMessageService _contactMessageService;

        public ContactMessagesController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactMessageDto messageDto)
        {
            var message = await _contactMessageService.CreateContactMessageAsync(messageDto);
            return Ok(new { message = "Message sent successfully!", id = message.Id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _contactMessageService.GetAllContactMessagesAsync();
            return Ok(messages);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await _contactMessageService.GetContactMessageByIdAsync(id);
            if (message == null)
                return NotFound(new { message = "Message not found" });
            return Ok(message);
        }

        [Authorize]
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _contactMessageService.MarkAsReadAsync(id);
            if (!result)
                return NotFound(new { message = "Message not found" });
            return Ok(new { message = "Message marked as read" });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactMessageService.DeleteContactMessageAsync(id);
            if (!result)
                return NotFound(new { message = "Message not found" });
            return NoContent();
        }
    }
}