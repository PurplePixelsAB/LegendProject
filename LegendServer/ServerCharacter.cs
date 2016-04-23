using System;
using Data.World;
using Microsoft.Xna.Framework;

namespace UdpServer
{
    internal class ServerCharacter : Character
    {
        public NetState Owner { get; set; }
        
    }
}