using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data;
using LegendWorld.Data;

namespace UdpServer.Network.Packets
{
    internal class AuthPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            AuthPacket incomingPacket = (AuthPacket)packet;
            PlayerSession session = worldState.GetPlayerSession(incomingPacket.SessionId);
            //worldState.AuthenticateClient(netState, incomingPacket.SessionId, incomingPacket.CharacterId);

            if (netState.Address.ToString() == session.ClientAddress)
            {
                ServerCharacter selectedChar = new ServerCharacter(); //ToDo: Get from DataContext
                selectedChar.Id = session.CharacterId;
                selectedChar.Owner = netState;
                selectedChar.Owner.Id = session.Id;
                selectedChar.Owner.WorldId = selectedChar.Id;
                worldState.AddCharacter(selectedChar);
                netState.WriteConsole("Session confirmed, character: {0} selected.", selectedChar.Id);
            }
            else
            {
                netState.WriteConsole("Session failed, adress '{0}' did not match session address '{1}'.", netState.Address.ToString(), session.ClientAddress);
                netState.Dispose();
            }
        }
    }
}
