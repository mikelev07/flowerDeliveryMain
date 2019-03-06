using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlowersDelivery.Models;
using PagedList;

namespace FlowersDelivery.Controllers
{
    public class FlowerItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FlowerItem
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.FlowerItems
                        select i;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Category.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;
                default:  // Имя по возрастанию
                    items = items.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            //для списка категорий
            ViewBag.Categories = new MultiSelectList(db.Categories,"Id", "Name");

            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: FlowerItem/Details/5

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowerItem flowerItem = await db.FlowerItems.FindAsync(id);
            if (flowerItem == null)
            {
                return HttpNotFound();
            }
            return View(flowerItem);
        }

        // GET: FlowerItem/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: FlowerItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FlowerItem flowerItem)
        {
            if (ModelState.IsValid)
            {
                db.FlowerItems.Add(flowerItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", flowerItem.CategoryId);

            return View(flowerItem);
        }

        // GET: FlowerItem/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowerItem flowerItem = await db.FlowerItems.FindAsync(id);
            if (flowerItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", flowerItem.CategoryId);
            return View(flowerItem);
        }

        // POST: FlowerItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FlowerItem flowerItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flowerItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", flowerItem.CategoryId);
            return View(flowerItem);
        }

        // GET: FlowerItem/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowerItem flowerItem = await db.FlowerItems.FindAsync(id);
            if (flowerItem == null)
            {
                return HttpNotFound();
            }
            return View(flowerItem);
        }

        // POST: FlowerItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FlowerItem flowerItem = await db.FlowerItems.FindAsync(id);
            db.FlowerItems.Remove(flowerItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
