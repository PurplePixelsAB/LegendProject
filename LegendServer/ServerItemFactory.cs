using System;
using Data;
using LegendWorld.Data;
using Network;
using LegendWorld.Data.Items;

namespace UdpServer
{
    internal class ServerItemFactory : IItemFactory
    {
        private ItemData.ItemIdentity identity;

        public ServerItemFactory(ItemData.ItemIdentity identity)
        {
            this.identity = identity;
        }

        public IItem CreateNew(ItemData itemData)
        {
            IItem newItem = this.CreateItem(itemData.Identity);
            newItem.Data = itemData;

            return newItem;
        }

        private IItem CreateItem(ItemData.ItemIdentity identity)
        {
            switch (identity)
            {
                case ItemData.ItemIdentity.Bag:
                    return new BagItem();
                case ItemData.ItemIdentity.Gold:
                    return new GoldItem();
                case ItemData.ItemIdentity.LeatherArmor:
                    return new LeatherArmorItem();
                case ItemData.ItemIdentity.PlateArmor:
                    return new PlateArmorItem();
                case ItemData.ItemIdentity.ClothRobe:
                    return new ClothArmorItem();
                case ItemData.ItemIdentity.Dagger:
                    return new DaggerItem();
                case ItemData.ItemIdentity.Sword:
                    return new SwordItem();
                case ItemData.ItemIdentity.Bow:
                    return new BowItem();
                case ItemData.ItemIdentity.PowerScoll:
                    return new PowerScrollItem();
                case ItemData.ItemIdentity.Bandage:
                    return new BandageItem();
                case ItemData.ItemIdentity.Corpse:
                    return new CorpseItem();
                default:
                    throw new Exception("Unknown ItemIdentity!");
            }
        }
    }
}