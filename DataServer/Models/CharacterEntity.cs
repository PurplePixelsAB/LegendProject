using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataServer.Models
{
    public class Character
    {
        public Character()
        {
            this.Health = 100;
            this.Energy = 100;
            //this.Powers = new List<CharacterPowerLearned>();
        }

        [Key]
        public int Id { get; set; }
        
        public int MapId { get; set; }
        
        public int WorldX { get; set; }
        public int WorldY { get; set; }
        
        public string Name { get; set; }
        
        public int Health { get; set; }        
        public int Energy { get; set; }
        
        public int InventoryId { get; set; }
        public int? ArmorId { get; set; }
        public int? LeftHandId { get; set; }
        public int? RightHandId { get; set; }
    }
}