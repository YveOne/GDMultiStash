using System;
using System.Linq;
using System.Windows.Forms;

namespace GDIALib.GDHook
{

    public class ModeStatusHandler : IMessageProcessor
    {
        private readonly MessageType[] Relevants = new MessageType[] {
            MessageType.TYPE_GameInfo_IsHardcore,
            MessageType.TYPE_GameInfo_IsHardcore_via_init,
        };

        private readonly MessageType[] Errors = new MessageType[] {
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
                switch (type)
                {
                    case MessageType.TYPE_GameInfo_IsHardcore:
                    case MessageType.TYPE_GameInfo_IsHardcore_via_init:
                        RuntimeSettings.IsHardcore = data[0] == 1;
                        break;
                }
            }
        }
    }
}
