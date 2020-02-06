using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMCanteenApp.Models;

namespace SMCanteenApp.Controllers
{
    public class ItemsController : Controller
    {
        private SMCanteenDBContext db = new SMCanteenDBContext();

		// GET: Sales
		public ActionResult SalesItemList()
		{
			List<SalesItemList> sales = db.salesItemLists.Include(s => s.Item).Where(a => a.Sale.Date.Year == DateTime.Now.Year && a.Sale.Date.Month == DateTime.Now.Month && a.Sale.Date.Day == DateTime.Now.Day).ToList();

			List<ViewSalesItemListDatewise> viewSalesItems = GetSalesItemList(sales);

			return View(viewSalesItems.ToList());
		}

		private List<ViewSalesItemListDatewise> GetSalesItemList(List<SalesItemList> sales)
		{
			List<Item> items = db.items.ToList();
			List<ViewSalesItemListDatewise> viewSalesItems = new List<ViewSalesItemListDatewise>();

			foreach (Item item in items)
			{
				ViewSalesItemListDatewise viewSalesItem = new ViewSalesItemListDatewise();
				viewSalesItem.ItemCode = item.ItemCode;
				viewSalesItem.ItemName = item.ItemName;
				viewSalesItem.FileName = item.ImageFile;

				if (sales.Count > 0)
				{
					viewSalesItem.Quantity = sales.Where(a => a.ItemId == item.ItemCode).Sum(a => a.Quantity);
					viewSalesItem.CashPrice = sales.Where(a => a.ItemId == item.ItemCode && a.Sale.EmployeeId==null).Sum(a => a.Amount);
					viewSalesItem.DuePrice = sales.Where(a => a.ItemId == item.ItemCode && a.Sale.EmployeeId!=null).Sum(a => a.Amount);
					viewSalesItem.TotalPrice = sales.Where(a => a.ItemId == item.ItemCode ).Sum(a => a.Amount);
				}

				viewSalesItems.Add(viewSalesItem);

			}

			return viewSalesItems.ToList();
		}

		// GET: Items
		public ActionResult Index()
        {
            return View(db.items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemCode,ItemName,Price,ImageFile,Note,InActive")] Item item, HttpPostedFileBase Image)
        {

			if (Image != null)
			{
				try
				{
					if (Image.ContentLength > 0)
					{
						item.ImageFile = DateTime.Now.Date.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Date.Millisecond.ToString() + Path.GetFileName(Image.FileName);
						string ImagePath = Path.Combine(Server.MapPath("~/Image"), item.ImageFile);
						Image.SaveAs(ImagePath);

					}
				}

				catch
				{
					ViewBag.Message = " File Upload Failed!";
					return View(item);
				}


			}

			if (ModelState.IsValid)
            {
                db.items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemCode,ItemName,Price,ImageFile,Note,InActive")] Item item, HttpPostedFileBase Image)
		{

			if (Image != null)
			{
				try
				{
					if (Image.ContentLength > 0)
					{
						item.ImageFile = DateTime.Now.Date.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Date.Millisecond.ToString() + Path.GetFileName(Image.FileName);
						string ImagePath = Path.Combine(Server.MapPath("~/Image"), item.ImageFile);
						Image.SaveAs(ImagePath);

					}
				}

				catch
				{
					ViewBag.Message = " File Upload Failed!";
					return View(item);
				}


			}
			if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.items.Find(id);
            db.items.Remove(item);
            db.SaveChanges();
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
		public JsonResult GetSalesByDate( DateTime Date1, DateTime Date2)
		{

			List<SalesItemList> sales = db.salesItemLists.Include(s => s.Item).Include(s=>s.Sale).Where(a => a.Sale.Date>=Date1 && a.Sale.Date <= Date2).ToList();

			List<ViewSalesItemListDatewise> viewSalesItems = GetSalesItemList(sales);


			return Json(viewSalesItems, JsonRequestBehavior.AllowGet);
			

		}
	}
}
