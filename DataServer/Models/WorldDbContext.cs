using Data;
using LegendWorld.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataServer.Models
{
    public class WorldDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WorldDbContext() : base("name=WorldDbContext")
        {
        }

        public DbSet<ItemData> Items { get; set; }
        //public DbSet<GroundItem> GroundItems { get; set; }
        public DbSet<PlayerSession> PlayerSessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Item>()
            //            .Map<ContainerItem>(m => m.Requires("Category").HasValue(ItemCategory.Container))
            //            .Map<ArmorItem>(m => m.Requires("Category").HasValue(ItemCategory.Armor))
            //            .Map<WeaponItem>(m => m.Requires("Category").HasValue(ItemCategory.Weapon))
            //            .Map<ConsumableItem>(m => m.Requires("Category").HasValue(ItemCategory.Consumable))
            //            .Map<StackableItem>(m => m.Requires("Category").HasValue(ItemCategory.Stackable));
            base.OnModelCreating(modelBuilder);
        }

    }
}
