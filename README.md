# flowerDeliveryMain

Основной кейс

1. Обращение к методу Index (контроллер ShoppingCartController)
2. var cart = ShoppingCart.GetCart(this.HttpContext);
   создаем объект корзины + в статический метод getCart передаем контекст запроса
3. (метод getcart) ... cart.ShoppingCartId = cart.GetCartId(context);
   пытаемся получить айди корзины посредством конектста запроса
4. Вернули ключ сессии
5. Вернули объект бизнес-модели корзины
6. Завершаем заказ, CreateOrder (устанавливаем orderTotal)
7. Очищаем корзинку  