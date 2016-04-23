using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public static class PacketFactory
    {
        static readonly Dictionary<PacketIdentity, Func<IPacket>> _dict
         = new Dictionary<PacketIdentity, Func<IPacket>>(byte.MaxValue);

        private static IPacket Create(PacketIdentity packetId)
        {
            if (packetId == PacketIdentity.Invalid)
                return new Packet(packetId);

            Func<IPacket> constructor = null;
            if (_dict.TryGetValue(packetId, out constructor))
                return constructor();

            throw new ArgumentException("No type registered for this id");
        }

        public static IPacket GetPacket(byte[] buffer)
        {
            IPacket packet = PacketFactory.Create((PacketIdentity)buffer[0]);
            packet.Buffer = buffer;
            return packet;
        }

        public static void Register(PacketIdentity packetId, Func<IPacket> ctor)
        {
            _dict.Add(packetId, ctor);
        }


        //Dictionary<PacketIdentity, Type> packetTypeList;
        //public void Register<TPacket>(PacketIdentity packetID) where TPacket : Packet
        //{

        //}
        //public Packet GetPacket(byte[] buffer)
        //{
        //    PacketIdentity packetID = (PacketIdentity)buffer[0];

        //    Packet packet = this.CreatePacket<packetTypeList[packetID] >(packetID, buffer);
        //    packet.Buffer = buffer;
        //    return packet;
        //}
        //private TPacket CreatePacket<TPacket>(PacketIdentity packetID) where TPacket : Packet, new()
        //{
        //    return new TPacket();
        //}

        //private static PacketReciver<IPacket>[] Recivers = new PacketReciver<IPacket>[byte.MaxValue];
        //public static void Register<TPacket>(PacketIdentity packetID, TPacket packetReciver)
        //{
        //    Recivers[(byte)packetID] = packetReciver;
        //}

        //public static PacketReciver GetHandler(PacketIdentity packetID)
        //{
        //    return Recivers[(byte)packetID];
        //}

        //public abstract IPacket OnRecive(byte[] buffert);
    }
}
