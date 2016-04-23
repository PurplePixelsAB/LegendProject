using Network.Packets;

namespace Network
{
    public interface IPacket
    {
        PacketIdentity PacketId { get; }
        byte[] Buffer { get; set; }
        int Size { get; set; }
        //uint Tick { get; set; }

        void ReadBuffer();
    }
}