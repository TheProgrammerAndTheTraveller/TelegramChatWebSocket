using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TelegramChat.Domain;

namespace TelegramChat.Data
{
    public class TelegramChatDbContext: DbContext
    {
        public DbSet<MessageHistory> MessageHistories { get; set; }
        public TelegramChatDbContext(DbContextOptions<TelegramChatDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
