using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FlowersDelivery.Models
{
    /// <summary>
    /// Класс товара (цветы)
    /// </summary>
    public class FlowerItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя не допустимо")]
        [MaxLength(30)]
        [DisplayName("Название")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Цена не допсутима")]
        [Range(0.01, 99999.99, ErrorMessage = "Цена вне диапазона")]
        [DisplayName("Цена")]
        public decimal Price { get; set; }

        // 1 ко многим для категории товара
        [DisplayName("Категория")]
        public int CategoryId { get; set; }
        [DisplayName("Категория")]
        public virtual Category Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public FlowerItem()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}