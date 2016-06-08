using System;
using Data;
using LegendWorld.Data;
using Network;
using LegendWorld.Data.Items;

namespace UdpServer
{
    internal class ServerItemFactory : IItemFactory
    {
        private ItemIdentity identity;

        public ServerItemFactory(ItemIdentity identity)
        {
            this.identity = identity;
        }

        public Item CreateNew(ItemModel itemData)
        {
            Item newItem = this.CreateItem((ItemIdentity)itemData.Identity);
            newItem.LoadData(itemData);
            //newItem.Data = itemData;

            return newItem;
        }

        private Item CreateItem(ItemIdentity identity)
        {
            switch (identity)
            {
                case ItemIdentity.Bag:
                    return new BagItem();
                case ItemIdentity.Gold:
                    return new GoldItem();
                case ItemIdentity.LeatherArmor:
                    return new LeatherArmorItem();
                case ItemIdentity.PlateArmor:
                    return new PlateArmorItem();
                case ItemIdentity.ClothRobe:
                    return new ClothArmorItem();
                case ItemIdentity.Dagger:
                    return new DaggerItem();
                case ItemIdentity.Sword:
                    return new SwordItem();
                case ItemIdentity.Bow:
                    return new BowItem();
                case ItemIdentity.PowerScoll:
                    return new PowerScrollItem();
                case ItemIdentity.Bandage:
                    return new BandageItem();
                case ItemIdentity.Corpse:
                    return new CorpseItem();
                default:
                    throw new Exception("Unknown ItemIdentity!");
            }
        }
    }
}