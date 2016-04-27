using Data;
using Data.World;
using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsClient.World.Mobiles;

namespace WindowsClient.World
{
    public class ClientWorldState : WorldState
    {

        //private byte swingDmg = 24;

        public IEnumerable<ushort> Characters { get { return base.characters.Keys; } }
        internal ClientCharacter PlayerCharacter { get; set; }

        protected override WorldMap GetCharactersMap(Character character)
        {
            //ToDo Add Map Stuff
            return new WorldMap() { Bounds = new Rectangle(0, 0, short.MaxValue, short.MaxValue) };
        }

        internal void ClientUpdate(GameTime gameTime)
        {
            Vector2 centerVector2 = new Vector2(960f, 540f);
            float lerpAmount = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / WorldPump.Interval);

            Vector2 realPlayerPosition = centerVector2 - this.PlayerCharacter.Position.ToVector2();
            if (realPlayerPosition != PlayerCharacter.DrawPosition)
            {
                PlayerCharacter.DrawPosition = Vector2.Lerp(PlayerCharacter.DrawPosition, realPlayerPosition, lerpAmount);
            }

            foreach (ushort clientId in Characters)
            {
                if (PlayerCharacter.Id == clientId)
                    continue;

                ClientCharacter client = (ClientCharacter)this.GetCharacter(clientId);

                Vector2 realClientPosition = centerVector2 - (this.PlayerCharacter.Position - client.lastKnownServerPosition).ToVector2(); //new Vector2(this.PlayerCharacter.Position.X - client.Position.X, this.PlayerCharacter.Position.Y - client.Position.Y);
                if (realClientPosition != client.DrawPosition)
                {
                    client.DrawPosition = Vector2.Lerp(client.DrawPosition, realClientPosition, lerpAmount);
                }
            }
        }
    }
}
