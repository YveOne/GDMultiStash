using System;
using System.Linq;
using System.Windows.Forms;

namespace GDIALib.GDHook
{
    public class SaveTransferStashHandler : IMessageProcessor
    {
        private readonly MessageType[] Relevants = new MessageType[] {
            MessageType.TYPE_SAVE_TRANSFER_STASH,
        };

        private readonly MessageType[] Errors = new MessageType[] {
            MessageType.TYPE_ERROR_HOOKING_SAVETRANSFER_STASH,
        };

        public void Process(MessageType type, byte[] data, string dataString)
        {
            if (Errors.Contains(type))
            {
                var error = Errors.FirstOrDefault(m => m == type);

                Console.WriteLine($"[GDHook] GD Hook reports error detecting {error}, unable to detect stash status");
                MessageBox.Show("[GDHook] Alert dev - Possible version incompatibility", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Relevants.Contains(type))
            {

                RuntimeSettings.NoticeTransferStashSaved();
            }
        }
    }

}
