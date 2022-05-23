
namespace GDIALib.GDHook
{
    public interface IMessageProcessor
    {
        void Process(MessageType type, byte[] data, string dataString);
    }
}
