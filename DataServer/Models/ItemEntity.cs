using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataServer.Models
{
    public class Item
    {
        public Item()
        {
        }

        [Key]
        public int Id { get; set; }

        public int Identity { get; set; }
        public int? SubType { get; set; }

        public int Count { get; set; }

        public int? WorldMapId { get; set; }
        public int? WorldX { get; set; }
        public int? WorldY { get; set; }

        public int? ContainerId { get; set; }
    }
}