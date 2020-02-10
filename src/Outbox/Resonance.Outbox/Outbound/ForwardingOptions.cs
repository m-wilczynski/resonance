using System;
using Resonance.Outbox.Storage;

namespace Resonance.Outbox.Outbound
{
    public class ForwardingOptions
    {
        public ForwardingOptions(
            ReadMode readMode, 
            bool logCompletionInsideTransaction = false, 
            uint? batchSize = null)
        {
            if (readMode == ReadMode.Undefined) throw new ArgumentException($"{nameof(ReadMode)} is undefined!");
            ReadMode = readMode;
            LogCompletionInsideTransaction = logCompletionInsideTransaction;
            BatchSize = batchSize;
        }

        public uint? BatchSize { get; private set; }
        public ReadMode ReadMode { get; private set; }
        public bool LogCompletionInsideTransaction { get; private set; }
    }
}
