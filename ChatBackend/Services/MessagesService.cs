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
            .Include(m => m.Sender) // Include the Sender entity
            .Include(m => m.Receiver) // Include the Receiver entity
            .Where(m => (m.Sender.Id == user1id && m.Receiver.Id == user2id) || (m.Sender.Id == user2id && m.Receiver.Id == user1id))
            .OrderBy(m => m.SentAt) // Order messages by SentAt
            .Select(m => new MessageDTOs // Map each Message entity to a MessageDTO object
            {
                Id = m.Id,
                SenderId = m.Sender.Id, // Map the Sender's Id
                ReceiverId = m.Receiver.Id, // Map the Receiver's Id
                Content = m.Content,
                SentAt = m.SentAt
            })
                .ToListAsync();

            return messages;
        }
    }
}