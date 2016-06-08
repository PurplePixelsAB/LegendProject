using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataServer.Models
{
    public class PlayerSession
    {
        public PlayerSession()
        {

        }

        [Key]
        public int Id { get; set; }
        public int CharacterId { get; set; }

        public string ClientAddress { get; set; }        
        public DateTime Created { get; set; }
        
    }
}