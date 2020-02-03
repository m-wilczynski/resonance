namespace Resonance.Outbox.Storage
{
    public class StorageConfiguration
    {
        public int OperationTimeoutInSeconds { get; set; } = 5;

        public string SchemaName { get; set; } = "dbo";
        public string MessageTableName { get; set; } = "messages";
        public string LogTableName { get; set; } = "logs";
        public bool InitializeTablesOnStartup { get; set; } = true;

        public bool IsComplete =>
            OperationTimeoutInSeconds > 0
            && !string.IsNullOrEmpty(SchemaName)
            && !string.IsNullOrEmpty(MessageTableName)
            && !string.IsNullOrEmpty(LogTableName);

        //TODO: Make column names configurable too
    }
}
