using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramChat.Domain;

namespace TelegramChat.Data
{
    public class MessageHistoryConfiguration : IEntityTypeConfiguration<MessageHistory>
    {
        public void Configure(EntityTypeBuilder<MessageHistory> builder)
        {
            builder.HasKey(u => u.MessageID);
            builder.Property(u => u.MessageID).UseIdentityAlwaysColumn();
            builder.HasIndex(u => new { u.Id1, u.Id2 });
        }
    }
}
