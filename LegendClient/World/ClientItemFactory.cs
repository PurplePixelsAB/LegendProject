using System;
using Data;
using LegendWorld.Data;
using Network;
using LegendClient.World.Items;
using Microsoft.Xna.Framework.Graphics;
using LegendWorld.Data.Items;

namespace WindowsClient.World
{
    internal static class ClientItemFactory
    {
        private static IItemFactory[] factories = new IItemFactory[Enum.GetValues(typeof(ItemIdentity)).Length];
        internal static IItemFactory Get(ItemIdentity identity)
        {
            return factories[(int)identity];
        }
        internal static void Load(ItemIdentity identity, IItemFactory clientFactory)
        {
            factories[(int)identity] = clientFactory;
        }
    }
    internal class ClientItemFactory<TItem> : IItemFactory where TItem : Item, IClientItem, new()
    {
        public Texture2D Texture { get; internal set; }

        public virtual Item CreateNew(ItemModel itemData)
        {
            return this.CreateClientItem(itemData);
        }

        protected virtual TItem CreateClientItem(ItemModel itemData)
        {
            TItem returnItem = new TItem();
            returnItem.LoadData(itemData);
            returnItem.Texture = this.Texture;
            return returnItem;
        }
    }

    internal class ArmorClientItemFactory<TItem> : ClientItemFactory<TItem> where TItem : Item, IArmorClientItem, new()
    {
        public Texture2D HeadTexture { get; internal set; }

        protected override TItem CreateClientItem(ItemModel itemData)
        {
            TItem returnItem = base.CreateClientItem(itemData);
            returnItem.HeadTexture = this.HeadTexture;

            return returnItem;
        }
    }
}