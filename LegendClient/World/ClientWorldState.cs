﻿using Data;
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
        //private byte swingDmg = 24;

        //public IEnumerable<ushort> Characters { get { return base.characters.Keys; } }
        internal ClientCharacter PlayerCharacter { get; set; }
        public List<int> MissingCharacters { get; set; }
        internal List<ChatMessage> ChatMessages { get; private set; }

        public ClientWorldState() : base()
        {
            this.MissingCharacters = new List<int>(30);
            this.ChatMessages = new List<ChatMessage>(100);
        }

        internal void AddChatMessage(int mobileId, string message)
        {
            ClientCharacter clientCharacter = (ClientCharacter)this.GetCharacter(mobileId);
            clientCharacter.SentMessage();
            clientCharacter.IsWritingMessage = false;
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.Text = message;
            chatMessage.Duration = 2000 + message.Length * 50;
            chatMessage.Owner = clientCharacter;

            this.ChatMessages.Add(chatMessage);
        }

        internal void SetChatState(int mobileId, bool state)
        {
            ClientCharacter clientCharacter = (ClientCharacter)this.GetCharacter(mobileId);
            clientCharacter.IsWritingMessage = state;
        }

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

            if (this.ChatMessages.Count > 0)
            {
                Queue<ChatMessage> expiredMessage = new Queue<ChatMessage>(this.ChatMessages.Count);
                foreach (ChatMessage message in this.ChatMessages)
                {
                    message.Duration -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (message.Duration <= 0D)
                    {
                        expiredMessage.Enqueue(message);
                    }
                }
                while (expiredMessage.Count > 0)
                {
                    this.ChatMessages.Remove(expiredMessage.Dequeue());
                }
            }
        }

        internal List<Item> WorldItemsInRange(int id)
        {
            List<Item> itemsInRange = new List<Item>(10);
            Character charToRangeCheck = this.GetCharacter(id);
            foreach (int itemId in this.Items)
            {
                Item clientItem = (Item)this.GetItem(itemId);
                if (!clientItem.IsWorldItem)
                {
                    continue;
                }

                //float distance = Vector2.Distance(positionToCheck, clientItem.Data.WorldLocation.ToVector2());
                if (charToRangeCheck.IsPositionInRange(clientItem.WorldLocation))
                {
                    itemsInRange.Add(clientItem);
                }
            }

            return itemsInRange;
        }

        protected override IItemFactory GetItemFactory(ItemIdentity identity)
        {
            return ClientItemFactory.Get(identity);
        }

        internal List<IClientItem> GetItemsOnGround(int currentMapId)
        {
            List<IClientItem> itemsonGround = new List<IClientItem>(10);
            foreach (int itemId in this.Items)
            {
                IClientItem clientItem = (IClientItem)this.GetItem(itemId);
                if (!clientItem.IsWorldItem)
                {
                    continue;
                }

                if (clientItem.WorldMapId.Value == currentMapId)
                {
                    itemsonGround.Add(clientItem);
                }
            }

            return itemsonGround;
        }

        internal ClientCharacter CreateCharacter(CharacterModel charData) //, ItemData inventoryData)
        {
            ClientCharacter character = new ClientCharacter(charData.Id, charData.WorldLocation);
            character.Stats.Health = charData.Health;
            character.Stats.Energy = charData.Energy;
            foreach (var power in charData.Powers)
                character.Learn((CharacterPowerIdentity)power);

            if (charData.RightHandId.HasValue)
                character.RightHand = (WeaponItem)this.GetItem(charData.RightHandId.Value);
            if (charData.LeftHandId.HasValue)
                character.LeftHand = (WeaponItem)this.GetItem(charData.RightHandId.Value);
            if (charData.ArmorId.HasValue)
                character.Armor = (ArmorItem)this.GetItem(charData.ArmorId.Value);

            BagClientItem inventory = (BagClientItem)this.GetItem(charData.InventoryId);
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
