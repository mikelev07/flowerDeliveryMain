using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FlowersDelivery.Models
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Категория")]
        public string Name { get; set; }

        public ICollection<FlowerItem> FlowerItems { get; set; }

        public Category()
        {
            FlowerItems = new List<FlowerItem>();
        }
    }
}