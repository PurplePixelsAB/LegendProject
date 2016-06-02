using Network;
using WindowsClient.World;

namespace WindowsClient.Net
{
    public abstract class ClientPacketHandler
    {
        //internal NetworkEngine Network { get; set; }
        protected abstract void OnHandle(IPacket packet, ClientWorldState worldState);
        public void Handle(IPacket packet, ClientWorldState worldState)
        {
            packet.ReadBuffer();
            this.OnHandle(packet, worldState);
        }
    }
}