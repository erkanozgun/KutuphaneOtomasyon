using Kutuphane.Application.Dtos.ContactDtos;
using Kutuphane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Interfaces.Services
{
    public interface IContactService
    {
        Task SendMessageAsync(ContactMessageDto dto, int? userId);
        Task<IEnumerable<ContactMessage>> GetUnreadMessagesAsync();
        Task ReplyToMessageAsync(int id, string replyMessage);
        Task<List<ContactMessage>> GetMessagesByUserIdAsync(int userId);

        Task<IEnumerable<ContactMessage>> GetAllMessagesAsync(); 
        Task<ContactMessage> GetMessageByIdAsync(int id);        
        Task DeleteMessageAsync(int id);                         
        Task MarkAsReadAsync(int id);
    }
}
