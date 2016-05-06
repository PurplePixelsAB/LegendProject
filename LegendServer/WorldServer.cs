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

namespace UdpServer
{
    internal class WorldServer : WorldState
    {
        private List<ushort>[] maptoCharacterRelations;
        private List<ushort>[] mapToGroundItems;

        public WorldServer() : base()
        {
            int expectedMaxPlayers = 1000; //server.Statistics.PlayerPeak;
            int mapZones = 1; //maps.Count;
            characters = new Dictionary<ushort, Character>(expectedMaxPlayers);
            maptoCharacterRelations = new List<ushort>[mapZones];
            mapToGroundItems = new List<ushort>[mapZones];
            for (int i = 0; i < mapZones; i++)
            {
                maptoCharacterRelations[i] = new List<ushort>(expectedMaxPlayers);
                mapToGroundItems[i] = new List<ushort>(expectedMaxPlayers);
            }
        }

        public override void AddCharacter(Character character)
        {
            base.AddCharacter(character);
            ServerCharacter serverCharacter = (ServerCharacter)character;
            maptoCharacterRelations[character.CurrentMapId].Add(character.Id); 
            this.UpdateThisCharacterOfEveryone(serverCharacter);
            this.UpdateEveryoneOfThisCharacter(serverCharacter);
            //this.UpdateCharacterOfEveryGroundItem(serverCharacter);
        }

        //public override void AddItem(Item item)
        //{
        //    base.AddItem(item);
        //    //ServerCharacter serverCharacter = (ServerCharacter)character;
        //    maptoCharacterRelations[character.CurrentMapId].Add(character.Id);
        //}
        public void AddGroundItem(Item item, ushort mapId, Point position)
        {
            this.AddItem(item);
            GroundItem groundItem = new GroundItem();
            groundItem.Position = position;
            groundItem.CurrentMapId = mapId;
            groundItem.ItemId = (ushort)item.Id;
            base.AddGroundItem(groundItem);
            //this.UpdateEveryoneOfGroundItem(groundItem);
        }

        internal void CharacterDisconnects(object sender, EventArgs e)
        {
            NetState dcOwner = (NetState)sender;
            ServerCharacter character = (ServerCharacter)this.GetCharacter(dcOwner.WorldId);
            if (character != null)
                this.RemoveCharacter(character);
        }

        internal void UpdateThisCharacterOfEveryone(ServerCharacter characterToUpdate)
        {
            NetState clientSendTo = characterToUpdate.Owner;

            foreach (ushort characterId in characters.Keys)
            {
                if (characterToUpdate.Id == characterId)
                    continue;

                ServerCharacter aboutCharacter = ((ServerCharacter)characters[characterId]);
                clientSendTo.Send(new UpdateMobilePacket(aboutCharacter));
            }
        }
        internal void UpdateEveryoneOfThisCharacter(ServerCharacter aboutCharacter)
        {
            var packet = new UpdateMobilePacket(aboutCharacter);
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