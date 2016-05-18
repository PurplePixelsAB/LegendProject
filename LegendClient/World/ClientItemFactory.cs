using System;
using Data;
using LegendWorld.Data;
using Network;
using LegendClient.World.Items;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsClient.World
{
    internal static class ClientItemFactory
    {
        private static IItemFactory[] factories = new IItemFactory[Enum.GetValues(typeof(ItemData.ItemIdentity)).Length];
        internal static IItemFactory Get(ItemData.ItemIdentity identity)
        {
            return factories[(int)identity];
        }
        internal static void Load(ItemData.ItemIdentity identity, IItemFactory clientFactory)
        {
            factories[(int)identity] = clientFactory;
        }
    }
    internal class ClientItemFactory<TItem> : IItemFactory where TItem : IClientItem, new()
    {
        public Texture2D Texture { get; internal set; }

        public virtual IItem CreateNew(ItemData itemData)
        {
            return this.CreateClientItem(itemData);
        }

        public TItem CreateClientItem(ItemData itemData)
        {
            TItem returnItem = new TItem();
            returnItem.Data = itemData;
            returnItem.Texture = this.Texture;
            return returnItem;
        }
    }
}