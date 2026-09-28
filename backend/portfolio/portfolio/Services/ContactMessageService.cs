using AutoMapper;
using portfolio.DTOs;
using portfolio.Entities;
using portfolio.Interfaces;

namespace portfolio.Services
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IGenericRepository<ContactMessage> _contactMessageRepository;
        private readonly IMapper _mapper;

        public ContactMessageService(IGenericRepository<ContactMessage> contactMessageRepository, IMapper mapper)
        {
            _contactMessageRepository = contactMessageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactMessageDto>> GetAllContactMessagesAsync()
        {
            var messages = await _contactMessageRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ContactMessageDto>>(messages.OrderByDescending(m => m.CreatedAt));
        }

        public async Task<ContactMessageDto?> GetContactMessageByIdAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            return message == null ? null : _mapper.Map<ContactMessageDto>(message);
        }

        public async Task<ContactMessageDto> CreateContactMessageAsync(CreateContactMessageDto messageDto)
        {
            var message = _mapper.Map<ContactMessage>(messageDto);
            message.CreatedAt = DateTime.UtcNow;
            message.IsRead = false;

            var created = await _contactMessageRepository.AddAsync(message);
            return _mapper.Map<ContactMessageDto>(created);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            if (message == null)
                return false;

            message.IsRead = true;
            message.UpdatedAt = DateTime.UtcNow;

            await _contactMessageRepository.UpdateAsync(message);
            return true;
        }

        public async Task<bool> DeleteContactMessageAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            if (message == null)
                return false;

            message.IsDeleted = true;
            message.UpdatedAt = DateTime.UtcNow;

            await _contactMessageRepository.UpdateAsync(message);
            return true;
        }
    }
}
