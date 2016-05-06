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
using LegendWorld.Data.Abilities;
using LegendWorld.Data.Items;
using DataClient;
using LegendWorld.Data;

namespace WindowsClient.Net
{
    public class NetworkEngine
    {
        internal bool Connected { get { return socketClient.State == State.Connected; } }
        internal uint Ticks { get; set; }
        public ClientWorldState WorldState { get; internal set; }

        private WorldWebDataContext dataContext;
        private SocketClient socketClient;
        private ClientPacketHandler[] clientPacketHandlers;

        public NetworkEngine()
        {
            dataContext = new WorldWebDataContext(string.Format(@"http://{0}:{1}/", LegendClient.Properties.Settings.Default.DataServerAddress, LegendClient.Properties.Settings.Default.DataServerPort));
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
                socketClient.Connect(LegendClient.Properties.Settings.Default.GameServerAddress, LegendClient.Properties.Settings.Default.GameServerPort);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void LoadContent(ClientWorldState world)
        {
            IEnumerable<Item> items = dataContext.GetItems(world.PlayerCharacter.CurrentMapId).Result;
            if (items != null)
            {
                foreach (Item item in items)
                {
                    world.AddItem(item);
                }
            }
            IEnumerable<GroundItem> groundItems = dataContext.GetGroundItems(world.PlayerCharacter.CurrentMapId).Result;
            if (groundItems != null)
            {
                foreach (Item item in items)
                {
                    world.AddItem(item);
                }
            }
        }
        
        public void UnloadContent()
        {
            socketClient.Disconnect();
        }

        public void Update()
        {
            if (!this.Connected)
                return;

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

        internal void UseItem(ConsumableItem consumable)
        {
            UseConsumablePacket useConsumablePacket = new UseConsumablePacket();
            socketClient.Send(useConsumablePacket);
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

        internal void PerformAbility(ClientCharacter playerCharacter, AbilityIdentity abilityId)
        {
            PerformAbilityPacket packet = new PerformAbilityPacket(playerCharacter.Id, abilityId);
            socketClient.Send(packet);
        }

        //internal void PerformHeal(ClientCharacter playerCharacter)
        //{
        //    PerformAbilityPacket packet = new PerformAbilityPacket(playerCharacter.Id, AbilityIdentity.Heal);
        //    socketClient.Send(packet);
        //}
    }
}
