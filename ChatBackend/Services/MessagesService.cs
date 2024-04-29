using Chat.Database;
using Messages.Models;
using Microsoft.EntityFrameworkCore;

namespace Messages.Services
{
    public class MessageService
    {
        private readonly AppDbContext _dbContext;

        public MessageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MessageDTOs>> GetMessagesBetweenUsersAsync(string user1id, string user2id)
        {
            var messages = await _dbContext.messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver) 
            .Where(m => (m.Sender.Id == user1id && m.Receiver.Id == user2id) || (m.Sender.Id == user2id && m.Receiver.Id == user1id))
            .OrderBy(m => m.SentAt) 
            .Select(m => new MessageDTOs 
            {
                Id = m.Id,
                SenderId = m.Sender.Id,
                ReceiverId = m.Receiver.Id, 
                Content = m.Content,
                SentAt = m.SentAt
            })
                .ToListAsync();

            return messages;
        }
    }
}