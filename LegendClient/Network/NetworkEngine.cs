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
using LegendWorld.Network.Packets;

namespace WindowsClient.Net
{
    public sealed class NetworkEngine
    {
        public static NetworkEngine Instance { get; private set; }
        public static void CreateInstance()
        {
            if (NetworkEngine.Instance == null)
                NetworkEngine.Instance = new NetworkEngine();
        }

        Queue<ServerMessage> serverMessages = new Queue<ServerMessage>(10);
        internal bool ConnectedToWorld { get { return worldServerClient.State == State.Connected; } }
        internal bool CharacterSelected { get { return playerSelectableCharacter != null; } }
        internal uint Ticks { get; set; }
        public ClientWorldState WorldState { get; internal set; }
        public int SessionId { get; private set; }

        private SelectableCharacter playerSelectableCharacter = null;
        private WorldWebDataContext dataContext;
        private SocketClient worldServerClient;
        private ClientPacketHandler[] clientPacketHandlers;

        private NetworkEngine()
        {
            
            dataContext = new WorldWebDataContext(string.Format(@"http://{0}:{1}/", LegendClient.Properties.Settings.Default.DataServerAddress, LegendClient.Properties.Settings.Default.DataServerPort));
            clientPacketHandlers = new ClientPacketHandler[byte.MaxValue + 1];
            clientPacketHandlers[(byte)PacketIdentity.StatsChanged] = new StatsChangedPacketHandler();
            PacketFactory.Register(PacketIdentity.StatsChanged, () => new StatsChangedPacket());
            clientPacketHandlers[(byte)PacketIdentity.MoveTo] = new MoveToPacketHandler();
            PacketFactory.Register(PacketIdentity.MoveTo, () => new MoveToPacket());
            clientPacketHandlers[(byte)PacketIdentity.AimTo] = new AimToPacketHandler();
            PacketFactory.Register(PacketIdentity.AimTo, () => new AimToPacket());
            clientPacketHandlers[(byte)PacketIdentity.PerformAbility] = new PerformAbilityPacketHandler();
            PacketFactory.Register(PacketIdentity.PerformAbility, () => new PerformAbilityPacket());
            clientPacketHandlers[(byte)PacketIdentity.UseItem] = new UseItemPacketHandler();
            PacketFactory.Register(PacketIdentity.UseItem, () => new UseItemPacket());
            clientPacketHandlers[(byte)PacketIdentity.PickUpItem] = new PickUpItemPacketHandler();
            PacketFactory.Register(PacketIdentity.PickUpItem, () => new PickUpItemPacket());
            clientPacketHandlers[(byte)PacketIdentity.Error] = new ErrorPacketHandler();
            PacketFactory.Register(PacketIdentity.Error, () => new ErrorPacket());
            worldServerClient = new SocketClient();
            worldServerClient.ProcessPacket += SocketClient_ProcessPacket;
        }

        //public event EventHandler HandlePacket;
        private void SocketClient_ProcessPacket(object sender, EventArgs e)
        {
            Packet packet = (Packet)sender;
            ClientPacketHandler handler = this.GetHandler(packet.PacketId);
            handler.Network = this;
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
            if (!this.CharacterSelected)
            {
                worldServerClient.Disconnect();
                return false;
            }
            //CharacterData playerCharData = dataContext.GetCharacter(playerSelectableCharacter);
            IEnumerable<ItemData> items = dataContext.GetItems(playerSelectableCharacter.MapId);
            if (items != null)
            {
                List<IItem> hasContainerItem = new List<IItem>();
                foreach (ItemData itemData in items)
                {
                    IItem item = world.CreateItem(itemData);

                    if (item.Data.ContainerID.HasValue)
                    {
                        hasContainerItem.Add(item);
                    }

                    world.AddItem(item);
                }

                foreach (IItem item in hasContainerItem)
                {
                    ContainerItem containerItem = (ContainerItem)world.GetItem(item.Data.ContainerID.Value);
                    if (containerItem != null)
                        containerItem.Items.Add(item);
                }
            }


            //world.PlayerCharacter = new ClientCharacter(playerCharacterId, playerCharData.WorldLocation);
            ////world.PlayerCharacter.Id = playerCharacterId;
            //world.PlayerCharacter.Health = playerCharData.Health;
            //world.PlayerCharacter.Energy = playerCharData.Energy;

            //world.PlayerCharacter.InventoryData = dataContext.GetItem(playerCharData.InventoryID);
            //world.PlayerCharacter.Inventory = (BagClientItem)world.CreateItem(world.PlayerCharacter.InventoryData);

            //world.AddCharacter(world.PlayerCharacter);
            //world.AddItem(world.PlayerCharacter.Inventory);

            IEnumerable<CharacterData> onlineCharacters = dataContext.GetCharacters(playerSelectableCharacter.MapId);
            if (onlineCharacters != null)
            {
                foreach (CharacterData charData in onlineCharacters)
                {
                    ClientCharacter character = world.CreateCharacter(charData); //, dataContext.GetItem(charData.InventoryID)); //new ClientCharacter(charData.CharacterDataID, charData.WorldLocation);
                    ////world.PlayerCharacter.Id = playerCharacterId;
                    //character.Health = playerCharData.Health;
                    //character.Energy = playerCharData.Energy;

                    //character.InventoryData = dataContext.GetItem(charData.InventoryID);
                    //if (character.InventoryData != null)
                    //    character.Inventory = (BagClientItem)world.CreateItem(character.InventoryData);
                    if (character.Id == playerSelectableCharacter.CharacterId)
                        world.PlayerCharacter = character;

                    if (character != null)
                        world.AddCharacter(character);
                    //world.AddItem(character.Inventory);
                }
            }

            if (world.PlayerCharacter == null)
            {
                worldServerClient.Disconnect();
                return false;
            }

            return true;
        }

        public void UnloadContent()
        {
            worldServerClient.Disconnect();
        }

        public void Update(GameTime gameTime)
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

            lock (serverMessages)
            {
                if (serverMessages.Count > 0)
                {
                    var queue = serverMessages;
                    serverMessages = new Queue<ServerMessage>(queue.Count);
                    while (queue.Count > 0)
                    {
                        ServerMessage message = queue.Dequeue();
                        message.Duration -= gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (message.Duration > 0D)
                            serverMessages.Enqueue(message);
                    }
                }
            }

            if (WorldState.MissingCharacters.Count > 0)
            {
                List<int> charIdToCheck = WorldState.MissingCharacters;
                WorldState.MissingCharacters = new List<int>(10);

                foreach (int charID in charIdToCheck)
                {
                    CharacterData charData = dataContext.GetCharacter(charID);
                    ClientCharacter character = WorldState.CreateCharacter(charData); //, dataContext.GetItem(charData.InventoryID));
                    WorldState.AddCharacter(character);
                }
            }
        }

        internal void PickUpItem(int id, IItem item)
        {
            PickUpItemPacket useItemPacket = new PickUpItemPacket(item.Data.ItemDataID, id);
            worldServerClient.Send(useItemPacket);
        }

        internal List<ServerMessage> GetServerMessages()
        {
            return serverMessages.ToList();
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

        internal void UseItem(int characterId, IItem itemToUse)
        {
            UseItemPacket useItemPacket = new UseItemPacket(itemToUse.Data.ItemDataID, characterId);
            worldServerClient.Send(useItemPacket);
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

        internal void AddServerError(int code, string message)
        {
            serverMessages.Enqueue(new ServerMessage(code, message));
        }
        internal void SelectCharacter(SelectableCharacter selectedCharacter)
        {
            this.playerSelectableCharacter = selectedCharacter;
            this.SessionId = dataContext.CreateSession(selectedCharacter.CharacterId);
        }
        internal List<SelectableCharacter> GetSelectableCharacter()
        {
            List<SelectableCharacter> returnList = new List<SelectableCharacter>(3);
            var charList = dataContext.GetSelectableCharacter();
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
