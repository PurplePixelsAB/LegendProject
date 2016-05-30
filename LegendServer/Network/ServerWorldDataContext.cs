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

        internal void SaveCharacter(CharacterData dataToSave)
        {
            this.Put(characterAddress + dataToSave.CharacterDataID, dataToSave);
        }

        internal ItemData SaveItem(ItemData data)
        {
            if (data.ItemDataID != 0)
                this.Put(itemsAddress + data.ItemDataID, data);
            else
                this.Post(itemsAddress, data);

            return data;
        }
    }
}
