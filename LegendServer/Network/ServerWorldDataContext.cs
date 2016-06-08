using DataClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using UdpServer;
using Data;
using LegendWorld.Data;
using Microsoft.Xna.Framework;
using LegendWorld.Data.Items;

namespace LegendServer.Network
{
    internal class ServerWorldDataContext : WorldWebDataContext
    {
        public ServerWorldDataContext(string serverAdress) : base(serverAdress)
        {
        }

        internal void EndSession(int sessionId)
        {
            var result = this.Get<object>(authAddress + "EndSession/" + sessionId); ;
        }

        private void ResetSessions()
        {
            var result = this.Get<object>(authAddress + "ResetSessions/");
        }

        internal bool AuthServer(string dataServerUsername, string dataServerPassword)
        {
            //ToDo: Login to DataServer
            this.ResetSessions();
            return true; 
        }

        internal void SaveCharacterPosition(int id, int currentMapId, Point position)
        {
            this.PostCharacterPosition(id, currentMapId, position);
        }

        private void PostCharacterPosition(int id, int currentMapId, Point position)
        {
            base.Post(characterAddress + "PostPosition/", new CharacterPositionModel() { CharacterId = id, MapId = currentMapId, WorldX = position.X, WorldY = position.Y });
        }

        internal void SaveCharacterStats(int id, int health, int energy)
        {
            this.PostCharacterStats(id, health, energy);
        }

        private void PostCharacterStats(int id, int health, int energy)
        {
            base.Post(characterAddress + "PostStats/", new CharacterStatsModel() { CharacterId = id, Health = health, Energy = energy });
        }

        internal void SaveCharacterItems(int id, ArmorItem armor, WeaponItem leftHand, WeaponItem rightHand)
        {
            int armorId = 0, leftHandId = 0, rightHandId = 0;
            if (armor != null)
                armorId = armor.Id;

            if (leftHand != null)
                leftHandId = leftHand.Id;

            if (rightHand != null)
                rightHandId = rightHand.Id;

            this.PostCharacterItems(id, armorId, leftHandId, rightHandId);
        }

        internal void SaveItemPosition(int id, int? worldMapId, int? worldX, int? worldY, int? containerId)
        {
            if (worldMapId.HasValue && worldX.HasValue && worldY.HasValue)
                this.PostItemPosition(id, worldMapId.Value, worldX.Value, worldY.Value);
            else if (containerId.HasValue)
                this.PostItemPosition(id, containerId.Value);
        }        

        //private const string postItemPositionAdress = itemsAddress + "PostPosition/";

        private void PostItemPosition(int id, int worldMapId, int worldX, int worldY)
        {
            base.Post(itemsAddress + "PostWorldPosition/", new ItemWorldPositionModel() { ItemId = id, WorldMapId = worldMapId, WorldX = worldX, WorldY = worldY });
        }
        //private const string postItemPositionAdress = itemsAddress + "PostPosition/";

        private void PostItemPosition(int id, int containerId)
        {
            base.Post(itemsAddress + "PostContainerPosition/", new ItemContainerPositionModel() { ItemId = id, ContainerId = containerId });
        }

        private const string postCharacterItemsAdress = characterAddress + "PostItems/";
        private void PostCharacterItems(int id, int armorId, int leftHandId, int rightHandId)
        {
            base.Post(postCharacterItemsAdress, new CharacterItemsModel() { CharacterId = id, ArmorId = armorId, LeftHandId = leftHandId, RightHandId = rightHandId });
        }

        internal void SaveItemUse(int id)
        {
            var result = base.Get<object>(itemsAddress + "Use/" + id);
        }

        internal ItemModel CreateNewItem(ItemIdentity itemId, int mapId, Point position, int? subType)
        {
            return base.Post(itemsAddress + "CreateNew/", new ItemModel() { Identity = (int)itemId, WorldMapId = mapId, WorldX = position.X, WorldY = position.Y, SubType = subType });
        }

        internal void SaveCharacterPowerLearned(int id, CharacterPowerIdentity power)
        {
            var result = base.Get<object>(characterAddress + "LearnPower/" + id + "/" + (int)power);
        }

        //internal WorldState LoadWorldState()
        //{
        //    ServerWorldState worldState = 
        //    var mapList = new int[] { 0 };

        //    foreach (int mapID in mapList)
        //    {
        //        var items = this.GetItems(0);
        //        foreach (ItemData item in items)
        //        {
        //            IItem itemToAdd = worldState.CreateItem(item);
        //            worldState.AddItem(itemToAdd);
        //        }
        //    }

        //    return worldState;
        //}

        //internal void SaveCharacter(CharacterModel dataToSave)
        //{
        //    this.Put(characterAddress + dataToSave.Id, dataToSave);
        //}

        //internal ItemModel SaveItem(ItemModel data)
        //{
        //    if (data.Id != 0)
        //        this.Put(itemsAddress + data.Id, data);
        //    else
        //        data = this.Post(itemsAddress, data);

        //    return data;
        //}
    }
}
