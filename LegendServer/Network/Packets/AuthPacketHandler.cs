using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Data;
using LegendWorld.Data;
using System.Net;

namespace UdpServer.Network.Packets
{
    internal class AuthPacketHandler : ServerPacketHandler
    {
        protected override void OnHandle(IPacket packet, NetState netState, ServerWorldState worldState)
        {
            AuthPacket incomingPacket = (AuthPacket)packet;
            PlayerSessionModel session = worldState.GetPlayerSession(incomingPacket.SessionId);
            //worldState.AuthenticateClient(netState, incomingPacket.SessionId, incomingPacket.CharacterId);

            if (session != null)
            {
                IPAddress sessionIPAddress = IPAddress.Parse(session.ClientAddress);

                if (netState.Address == sessionIPAddress || sessionIPAddress.Equals(IPAddress.IPv6Loopback) || sessionIPAddress.Equals(IPAddress.Loopback))
                {
                    netState.Id = session.Id;
                    ServerCharacter selectedChar = worldState.LoadCharacter(session.CharacterID);
                    if (selectedChar != null)
                    {
                        selectedChar.Owner = netState;
                        selectedChar.Owner.WorldId = selectedChar.Id;
                        worldState.AddCharacter(selectedChar);
                        netState.WriteConsole("Session confirmed, character: {0} selected.", selectedChar.Id);
                    }
                    else
                    {
                        netState.WriteConsole("AUTHERROR: Invalid characterID: {0}.", session.CharacterID);
                        netState.Dispose();
                    }
                }
                else
                {
                    netState.WriteConsole("AUTHERROR: Session failed, adress '{0}' did not match session address '{1}'.", netState.Address.ToString(), sessionIPAddress.ToString());
                    netState.Dispose();
                }
            }
            else
            {
                netState.WriteConsole("AUTHERROR: Invalid sessionID '{0}'.", incomingPacket.SessionId);
                netState.Dispose();
            }
        }
    }
}
