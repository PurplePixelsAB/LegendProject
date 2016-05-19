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

        internal bool AuthServer(string dataServerUsername, string dataServerPassword)
        {
            return true; //ToDo: Login to DataServer.
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
    }
}
