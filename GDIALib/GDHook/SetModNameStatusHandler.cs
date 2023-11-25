using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDIALib.GDHook
{
    internal class SetModNameStatusHandler : IMessageProcessor
    {
        private readonly MessageType[] Relevants = new MessageType[] {
            MessageType.TYPE_GameInfo_ModName,
        };

        private readonly MessageType[] Errors = new MessageType[] {
        };

        public void Process(MessageType type, byte[] data, string dataString)
        {
            if (Errors.Contains(type))
            {
                var error = Errors.FirstOrDefault(m => m == type);
                Console.WriteLine($"[GDHook] GD Hook reports error detecting {error}, unable to detect stash status");
            }
            else if (Relevants.Contains(type))
            {
                switch (type)
                {
                    case MessageType.TYPE_GameInfo_ModName:
                        //Console.WriteLine($"-------MODNAME: {dataString}");
                        RuntimeSettings.ModName = dataString;
                        break;
                }
            }
        }
    }
}
