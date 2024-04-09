namespace TelegramChat.Domain
{
    public class MessageHistory
    {
        public long Id1 { get; set; }
        public long Id2 { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public long MessageID {  get; set; }
    }
}
