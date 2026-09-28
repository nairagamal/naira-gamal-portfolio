using portfolio.DTOs;

namespace portfolio.Interfaces
{
    public interface IContactMessageService
    {
        Task<IEnumerable<ContactMessageDto>> GetAllContactMessagesAsync();
        Task<ContactMessageDto?> GetContactMessageByIdAsync(int id);
        Task<ContactMessageDto> CreateContactMessageAsync(CreateContactMessageDto messageDto);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteContactMessageAsync(int id);
    }
}
