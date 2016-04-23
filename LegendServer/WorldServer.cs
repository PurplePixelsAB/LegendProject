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
            this.UpdateThisCharacterToEveryone(serverCharacter);
            this.UpdateEveryoneOfThisCharacter(serverCharacter);
        }

        internal void UpdateEveryoneOfThisCharacter(ServerCharacter character)
        {
            foreach (int characterId in characters.Keys)
            {
                if (character.Id == characterId)
                    continue;

                ServerCharacter charToUpdate = ((ServerCharacter)characters[characterId]);
                NetState clientSendTo = character.Owner;

                clientSendTo.WriteConsole("Sending client updateInfo for {0}.", charToUpdate.Id, character.Id);
                clientSendTo.Send(new UpdateMobilePacket(character));
            }
        }
        internal void UpdateThisCharacterToEveryone(ServerCharacter character)
        {
            var packet = new UpdateMobilePacket(character);
            foreach (int mapCharacterId in maptoCharacterRelations[character.CurrentMapId])
            {
                ServerCharacter charSendTo = ((ServerCharacter)characters[mapCharacterId]);
                NetState clientSendTo = charSendTo.Owner;

                clientSendTo.WriteConsole("Sending client updateInfo for {1}.", charSendTo.Id, character.Id);
                clientSendTo.Send(packet);
            }
        }

        private void character_InputUpdated(object sender, EventArgs e)
        {
            ServerCharacter mobileUpdated = (ServerCharacter)sender;
            this.UpdateThisCharacterToEveryone(mobileUpdated);
        }

        public void RemoveCharacter(Character character)
        {
            if (!characters.ContainsKey(character.Id))
                return;

            characters.Remove(character.Id);
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