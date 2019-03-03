using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FlowersDelivery.Models
{
    /// <summary>
    /// Класс заказа (хранение в БД)
    /// Итоговое формирование цепочки заказа
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Имя не допустимо")]
        [DisplayName("Имя")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия не допсутима")]
        [DisplayName("Фамилия")]
        [StringLength(160)]
        public string LastName { get; set; }

        public bool SaveInfo { get; set; }

        // Детали заказа
        // Связь 1 ко многим
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

    }
}