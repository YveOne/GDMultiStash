using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GDMultiStash.GlobalHandlers
{
    partial class FileSystemHandler
    {

        public class TransferFileChangedEventArgs : EventArgs
        {
            public string FileName { get; private set; }
            public string FilePath { get; private set; }
            public TransferFileChangedEventArgs(string filePath)
            {
                FilePath = filePath;
                FileName = Path.GetFileName(filePath);
            }
        }

        public EventHandler<TransferFileChangedEventArgs> TransferFileChanged;

        public void InvokeTransferFileChanged(string filePath)
            => SaveInvoke(() => TransferFileChanged?.Invoke(this, new TransferFileChangedEventArgs(filePath)));

    }
}
