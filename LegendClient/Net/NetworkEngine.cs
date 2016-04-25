using Microsoft.Xna.Framework;
using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsClient.Net.Packets;
using WindowsClient.World.Mobiles;
using WindowsClient.World;

namespace WindowsClient.Net
{
    public class NetworkEngine
    {
        internal bool Connected { get { return socketClient.State == State.Connected; } }
        internal uint Ticks { get; set; }
        public ClientWorldState WorldState { get; internal set; }

        private SocketClient socketClient;
        private ClientPacketHandler[] clientPacketHandlers;

        public NetworkEngine()
        {
            clientPacketHandlers = new ClientPacketHandler[byte.MaxValue];
            clientPacketHandlers[(byte)PacketIdentity.UpdateMobile] = new UpdateMobilePacketHandler();
            PacketFactory.Register(PacketIdentity.UpdateMobile, () => new UpdateMobilePacket());
            socketClient = new SocketClient();
            socketClient.ProcessPacket += SocketClient_ProcessPacket;
        }

        //public event EventHandler HandlePacket;
        private void SocketClient_ProcessPacket(object sender, EventArgs e)
        {
            Packet packet = (Packet)sender;
            ClientPacketHandler handler = this.GetHandler(packet.PacketId);
            handler.Handle(packet, this.WorldState);
        }

        public ClientPacketHandler GetHandler(PacketIdentity packetId)
        {
            return clientPacketHandlers[(byte)packetId];
        }

        public void Initialize()
        {
            try
            {
                socketClient.Connect(LegendClient.Properties.Settings.Default.ServerAddress, LegendClient.Properties.Settings.Default.ServerPort);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void LoadContent()
        { 
        }
        public void UnloadContent()
        {
            socketClient.Disconnect();
        }

        public void Update()
        {
            socketClient.Process();
            if (moveToPacket != null)
            {
                var toSendPacket = moveToPacket;
                moveToPacket = null;
                socketClient.Send(toSendPacket);
            }
            if (aimToPacket != null)
            {
                var toSendPacket = aimToPacket;
                aimToPacket = null;
                socketClient.Send(toSendPacket);
            }
        }

        //public void SendInputUpdate(ClientCharacter clientCharacter)
        //{
        //    InputPacket inputPacket = new InputPacket();
        //    //inputPacket.Tick = this.Ticks;
        //    inputPacket.Up = clientCharacter.Up;
        //    inputPacket.Down = clientCharacter.Down;
        //    inputPacket.Left = clientCharacter.Left;
        //    inputPacket.Right = clientCharacter.Right;

        //    socketClient.Send(inputPacket);
        //}

        internal void SelectCharacter(int id)
        {
            SelectCharacterPacket packet = new SelectCharacterPacket(id);
            socketClient.Send(packet);
        }

        private MoveToPacket moveToPacket;
        private AimToPacket aimToPacket;

        internal void MoveTo(Point moveToPoint)
        {
            moveToPacket = new MoveToPacket(moveToPoint);
            //socketClient.Send(packet);
        }

        internal void AimTo(Point aimToPoint)
        {
            aimToPacket = new AimToPacket(aimToPoint);
            //socketClient.Send(packet);
        }

        internal void PerformSwing(ClientCharacter playerCharacter)
        {
            SwingPacket packet = new SwingPacket(playerCharacter.Id, AbilityIdentity.Swing);
            socketClient.Send(packet);
        }

        internal void PerformHeal(ClientCharacter playerCharacter)
        {
            SwingPacket packet = new SwingPacket(playerCharacter.Id, AbilityIdentity.Heal);
            socketClient.Send(packet);
        }
    }
}
