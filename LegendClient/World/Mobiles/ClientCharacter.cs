using Data.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Network;

namespace WindowsClient.World.Mobiles
{
    public class ClientCharacter : Character
    {
        //public Point ServerPosition { get; set; }

        public Vector2 DrawPosition { get; set; }
    }
}
