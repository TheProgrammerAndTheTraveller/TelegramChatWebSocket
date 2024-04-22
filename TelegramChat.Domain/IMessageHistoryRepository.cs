namespace TelegramChat.Domain
{
    public interface IMessageHistoryRepository
    {
        Task Add(long id1, long id2, byte[] text);
    }
}