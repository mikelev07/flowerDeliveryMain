using FlowersDelivery.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FlowersDelivery.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            var previousOrder = db.Orders.FirstOrDefault(x => x.Username == User.Identity.Name);

            if (previousOrder != null)
                return View(previousOrder);
            else
                return View();
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
          //  try
         //   {
             order.Username = User.Identity.Name;
             order.Email = User.Identity.Name;
             order.PurchaseDate = DateTime.Now;
                
             var currentUserId = User.Identity.GetUserId();

             db.Orders.Add(order);
             await db.SaveChangesAsync();

             var cart = ShoppingCart.GetCart(this.HttpContext);
             order = cart.CreateOrder(order);

             return RedirectToAction("Complete", new { id = order.Id });
        //    }
           // catch
          //  {
            //    return View(order);
           // }
        }

        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            bool isValid = db.Orders.Any(
                o => o.Id == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}