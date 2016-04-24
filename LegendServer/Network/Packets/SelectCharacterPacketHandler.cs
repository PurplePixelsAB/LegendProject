using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data;

namespace UdpServer.Network.Packets
{


    internal class SelectCharacterPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, WorldServer worldState)
        {
            SelectCharacterPacket selectCharacterPacket = (SelectCharacterPacket)packet;
            ServerCharacter selectedChar = new ServerCharacter(); //ToDo: Get from DataContext
            selectedChar.Id = selectCharacterPacket.CharacterId;
            selectedChar.Owner = netState;
            selectedChar.Owner.WorldId = selectedChar.Id;
            selectedChar.Owner.Disconnected += worldState.CharacterDisconnects;
            worldState.AddCharacter(selectedChar);

            netState.WriteConsole("WorldId: {0} registred.", selectedChar.Id);
        }
    }
}
