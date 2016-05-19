using Data.World;
using Network;
using Network.Packets;
using System.Collections.Generic;
using System;
using Data;
using Microsoft.Xna.Framework;
using LegendWorld.Data;
using LegendWorld.Data.Abilities;
using LegendWorld.Data.Items;
using DataClient;
using LegendServer.Network;

namespace UdpServer
{
    internal class ServerWorldState : WorldState
    {
        private List<int>[] maptoCharacterRelations;
        private List<int>[] mapToGroundItems;
        private ServerWorldDataContext dataContext;


        public ServerWorldState(ServerWorldDataContext serverWorldDataContext) : base()
        {
            dataContext = serverWorldDataContext; //new WorldWebDataContext(string.Format(@"http://{0}:{1}/", LegendServer.Properties.Settings.Default.DataServerAddress, LegendServer.Properties.Settings.Default.DataServerPort));
            int expectedMaxPlayers = 1000; //server.Statistics.PlayerPeak;
            int mapZones = 1; //maps.Count;
            characters = new Dictionary<int, Character>(expectedMaxPlayers);
            maptoCharacterRelations = new List<int>[mapZones];
            mapToGroundItems = new List<int>[mapZones];
            for (int i = 0; i < mapZones; i++)
            {
                maptoCharacterRelations[i] = new List<int>(expectedMaxPlayers);
                mapToGroundItems[i] = new List<int>(expectedMaxPlayers);
            }
        }

        internal ServerCharacter LoadCharacter(int characterID)
        {
            CharacterData characterData = dataContext.GetCharacter(characterID);
            ServerCharacter serverCharacter = new ServerCharacter(characterData);
            var bagItem = this.CreateItem(characterData.Inventory);
            //serverCharacter.Inventory
            return serverCharacter;
        }

        internal void LoadMapData(int mapId)
        {
            IEnumerable<ItemData> items = dataContext.GetItems(mapId);
            if (items != null)
            {
                foreach (ItemData itemData in items)
                {
                    IItem item = this.GetItem(itemData.ItemDataID);
                    if (item == null)
                    {
                        item = this.CreateItem(itemData);
                        this.AddItem(item);
                    }
                    item.Data = itemData;
                }
            }
            //IEnumerable<GroundItem> groundItems = dataContext.GetGroundItems(mapId);
            //if (groundItems != null)
            //{
            //    foreach (GroundItem item in groundItems)
            //    {
            //        this.AddGroundItem(item);
            //    }
            //}
        }

        internal PlayerSession GetPlayerSession(int sessionId)
        {
            return dataContext.GetSession(sessionId);
        }

        internal IEnumerable<int> GetMapCharacters(int currentMapId)
        {
            return maptoCharacterRelations[currentMapId];
        }

        public override void AddCharacter(Character character)
        {
            ServerCharacter serverCharacter = (ServerCharacter)character;
            serverCharacter.HealthChanged += ServerCharacter_HealthChanged;
            serverCharacter.AimToChanged += ServerCharacter_AimToChanged;
            serverCharacter.MoveToChanged += ServerCharacter_MoveToChanged;
            serverCharacter.Owner.Disconnected += ServerCharacter_Disconnects;

            base.AddCharacter(character);
            this.AddCharacterToMap(character);
            this.UpdateThisCharacterOfEveryone(serverCharacter);
            this.UpdateEveryoneOfThisCharacter(serverCharacter);
            //this.UpdateCharacterOfEveryGroundItem(serverCharacter);
        }

        private void AddCharacterToMap(Character character)
        {
            var mapCollection = maptoCharacterRelations[character.CurrentMapId];

            if (mapCollection.Count == 0)
                this.LoadMapData(character.CurrentMapId);

            mapCollection.Add(character.Id);
        }

        private void ServerCharacter_MoveToChanged(object sender, EventArgs e)
        {
            ServerCharacter character = (ServerCharacter)sender;
            MoveToPacket packet = new MoveToPacket(character.Id, character.MovingToPosition);
            foreach (ushort mapCharacterId in maptoCharacterRelations[character.CurrentMapId])
            {
                ServerCharacter characterToUpdate = ((ServerCharacter)characters[mapCharacterId]);
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(packet);
            }
        }

        private void ServerCharacter_AimToChanged(object sender, EventArgs e)
        {
            ServerCharacter character = (ServerCharacter)sender;
            AimToPacket packet = new AimToPacket(character.Id, character.AimToPosition);
            foreach (ushort mapCharacterId in maptoCharacterRelations[character.CurrentMapId])
            {
                ServerCharacter characterToUpdate = ((ServerCharacter)characters[mapCharacterId]);
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(packet);
            }
        }

        private void ServerCharacter_HealthChanged(object sender, Character.HealthChangedEventArgs e)
        {
            ServerCharacter character = (ServerCharacter)sender;
            this.UpdateEveryoneOfThisCharacter(character);
        }

        //public override void AddItem(Item item)
        //{
        //    base.AddItem(item);
        //    //ServerCharacter serverCharacter = (ServerCharacter)character;
        //    maptoCharacterRelations[character.CurrentMapId].Add(character.Id);
        //}
        //public void AddGroundItem(Item item, ushort mapId, Point position)
        //{
        //    this.AddItem(item);
        //    GroundItem groundItem = new GroundItem();
        //    groundItem.Position = position;
        //    groundItem.CurrentMapId = mapId;
        //    groundItem.ItemId = (ushort)item.Id;
        //    base.AddGroundItem(groundItem);
        //    //this.UpdateEveryoneOfGroundItem(groundItem);
        //}

        private void ServerCharacter_Disconnects(object sender, EventArgs e)
        {
            NetState dcOwner = (NetState)sender;
            ServerCharacter character = (ServerCharacter)this.GetCharacter(dcOwner.WorldId);
            dataContext.SaveCharacter(character.GetData());

            if (character != null)
                this.RemoveCharacter(character);
            if (dcOwner.Id != -1)
                dataContext.EndSession(dcOwner.Id);
        }

        internal void UpdateThisCharacterOfEveryone(ServerCharacter characterToUpdate)
        {
            NetState clientSendTo = characterToUpdate.Owner;

            foreach (ushort characterId in characters.Keys)
            {
                if (characterToUpdate.Id == characterId)
                    continue;

                ServerCharacter aboutCharacter = ((ServerCharacter)characters[characterId]);
                clientSendTo.Send(new StatsChangedPacket(aboutCharacter.Id, aboutCharacter.Health, aboutCharacter.Energy));
            }
        }
        internal void UpdateEveryoneOfThisCharacter(ServerCharacter aboutCharacter)
        {
            var packet = new StatsChangedPacket(aboutCharacter.Id, aboutCharacter.Health, aboutCharacter.Energy);
            foreach (ushort mapCharacterId in maptoCharacterRelations[aboutCharacter.CurrentMapId])
            {
                ServerCharacter characterToUpdate = ((ServerCharacter)characters[mapCharacterId]);
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(packet);
            }
        }
        //internal void UpdateEveryoneOfGroundItem(GroundItem aboutItem)
        //{
        //    var packet = new UpdateStaticPacket(new List<GroundItem>(new GroundItem[] { aboutItem }));
        //    foreach (ushort mapCharacterId in maptoCharacterRelations[aboutItem.CurrentMapId])
        //    {
        //        ServerCharacter characterToUpdate = ((ServerCharacter)characters[mapCharacterId]);
        //        NetState clientSendTo = characterToUpdate.Owner;

        //        clientSendTo.Send(packet);
        //    }
        //}
        //private void UpdateCharacterOfEveryGroundItem(ServerCharacter characterToUpdate)
        //{
        //    NetState clientSendTo = characterToUpdate.Owner;
        //    var sendList = new List<GroundItem>(this.GroundItems.Count);
        //    foreach (ushort groundId in this.GroundItems)
        //    {
        //        var groundItemToUpdate = this.GetGroundItem(groundId); //((ServerCharacter)characters[characterId]);
        //        sendList.Add(groundItemToUpdate);
        //    }
        //    if (sendList.Count > 0)
        //        clientSendTo.Send(new UpdateStaticPacket(sendList));
        //}

        private void character_InputUpdated(object sender, EventArgs e)
        {
            ServerCharacter mobileUpdated = (ServerCharacter)sender;
            this.UpdateEveryoneOfThisCharacter(mobileUpdated);
        }

        public override void RemoveCharacter(Character character)
        {
            character.HealthChanged -= ServerCharacter_HealthChanged;
            character.AimToChanged -= ServerCharacter_AimToChanged;
            character.MoveToChanged -= ServerCharacter_MoveToChanged;
            base.RemoveCharacter(character);
            maptoCharacterRelations[character.CurrentMapId].Remove(character.Id);
        }

        public void ChangeMap(ServerCharacter character, ushort newMapId)
        {
            maptoCharacterRelations[character.CurrentMapId].Remove(character.Id);
            character.CurrentMapId = newMapId;
            maptoCharacterRelations[character.CurrentMapId].Add(character.Id);
        }

        protected override WorldMap GetCharactersMap(Character character)
        {
            //ToDo Add Map Stuff
            return new WorldMap() { Bounds = new Rectangle(0, 0, short.MaxValue, short.MaxValue) };
        }

        public override bool PerformAbility(CharacterPowerIdentity abilityId, Character character)
        {
            return base.PerformAbility(abilityId, character);
        }

        protected override IItemFactory GetItemFactory(ItemData.ItemIdentity identity)
        {
            return new ServerItemFactory(identity);
        }
        //public void Update()
        //{

        //}

        //internal ICanMove GetMobile(int id)
        //{
        //    if (characters.ContainsKey(id))
        //    {
        //        return characters[id];
        //    }

        //    return null;
        //}
    }
}