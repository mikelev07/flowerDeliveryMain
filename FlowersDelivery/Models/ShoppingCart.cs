using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlowersDelivery.Models
{
    /// <summary>
    /// Класс бизнес-логики для корзины
    /// </summary>
    public class ShoppingCart
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        #region Функционал работы с корзиной

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Вспомогательный метод для упрощения вызовова корзины покупок
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        /// <summary>
        /// Добавление в корзину
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int AddToCart(FlowerItem item)
        {
            // Получаем конкретную корзину
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.FlowerItemId == item.Id);

            if (cartItem == null)
            {
                // Создаем новую корзину, если не существует 
                cartItem = new Cart
                {
                    FlowerItemId = item.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                // Если объект FlowerItem существует в корзине, 
                // добавляем 1 к кол-ву
                cartItem.Count++;
            }
            // Сохранить изменения
            db.SaveChanges();

            return cartItem.Count;
        }

        /// <summary>
        /// Удаляем из корзины
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int RemoveFromCart(int id)
        {
            // Получаем корзину
            var cartItem = db.Carts.FirstOrDefault(
                cart => cart.CartId == ShoppingCartId
                && cart.FlowerItemId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                
                db.SaveChanges();
            }
            return itemCount;
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }

        /// <summary>
        /// Получить все товары в корзине
        /// </summary>
        /// <returns>Товары в корзине</returns>
        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        /// <summary>
        /// Получить кол-во товаров в корзине
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            // Получить количество каждого товара в корзине и суммировать их
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            // Возвращаем 0, если все записи нулевые
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            // Умножаем цену товара на количество этого товара, чтобы получить
            // текущую цена для каждого из этих товаров в корзине
            // Суммируем все итоговые цены товара, чтобы получить сумму в корзине
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.FlowerItem.Price).Sum();

            return total ?? decimal.Zero;
        }

        /// <summary>
        /// Создать заказ
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.OrderDetails = new List<OrderDetail>();

            var cartItems = GetCartItems();

            // перебираем товары в корзине,
            // добавляем детали заказа для каждого
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    FlowerItemId = item.FlowerItemId,
                    OrderId = order.Id,
                    UnitPrice = item.FlowerItem.Price,
                    Quantity = item.Count
                };

                // Установить сумму заказа в корзине
                orderTotal += (item.Count * item.FlowerItem.Price);
                order.OrderDetails.Add(orderDetail);
                db.OrderDetails.Add(orderDetail);

            }

            // Установить сумму заказа на счетчик orderTotal
            order.Total = orderTotal;
            // Сохранить заказ
            db.SaveChanges();
            // Очистить корзину
            EmptyCart();
            // Вернуть идентификатор заказа в качестве номера подтверждения
            return order;
        }

        // Мы используем HttpContextBase, чтобы разрешить доступ к файлам cookie.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Создать новый случайный GUID с помощью класса System.Guid
                    Guid tempCartId = Guid.NewGuid();
                    // Отправить tempCartId обратно клиенту как cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // Когда пользователь вошел в систему, переносим его корзину покупок
        // связываем с именем пользователя
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }
        #endregion

    }
}