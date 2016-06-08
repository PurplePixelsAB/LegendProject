using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataServer.Models
{
    public class CharacterPower
    {
        [Key]
        public int Id { get; set; }        
        public int CharacterId { get; set; }
        public int Power { get; set; }
    }
}