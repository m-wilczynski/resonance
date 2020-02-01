namespace Resonance.Outbox.Storage
{
    public class MessageRepositoryOptions
    {
        public MessageRepositoryOptions(ReadMode readMode)
        {
            ReadMode = readMode;
        }

        public ReadMode ReadMode { get; private set; }
    }
}
