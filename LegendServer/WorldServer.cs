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

        internal void SaveItemPosition(Item item)
        {
            dataContext.SaveItemPosition(item.Id, item.WorldMapId, item.WorldX, item.WorldY, item.ContainerId);
        }

        //internal void SaveItem(IItem itemToUse)
        //{
        //    dataContext.SaveItemCount(itemToUse.Data);
        //}

        internal ServerCharacter LoadCharacter(int characterID)
        {
            CharacterModel characterData = dataContext.GetCharacter(characterID);
            if (characterData != null)
            {
                ServerCharacter serverCharacter = new ServerCharacter(characterData);
                ItemModel inventory = dataContext.GetItem(characterData.InventoryId);
                if (inventory != null)
                {
                    var inventoryBagItem = (ContainerItem)this.CreateItem(inventory);
                    this.AddItem(inventoryBagItem);
                    serverCharacter.Inventory = inventoryBagItem; //characterData.Inventory;
                }
                else
                    return null;

                if (characterData.RightHandId.HasValue)
                {
                    ItemModel rightHand = dataContext.GetItem(characterData.RightHandId.Value);
                    if (rightHand != null)
                    {
                        var item = (WeaponItem)this.CreateItem(rightHand);
                        this.AddItem(item);
                        if (serverCharacter.IsItemInInventory(item))
                            serverCharacter.RightHand = item;
                        else
                            characterData.RightHandId = null;
                    }
                    else
                    {
                        characterData.RightHandId = null;
                    }
                          
                }
                if (characterData.LeftHandId.HasValue)
                {
                    ItemModel leftHand = dataContext.GetItem(characterData.LeftHandId.Value);
                    if (leftHand != null)
                    {
                        var item = (WeaponItem)this.CreateItem(leftHand);
                        this.AddItem(item);
                        if (serverCharacter.IsItemInInventory(item))
                            serverCharacter.LeftHand = item;
                        else
                            characterData.LeftHandId = null;
                    }
                    else
                    {
                        characterData.LeftHandId = null;
                    }
                }
                if (characterData.ArmorId.HasValue)
                {
                    ItemModel armor = dataContext.GetItem(characterData.ArmorId.Value);
                    if (armor != null)
                    {
                        var item = (ArmorItem)this.CreateItem(armor);
                        this.AddItem(item);
                        if (serverCharacter.IsItemInInventory(item))
                            serverCharacter.Armor = item;
                        else
                            characterData.ArmorId = null;
                    }
                    else
                    {
                        characterData.ArmorId = null;
                    }
                }

                return serverCharacter;
            }
            else
                return null;
        }

        internal void SaveCharacterItems(ServerCharacter serverCharacter)
        {
            dataContext.SaveCharacterItems(serverCharacter.Id, serverCharacter.Armor, serverCharacter.LeftHand, serverCharacter.RightHand);
        }

        internal void SaveItemUse(Item itemToUse)
        {
            dataContext.SaveItemUse(itemToUse.Id);
        }

        //internal void SaveCharacter(ServerCharacter serverCharacter)
        //{
        //    dataContext.SaveCharacter(serverCharacter.GetData());
        //}

        internal void LoadMapData(int mapId)
        {
            IEnumerable<ItemModel> items = dataContext.GetItems(mapId);
            if (items != null)
            {
                foreach (ItemModel itemData in items)
                {
                    Item item = this.GetItem(itemData.Id);
                    if (item == null)
                    {
                        item = this.CreateItem(itemData);
                        this.AddItem(item);
                    }
                    item.LoadData(itemData);
                    //item.Data = itemData;
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

        internal PlayerSessionModel GetPlayerSession(int sessionId)
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
            serverCharacter.Stats.OnStatChangedRegister(StatIdentifier.Health, ServerCharacter_HealthChanged);
            //serverCharacter.HealthChanged += ServerCharacter_HealthChanged;
            serverCharacter.AimToChanged += ServerCharacter_AimToChanged;
            serverCharacter.MoveToChanged += ServerCharacter_MoveToChanged;
            serverCharacter.PowerLearned += ServerCharacter_PowerLearned;
            serverCharacter.Owner.Disconnected += ServerCharacter_Disconnects;

            base.AddCharacter(character);
            this.AddCharacterToMap(character);
            this.SendInitialMapStatValues(serverCharacter);
            this.SendInitialMapPositions(serverCharacter);
            this.SendStatChangeToMapCharacters(serverCharacter);
        }

        private void ServerCharacter_PowerLearned(Character character, PowerLearnedEventArgs e)
        {
            dataContext.SaveCharacterPowerLearned(character.Id, e.Power);
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


        private void ServerCharacter_HealthChanged(Character character, StatChangedEventArgs e) //private void ServerCharacter_HealthChanged(object sender, Character.HealthChangedEventArgs e)
        {
            if (e.Value != e.PreviousValue) //Why does not this get called?
            {
                ServerCharacter servCharacter = (ServerCharacter)character;
                this.SendStatChangeToMapCharacters(servCharacter);
                if (e.Value <= 0 && e.PreviousValue > 0)
                {
                    CorpseItem corpse = (CorpseItem)this.CreateNewItem(ItemIdentity.Corpse, character.CurrentMapId, servCharacter.Position, character.Id);
                    //corpse.CharacterID = character.Id;
                    //corpse.MoveTo(character.CurrentMapId, servCharacter.Position);
                    //CorpseItem corpse = new CorpseItem();
                    //corpse.Data = new ItemModel();
                    //corpse.Data.Identity = ItemIdentity.Corpse;
                    //corpse.Data = dataContext.SaveItem(corpse.Data);

                    if (corpse.Id != 0)
                    {
                        this.AddItem(corpse);
                        foreach (var item in servCharacter.Inventory.Items)
                        {
                            servCharacter.MoveItem(item, corpse);
                        }

                        foreach (int charID in this.maptoCharacterRelations[character.CurrentMapId])
                        {
                            ServerCharacter informChar = (ServerCharacter)this.GetCharacter(charID);
                            informChar.Owner.Send(new NewItemPacket(corpse.Id));
                            foreach (var item in corpse.Items)
                                informChar.Owner.Send(new MoveItemPacket(servCharacter.Inventory.Id, corpse.Id));
                        }

                        //servCharacter.MoveItem(servCharacter.Inventory, corpse); //                         item.Data.MoveTo(corpse.Data);
                        //dataContext.SaveItem(servCharacter.Inventory.Data);
                    }

                    //this.SendStatChangeToMapCharacters(servCharacter);
                }
                //else if (e.Value > 0 && e.PreviousValue <= 0)
                //{                    
                //    BagItem newInventory = new BagItem();
                //    newInventory.Data = new ItemData();
                //    newInventory.Data.Identity = ItemIdentity.Bag;
                //    newInventory.Data = dataContext.SaveItem(newInventory.Data);

                //    if (newInventory.Data.ItemDataID != 0)
                //    {
                //        this.AddItem(newInventory);
                //        servCharacter.Inventory = newInventory;
                //        servCharacter.LatestData.InventoryID = newInventory.Data.ItemDataID;
                //        dataContext.SaveCharacter(servCharacter.GetData());
                //    }
                //}
            }
        }

        private Item CreateNewItem(ItemIdentity itemId, int mapId, Point position, int? subType)
        {
            ItemModel newItem = dataContext.CreateNewItem(itemId, mapId, position, subType);
            return this.CreateItem(newItem);
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
            dataContext.SaveCharacterPosition(character.Id, character.CurrentMapId, character.Position);
            dataContext.SaveCharacterStats(character.Id, character.Stats.Health, character.Stats.Energy);
            dataContext.SaveCharacterItems(character.Id, character.Armor, character.LeftHand, character.RightHand);
            //dataContext.SaveCharacter(character.GetData());

            if (character != null)
                this.RemoveCharacter(character);
            if (dcOwner.Id != -1)
                dataContext.EndSession(dcOwner.Id);
        }

        internal void SendInitialMapStatValues(ServerCharacter toCharacter)
        {
            NetState clientSendTo = toCharacter.Owner;

            foreach (ushort characterId in characters.Keys)
            {
                if (toCharacter.Id == characterId)
                    continue;

                ServerCharacter aboutCharacter = ((ServerCharacter)characters[characterId]);
                clientSendTo.Send(new StatsChangedPacket(aboutCharacter.Id, (byte)aboutCharacter.Stats.Health, (byte)aboutCharacter.Stats.Energy));
            }
        }
        internal void SendInitialMapPositions(ServerCharacter toCharacter)
        {
            NetState clientSendTo = toCharacter.Owner;

            foreach (ushort characterId in characters.Keys)
            {
                if (toCharacter.Id == characterId)
                    continue;

                ServerCharacter aboutCharacter = ((ServerCharacter)characters[characterId]);
                clientSendTo.Send(new MoveToPacket(aboutCharacter.Id, aboutCharacter.MovingToPosition)); // (byte)aboutCharacter.Stats.Health, (byte)aboutCharacter.Stats.Energy));
                clientSendTo.Send(new AimToPacket(aboutCharacter.Id, aboutCharacter.AimToPosition));
            }
        }

        internal void SendStatChangeToMapCharacters(ServerCharacter aboutCharacter)
        {
            var packet = new StatsChangedPacket(aboutCharacter.Id, (byte)aboutCharacter.Stats.Health, (byte)aboutCharacter.Stats.Energy);
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
            this.SendStatChangeToMapCharacters(mobileUpdated);
        }

        public override void RemoveCharacter(Character character)
        {
            character.Stats.OnStatChangedUnRegister(StatIdentifier.Health, this.ServerCharacter_HealthChanged);
            //character.HealthChanged -= ServerCharacter_HealthChanged;
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

        protected override IItemFactory GetItemFactory(ItemIdentity identity)
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