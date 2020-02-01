namespace Resonance.Outbox.Storage
{
    public enum ReadMode
    {
        Undefined = 0,
        ReadThenMarkAsRead = 1,
        ReadThenDelete = 2
    }
}
