using System;
using System.Linq;
using System.Windows.Forms;

namespace GDIALib.GDHook
{
    public class StashStatusHandler : IMessageProcessor
    {
        private readonly MessageType[] Relevants = new MessageType[] {
            MessageType.TYPE_OPEN_CLOSE_TRANSFER_STASH,
            MessageType.TYPE_SAVE_TRANSFER_STASH,
            //MessageType.TYPE_OPEN_PRIVATE_STASH,
            //MessageType.TYPE_InventorySack_Sort,
            //MessageType.TYPE_ERROR_HOOKING_SAVETRANSFER_STASH,
            //MessageType.TYPE_DISPLAY_CRAFTER,
        };

        private readonly MessageType[] Errors = new MessageType[] {
            MessageType.TYPE_ERROR_HOOKING_PRIVATE_STASH,
            MessageType.TYPE_ERROR_HOOKING_TRANSFER_STASH,
            MessageType.TYPE_ERROR_HOOKING_SAVETRANSFER_STASH
        };

        private enum InternalStashStatus
        {
            Open, Closed, Unknown
        };

        private InternalStashStatus TransferStashStatus = InternalStashStatus.Unknown;

        private bool IsClosed => TransferStashStatus == InternalStashStatus.Closed;

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
                if (data.Length != 1 && type == MessageType.TYPE_OPEN_CLOSE_TRANSFER_STASH)
                    Console.WriteLine("[GDHook] Received a Open/Close stash message from hook, expected length 1, got length {0}", data.Length);


                switch (type)
                {
                    case MessageType.TYPE_OPEN_CLOSE_TRANSFER_STASH:
                        bool isOpen = ((int)data[0]) != 0;
                        TransferStashStatus = isOpen ? InternalStashStatus.Open : InternalStashStatus.Closed;
                        break;

                    case MessageType.TYPE_SAVE_TRANSFER_STASH:
                        TransferStashStatus = InternalStashStatus.Closed;
                        break;
                }

                RuntimeSettings.StashStatus = !IsClosed ? StashAvailability.OPEN : StashAvailability.CLOSED;
            }
        }
    }

}
