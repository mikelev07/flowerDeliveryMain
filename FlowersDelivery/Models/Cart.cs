using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowersDelivery.Models
{
    /// <summary>
    /// Класс корзины
    /// </summary>
    public class Cart
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

        // Связь 1 ко многим
        // Корзина может содержать много сущностей FlowerItem
        public int FlowerItemId { get; set; }
        // Свойство virtual объявляем для позднего связывания
        // Подгрузка данных осуществляется при обращении к навигационному свойству
        public virtual FlowerItem FlowerItem { get; set; }

    }
}