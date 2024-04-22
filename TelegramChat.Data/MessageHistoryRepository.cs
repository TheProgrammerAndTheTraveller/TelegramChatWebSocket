using Microsoft.EntityFrameworkCore;
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
        private readonly IDbContextFactory<TelegramChatDbContext> _contextFactoryFactory;

        public MessageHistoryRepository(IDbContextFactory<TelegramChatDbContext> contextFactoryFactory)
        {
            _contextFactoryFactory = contextFactoryFactory;
        }

        public async Task Add(long id1, long id2, byte[] text)
        {
            var dbContext = await _contextFactoryFactory.CreateDbContextAsync();
            var message = new MessageHistory { Id1 = id1, Id2 = id2, Text = text, TimeStamp = DateTime.UtcNow };
            await dbContext.MessageHistories.AddAsync(message);
            await dbContext.SaveChangesAsync();
        }
    }
}
