using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramChat.Domain;

namespace TelegramChat.Data
{
    public class MessageHistoryRepository : IMessageHistoryRepository
    {
        public readonly TelegramChatDbContext _telegramChatDbContext;

        public MessageHistoryRepository(TelegramChatDbContext telegramChatDbContext)
        {
            _telegramChatDbContext = telegramChatDbContext;
        }
        public async Task Add(long id1, long id2, string text)
        {
            var message = new MessageHistory { Id1 = id1, Id2 = id2, Text = text };
            await _telegramChatDbContext.MessageHistories.AddAsync(message);
            await _telegramChatDbContext.SaveChangesAsync();
        }
    }
}
