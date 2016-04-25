using Data.World;
using Network;
using Network.Packets;
using System.Collections.Generic;
using System;
using Data;
using Microsoft.Xna.Framework;

namespace UdpServer
{
    internal class WorldServer : WorldState
    {
        private List<int>[] maptoCharacterRelations;

        public WorldServer() : base()
        {
            int expectedMaxPlayers = 1000; //server.Statistics.PlayerPeak;
            int mapZones = 1; //maps.Count;
            characters = new Dictionary<int, Character>(expectedMaxPlayers);
            maptoCharacterRelations = new List<int>[mapZones];
            for (int i = 0; i < mapZones; i++)
            {
                maptoCharacterRelations[i] = new List<int>(expectedMaxPlayers);
            }
        }

        public override void AddCharacter(Character character)
        {
            base.AddCharacter(character);
            ServerCharacter serverCharacter = (ServerCharacter)character;
            maptoCharacterRelations[character.CurrentMapId].Add(character.Id); 
            this.UpdateThisCharacterOfEveryone(serverCharacter);
            this.UpdateEveryoneOfThisCharacter(serverCharacter);
        }

        internal void CharacterDisconnects(object sender, EventArgs e)
        {
            NetState dcOwner = (NetState)sender;
            ServerCharacter character = (ServerCharacter)this.GetCharacter(dcOwner.WorldId);
            this.RemoveCharacter(character);
        }

        internal void UpdateThisCharacterOfEveryone(ServerCharacter characterToUpdate)
        {
            NetState clientSendTo = characterToUpdate.Owner;

            foreach (int characterId in characters.Keys)
            {
                if (characterToUpdate.Id == characterId)
                    continue;

                ServerCharacter aboutCharacter = ((ServerCharacter)characters[characterId]);
                clientSendTo.Send(new UpdateMobilePacket(aboutCharacter));
                clientSendTo.WriteConsole("Sending client{0} info for client{1}.", characterToUpdate.Id, aboutCharacter.Id);
            }
        }
        internal void UpdateEveryoneOfThisCharacter(ServerCharacter aboutCharacter)
        {
            var packet = new UpdateMobilePacket(aboutCharacter);
            foreach (int mapCharacterId in maptoCharacterRelations[aboutCharacter.CurrentMapId])
            {
                ServerCharacter characterToUpdate = ((ServerCharacter)characters[mapCharacterId]);
                NetState clientSendTo = characterToUpdate.Owner;

                clientSendTo.Send(packet);
                clientSendTo.WriteConsole("Sending client{0} info for client{1}.", characterToUpdate.Id, aboutCharacter.Id);
            }
        }

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

        public void ChangeMap(ServerCharacter character, int newMapId)
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