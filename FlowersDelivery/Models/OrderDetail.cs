using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowersDelivery.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Связь 1 ко многим
        // Заказ может содержать много сущностей FlowerItem
        public int FlowerItemId { get; set; }
        // Свойство virtual объявляем для позднего связывания
        // Подгрузка данных осуществляется при обращении к навигационному свойству
        public virtual FlowerItem FlowerItem { get; set; }

        // Связь 1 ко многим
        // Заказ может содержать много сущностей Order
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}