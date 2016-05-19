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
using Data;
using LegendClient.Screens;

namespace WindowsClient.Net
{
    public class NetworkEngine
    {
        internal bool ConnectedToWorld { get { return worldServerClient.State == State.Connected; } }
        internal bool CharacterSelected { get { return playerCharacterId != -1; } }
        internal uint Ticks { get; set; }
        public ClientWorldState WorldState { get; internal set; }
        public int SessionId { get; private set; }

        private int playerCharacterId = -1;
        private WorldWebDataContext dataContext;
        private SocketClient worldServerClient;
        private ClientPacketHandler[] clientPacketHandlers;

        public NetworkEngine()
        {
            dataContext = new WorldWebDataContext(string.Format(@"http://{0}:{1}/", LegendClient.Properties.Settings.Default.DataServerAddress, LegendClient.Properties.Settings.Default.DataServerPort));
            clientPacketHandlers = new ClientPacketHandler[byte.MaxValue];
            clientPacketHandlers[(byte)PacketIdentity.StatsChanged] = new StatsChangedPacketHandler();
            PacketFactory.Register(PacketIdentity.StatsChanged, () => new StatsChangedPacket());
            clientPacketHandlers[(byte)PacketIdentity.MoveTo] = new MoveToPacketHandler();
            PacketFactory.Register(PacketIdentity.MoveTo, () => new MoveToPacket());
            clientPacketHandlers[(byte)PacketIdentity.AimTo] = new AimToPacketHandler();
            PacketFactory.Register(PacketIdentity.AimTo, () => new AimToPacket());
            clientPacketHandlers[(byte)PacketIdentity.PerformAbility] = new PerformAbilityPacketHandler();
            PacketFactory.Register(PacketIdentity.PerformAbility, () => new PerformAbilityPacket());
            worldServerClient = new SocketClient();
            worldServerClient.ProcessPacket += SocketClient_ProcessPacket;
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
                worldServerClient.Connect(LegendClient.Properties.Settings.Default.GameServerAddress, LegendClient.Properties.Settings.Default.GameServerPort);
                AuthPacket packet = new AuthPacket(this.SessionId);
                worldServerClient.Send(packet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool LoadContent(ClientWorldState world)
        {
            CharacterData playerCharData = dataContext.GetCharacter(playerCharacterId);
            if (playerCharData == null)
            {
                worldServerClient.Disconnect();
                return false;
            }
            world.PlayerCharacter = new ClientCharacter(playerCharData.WorldLocation); //Todo: Get from DataServer
            world.PlayerCharacter.Id = playerCharacterId;
            world.PlayerCharacter.Health = playerCharData.Health;
            world.PlayerCharacter.Energy = playerCharData.Energy;

            ItemData inventoryItemData = dataContext.GetItem(playerCharData.InventoryID);
            world.PlayerCharacter.Inventory = (BagClientItem)world.CreateItem(inventoryItemData);

            world.AddCharacter(world.PlayerCharacter);
            world.AddItem(world.PlayerCharacter.Inventory);

            IEnumerable<ItemData> items = dataContext.GetItems(world.PlayerCharacter.CurrentMapId);
            if (items != null)
            {
                foreach (ItemData itemData in items)
                {
                    IItem item = world.CreateItem(itemData);
                    world.AddItem(item);
                }
            }

            return true;
        }

        public void UnloadContent()
        {
            worldServerClient.Disconnect();
        }

        public void Update()
        {
            if (!this.ConnectedToWorld)
                return;

            worldServerClient.Process();
            if (moveToPacket != null)
            {
                worldServerClient.Send(moveToPacket);
                moveToPacket = null;
            }
            if (aimToPacket != null)
            {
                var toSendPacket = aimToPacket;
                aimToPacket = null;
                worldServerClient.Send(toSendPacket);
            }
            if (performAbilityPacket != null)
            {
                var toSendPacket = performAbilityPacket;
                performAbilityPacket = null;
                worldServerClient.Send(toSendPacket);
            }
        }

        private MoveToPacket moveToPacket;
        internal void MoveTo(int characterId, Point moveToPoint)
        {
            moveToPacket = new MoveToPacket(characterId, moveToPoint); //Packets are sent on Update to reduce Spam
        }

        private AimToPacket aimToPacket;
        internal void AimTo(int characterId, Point aimToPoint)
        {
            aimToPacket = new AimToPacket(characterId, aimToPoint); //Packets are sent on Update to reduce Spam
        }

        PerformAbilityPacket performAbilityPacket;
        internal void PerformAbility(ClientCharacter playerCharacter, CharacterPowerIdentity abilityId)
        {
            performAbilityPacket = new PerformAbilityPacket(playerCharacter.Id, abilityId); //Packets are sent on Update to reduce Spam
        }

        internal void UseItem(ConsumableItem consumable)
        {
            UseItemPacket useConsumablePacket = new UseItemPacket();
            worldServerClient.Send(useConsumablePacket);
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

        internal void SelectCharacter(int selectedCharacterId)
        {
            this.playerCharacterId = selectedCharacterId;
            this.SessionId = dataContext.CreateSession(selectedCharacterId);
        }
        internal List<SelectableCharacter> GetCharacters()
        {
            List<SelectableCharacter> returnList = new List<SelectableCharacter>(3);
            var charList = dataContext.GetCharacters();
            if (charList != null)
            {
                returnList.AddRange(charList);
            }

            return returnList;
        }

        //internal void PerformHeal(ClientCharacter playerCharacter)
        //{
        //    PerformAbilityPacket packet = new PerformAbilityPacket(playerCharacter.Id, AbilityIdentity.Heal);
        //    socketClient.Send(packet);
        //}
    }
}
