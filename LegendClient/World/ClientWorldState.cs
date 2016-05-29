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
using LegendClient.Screens;
using LegendWorld.Data.Items;

namespace WindowsClient.World
{
    public class ClientWorldState : WorldState
    {
        public ClientWorldState() : base()
        {
            this.MissingCharacters = new List<int>(30);
        }
        //private byte swingDmg = 24;

        //public IEnumerable<ushort> Characters { get { return base.characters.Keys; } }
        internal ClientCharacter PlayerCharacter { get; set; }
        public List<int> MissingCharacters { get; set; }

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

        internal List<IItem> GroundItemsInRange(int id)
        {
            List<IItem> itemsInRange = new List<IItem>(10);
            Character charToRangeCheck = this.GetCharacter(id);
            foreach (int itemId in this.Items)
            {
                IItem clientItem = (IItem)this.GetItem(itemId);
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

        internal ClientCharacter CreateCharacter(CharacterData charData) //, ItemData inventoryData)
        {
            ClientCharacter character = new ClientCharacter(charData.CharacterDataID, charData.WorldLocation);
            character.Stats.Health = charData.Health;
            character.Stats.Energy = charData.Energy;
            foreach (var power in charData.Powers)
                character.Learn(power.Power);

            if (charData.RightHandID.HasValue)
                character.RightHand = (WeaponItem)this.GetItem(charData.RightHandID.Value);
            if (charData.LeftHandID.HasValue)
                character.LeftHand = (WeaponItem)this.GetItem(charData.RightHandID.Value);
            if (charData.ArmorID.HasValue)
                character.Armor = (ArmorItem)this.GetItem(charData.ArmorID.Value);

            BagClientItem inventory = (BagClientItem)this.GetItem(charData.InventoryID);
            character.Inventory = inventory;

            //character.InventoryData = inventory.Data; //inventoryData; //dataContext.GetItem(charData.InventoryID);
            //if (character.InventoryData != null)
            //    character.Inventory = inventory; //(BagClientItem)this.CreateItem(character.InventoryData);

            //world.AddCharacter(character);
            //this.AddItem(character.Inventory);

            return character;
        }
    }

    public class NewCharacterEventArgs : EventArgs
    {
        public Character Character { get; set; }
    }
}
