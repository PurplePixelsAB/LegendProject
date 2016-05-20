using Data;
using Data.World;
using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsClient.World.Mobiles;
using LegendWorld.Data;
using LegendClient.World.Items;

namespace WindowsClient.World
{
    public class ClientWorldState : WorldState
    {

        //private byte swingDmg = 24;

        //public IEnumerable<ushort> Characters { get { return base.characters.Keys; } }
        internal ClientCharacter PlayerCharacter { get; set; }

        protected override WorldMap GetCharactersMap(Character character)
        {
            //ToDo Add Map Stuff
            return new WorldMap() { Bounds = new Rectangle(0, 0, short.MaxValue, short.MaxValue) };
        }

        public override void AddCharacter(Character newCharacter)
        {
            base.AddCharacter(newCharacter);
            this.OnCharacterAdded(newCharacter);
        }

        public event EventHandler<NewCharacterEventArgs> CharacterAdded;
        private void OnCharacterAdded(Character newCharacter)
        {
            if (this.CharacterAdded != null)
            {
                this.CharacterAdded(this, new NewCharacterEventArgs() { Character = newCharacter });
            }
        }

        internal void ClientUpdate(GameTime gameTime)
        {
            Vector2 centerVector2 = new Vector2(960f, 540f);
            float lerpAmount = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / WorldPump.Interval);
            
            if (this.PlayerCharacter.Position != PlayerCharacter.DrawPosition)
            {
                Vector2 drawPosition = Vector2.Lerp(PlayerCharacter.DrawPosition.ToVector2(), this.PlayerCharacter.Position.ToVector2(), lerpAmount);
                PlayerCharacter.DrawPosition = drawPosition.ToPoint();
                PlayerCharacter.OldDrawPosition = PlayerCharacter.DrawPosition.ToVector2();
            }

            foreach (ushort clientId in Characters)
            {
                if (PlayerCharacter.Id == clientId)
                    continue;

                ClientCharacter client = (ClientCharacter)this.GetCharacter(clientId);

                //Vector2 realClientPosition = centerVector2 - (this.PlayerCharacter.Position - client.Position).ToVector2(); //new Vector2(this.PlayerCharacter.Position.X - client.Position.X, this.PlayerCharacter.Position.Y - client.Position.Y);
                if (client.Position != client.DrawPosition)
                {
                    Vector2 drawPosition = Vector2.Lerp(client.DrawPosition.ToVector2(), client.Position.ToVector2(), lerpAmount);
                    client.DrawPosition = drawPosition.ToPoint();
                    client.OldDrawPosition = PlayerCharacter.DrawPosition.ToVector2();

                    //client.OldDrawPosition = Vector2.Lerp(client.OldDrawPosition, realClientPosition, lerpAmount);
                }
            }
        }

        internal List<IClientItem> GroundItemsInRange(int id)
        {
            List<IClientItem> itemsInRange = new List<IClientItem>(10);
            Character charToRangeCheck = this.GetCharacter(id);
            foreach (int itemId in this.Items)
            {
                IClientItem clientItem = (IClientItem)this.GetItem(itemId);
                if (!clientItem.Data.IsWorldItem)
                {
                    continue;
                }

                //float distance = Vector2.Distance(positionToCheck, clientItem.Data.WorldLocation.ToVector2());
                if (charToRangeCheck.IsPositionInRange(clientItem.Data.WorldLocation))
                {
                    itemsInRange.Add(clientItem);
                }
            }

            return itemsInRange;
        }

        protected override IItemFactory GetItemFactory(ItemData.ItemIdentity identity)
        {
            return ClientItemFactory.Get(identity);
        }

        internal List<IClientItem> GetItemsOnGround(int currentMapId)
        {
            List<IClientItem> itemsonGround = new List<IClientItem>(10);
            foreach (int itemId in this.Items)
            {
                IClientItem clientItem = (IClientItem)this.GetItem(itemId);
                if (!clientItem.Data.IsWorldItem)
                {
                    continue;
                }
                
                if (clientItem.Data.WorldMapID.Value == currentMapId)
                {
                    itemsonGround.Add(clientItem);
                }
            }

            return itemsonGround;
        }
    }

    public class NewCharacterEventArgs : EventArgs
    {
        public Character Character { get; set; }
    }
}
