using Kutuphane.Application.Dtos.ContactDtos;
using Kutuphane.Application.Interfaces.Repositories;
using Kutuphane.Application.Interfaces.Services;
using Kutuphane.Domain.Entities;

namespace Kutuphane.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactMessageRepository _contactRepository;

        public ContactService(IContactMessageRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IEnumerable<ContactMessage>> GetUnreadMessagesAsync()
        {
            return await  _contactRepository.GetUnreadMessagesAsync();
        }

        public async Task SendMessageAsync(ContactMessageDto dto, int? userId)
        {
            var message = new ContactMessage
            {
                Name = dto.Name,
                Email = dto.Email,
                Subject = dto.Subject,
                MessageType = dto.MessageType,
                Message = dto.Message,
                UserId = userId,
                CreatedDate = DateTime.Now,
                IsRead = false
            };

            await _contactRepository.AddAsync(message);
        }
        public async Task ReplyToMessageAsync(int id, string replyMessage)
        {
            var message = await _contactRepository.GetByIdAsync(id);
            if (message != null)
            {
                message.AdminReplay = replyMessage;   
                message.ReplayDate = DateTime.Now;   
                message.IsRead = true;                

                await _contactRepository.UpdateAsync(message);
            }
        }

        public async Task<List<ContactMessage>> GetMessagesByUserIdAsync(int userId)
        {
            var messages = await _contactRepository.GetMessagesByUserIdAsync(userId);
            return messages.ToList();
        }
        
        public async Task<IEnumerable<ContactMessage>> GetAllMessagesAsync()
        {
          
            var messages = await _contactRepository.GetAllAsync();
            return messages.OrderByDescending(x => x.CreatedDate);
        }


        public async Task<ContactMessage> GetMessageByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }

    
        public async Task MarkAsReadAsync(int id)
        {
            var message = await _contactRepository.GetByIdAsync(id);
            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                await _contactRepository.UpdateAsync(message);
            }
        }

       
        public async Task DeleteMessageAsync(int id)
        {
            var message = await _contactRepository.GetByIdAsync(id);
            if (message != null)
            {
                message.IsDeleted = true;
                await _contactRepository.UpdateAsync(message);
            }
        }
    }
}