using Data;
using LegendWorld.Data.Items;
using LegendWorld.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    [KnownType(typeof(AbilityScrollItem))]
    [JsonConverter(typeof(ItemJsonConverter))]
    [DataContract]
    public class Item
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public ItemIdentity Identity { get; set; }

        [DataMember]
        public ItemCategory Category { get; set; }

        [NotMapped]
        public int Weight { get; protected set; }

        //public virtual string GetInventoryString()
        //{
        //    if (this.Data.IsStackable)
        //    {
        //        return string.Format("{1} {0}", this.Data.Name, this.StackCount);
        //    }
        //    else
        //        return this.Data.Name;
        //}

        //public void OnUse()
        //{

        //}
        private class ItemJsonConverter : JsonCreationConverter<Item>
        {
            protected override Item Create(Type objectType,
              Newtonsoft.Json.Linq.JObject jObject)
            {
                //TODO: read the raw JSON object through jObject to identify the type
                //e.g. here I'm reading a 'typename' property:
                ItemIdentity itemIdentity = (ItemIdentity)jObject.Value<int>("Identity");

                if (itemIdentity.Equals(ItemIdentity.AbilityScoll))
                    return new AbilityScrollItem();
                else
                    return new Item();
                //if ("DerivedType".Equals(jObject.Value<string>("typename"))
                //  return new DerivedClass();
                //else
                //    return new DefaultClass();

                    //now the base class' code will populate the returned object.
            }
        }
    }
}
