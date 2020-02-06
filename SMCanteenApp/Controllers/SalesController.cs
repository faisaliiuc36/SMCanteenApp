using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMCanteenApp.Models;

namespace SMCanteenApp.Controllers
{
    public class SalesController : Controller
    {
        private SMCanteenDBContext db = new SMCanteenDBContext();

        // GET: Sales
        public ActionResult Index()
        {

			ViewBag.UnitId = new SelectList(db.units, "UnitId", "Name");

			var sales = db.sales.Include(s => s.Employee).Where(a=>a.Date.Year==DateTime.Now.Year && a.Date.Month==DateTime.Now.Month && a.Date.Day==DateTime.Now.Day);
            return View(sales.ToList());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {


			//List Item
			List<ViewSalesItem> viewSalesItems = new List<ViewSalesItem>();
			List<Item> items = db.items.Where(a => a.InActive == false).ToList();

			if(items.Count>0)
			{
				foreach(Item item in items)
					{
					ViewSalesItem viewSalesItem = new ViewSalesItem();
					viewSalesItem.ItemId = item.ItemCode;
					viewSalesItem.Name = item.ItemName;
					viewSalesItem.ImageFile = item.ImageFile;
					viewSalesItem.Price = item.Price;
					viewSalesItems.Add(viewSalesItem);
				}
			}

			ViewBag.ItemList = viewSalesItems.ToList();

			Sale sale = new Sale();
            return View(sale);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Sale sale)
        {
            if (ModelState.IsValid)
            {
				Employee employee = db.employees.Find(sale.EmployeeId);

				if(employee == null)
				{
					sale.EmployeeId = null;
				}

				if(sale.SalesId==0)
				{
					db.sales.Add(sale);
					db.SaveChanges();
				}
				else
				{
					//List<SalesItemList> salesItemLists = db.salesItemLists.Where(a => a.SalesId == sale.SalesId).ToList();
					//db.salesItemLists.RemoveRange(salesItemLists);
					db.Entry(sale).State = EntityState.Modified;
					Array.ForEach(sale.SalesItemLists.ToArray(), a => db.Entry(a).State = EntityState.Modified );

					db.SaveChanges();
				}
                

                return RedirectToAction("Details", new { id=sale.SalesId});
            }

			//List Item
			List<ViewSalesItem> viewSalesItems = new List<ViewSalesItem>();
			List<Item> items = db.items.Where(a => a.InActive == false).ToList();

			if (items.Count > 0)
			{
				foreach (Item item in items)
				{
					

					ViewSalesItem viewSalesItem = new ViewSalesItem();
					viewSalesItem.ItemId = item.ItemCode;
					viewSalesItem.Name = item.ItemName;
					viewSalesItem.ImageFile = item.ImageFile;
					viewSalesItem.Price = item.Price;
					viewSalesItems.Add(viewSalesItem);

					if(sale.SalesItemLists.Count>0)
					{
						var salesItem = sale.SalesItemLists.FirstOrDefault(a => a.ItemId == item.ItemCode);
						if (salesItem != null)
						{
							viewSalesItem.Quantity = salesItem.Quantity;
						}
					}

				}
			}

			ViewBag.ItemList = viewSalesItems.ToList();

			ViewBag.EmployeeId = new SelectList(db.employees, "Id", "EmpFullName");
			return View(sale);
		}

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
			//List Item
			List<ViewSalesItem> viewSalesItems = new List<ViewSalesItem>();
			List<Item> items = db.items.Where(a => a.InActive == false).ToList();

			if (items.Count > 0)
			{
				foreach (Item item in items)
				{


					ViewSalesItem viewSalesItem = new ViewSalesItem();
					viewSalesItem.ItemId = item.ItemCode;
					viewSalesItem.Name = item.ItemName;
					viewSalesItem.ImageFile = item.ImageFile;
					viewSalesItem.Price = item.Price;
					viewSalesItem.SalesId = sale.SalesId;
					viewSalesItems.Add(viewSalesItem);

					if (sale.SalesItemLists.Count > 0)
					{
						var salesItem = sale.SalesItemLists.FirstOrDefault(a => a.ItemId == item.ItemCode);
						if (salesItem != null)
						{
							viewSalesItem.Quantity = salesItem.Quantity;
							viewSalesItem.SalesItemId = salesItem.SalesItemId;
						}
					}

				}
			}

			ViewBag.ItemList = viewSalesItems.ToList();

			ViewBag.EmployeeId = new SelectList(db.employees, "Id", "EmpFullName");
			return View("Create",sale);
		}

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalesId,EmployeeId,Date,Discount,Note")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.employees, "Id", "EmpFullName", sale.EmployeeId);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.sales.Find(id);
            db.sales.Remove(sale);
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

		public JsonResult GetEmployeeId(string Prefix)
		{
			int id = 0;
			try
			{
				id = Convert.ToInt32(Prefix);
			}
			catch
			{

			}
			//Note : you can bind same list from database  
			List<Employee> employees = db.employees.ToList();
			//Searching records from list using LINQ query  
			var emplIst = (from N in employees
							   where N.Emp_Id.Equals(id) || N.MobileNo.Contains(Prefix)
							   select new {N.Id, N.Emp_Id, N.EmpFullName, N.MobileNo });
			return Json(emplIst, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetEmployeeNameById(string EmployeeId)
		{
			int id = 0;
			try
			{
				id = Convert.ToInt32(EmployeeId);
			}
			catch
			{

			}

			//Note : you can bind same list from database  
			List<Employee> employees = db.employees.ToList();
			//Searching records from list using LINQ query  
			var emplIst = (from N in employees
						   where N.Id.Equals(id)
						   select new {N.EmpFullName, N.MobileNo });
			return Json(emplIst, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetSalesByEmployeeNDate(int? EmployeeId, DateTime? Date1, DateTime? Date2, int? TypeId)
		{
			List<ViewSales> viewSales = new List<ViewSales>();

			if (EmployeeId != null && Date1 != null && Date2 != null)
			{
				List<Sale> sales = db.sales.Where(a => a.EmployeeId == EmployeeId && a.Date >= Date1 && a.Date <= Date2).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
				

			}
			else if (Date1 != null && Date2 != null)
			{
				List<Sale> sales = db.sales.Where(a =>a.Date >= Date1 && a.Date <= Date2).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
			
			}

			else if (EmployeeId != null )
			{
				List<Sale> sales = db.sales.Where(a => a.EmployeeId == EmployeeId && a.Date.Year==DateTime.Now.Year && a.Date.Month==DateTime.Now.Month && a.Date.Day==DateTime.Now.Day).Include(a => a.Employee).ToList();
			    viewSales = GetViewSales(sales);
			   
			}

			if (TypeId == 1)
			{
				return Json(viewSales.Where(a => a.EmpId != null), JsonRequestBehavior.AllowGet);
			}
			else if(TypeId==2)
			{
				return Json(viewSales.Where(a => a.EmpId == null), JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(viewSales, JsonRequestBehavior.AllowGet);
			}


		}
		public JsonResult GetSalesByUnitNDate(DateTime? Date1, DateTime? Date2, int? UnitId, int? TypeId)
		{
			List<ViewSales> viewSales = new List<ViewSales>();

			if (Date1 != null && Date2 != null && UnitId != null)
			{
				List<Sale> sales = db.sales.Where(a => a.Date >= Date1 && a.Date <= Date2 && a.Employee.UnitId == UnitId).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
				//return Json(viewSales, JsonRequestBehavior.AllowGet);

			}
		
			else if (Date1 != null && Date2 != null)
			{
				List<Sale> sales = db.sales.Where(a => a.Date >= Date1 && a.Date <= Date2).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
				
			}

		
			else if (UnitId != null)
			{
				List<Sale> sales = db.sales.Where(a => a.Employee.UnitId == UnitId && a.Date.Year == DateTime.Now.Year && a.Date.Month == DateTime.Now.Month && a.Date.Day == DateTime.Now.Day).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
				
			}

			if (TypeId == 1)
			{
				return Json(viewSales.Where(a => a.EmpId != null), JsonRequestBehavior.AllowGet);
			}
			else if (TypeId == 2)
			{
				return Json(viewSales.Where(a => a.EmpId == null), JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(viewSales, JsonRequestBehavior.AllowGet);
			}

		}
		public JsonResult GetSalesByDate(int? EmployeeId, DateTime? Date1, DateTime? Date2, int? UnitId, int? TypeId)
		{
			List<ViewSales> viewSales = new List<ViewSales>();

			if (Date1 != null && Date2 != null && UnitId != null)
			{
				List<Sale> sales = db.sales.Where(a => a.Date >= Date1 && a.Date <= Date2 && a.Employee.UnitId == UnitId).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
			

			}
			else if (Date1 != null && Date2 != null && EmployeeId != null)
			{
				List<Sale> sales = db.sales.Where(a => a.Date >= Date1 && a.Date <= Date2 && a.EmployeeId == EmployeeId).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);

			}

			else if (Date1 != null && Date2 != null)
			{
				List<Sale> sales = db.sales.Where(a => a.Date >= Date1 && a.Date <= Date2).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
			}


			
			else if (UnitId != null)
			{
				List<Sale> sales = db.sales.Where(a => a.Employee.UnitId == UnitId && a.Date.Year == DateTime.Now.Year && a.Date.Month == DateTime.Now.Month && a.Date.Day == DateTime.Now.Day).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
			}
			else if (EmployeeId != null)
			{
				List<Sale> sales = db.sales.Where(a => a.EmployeeId == EmployeeId && a.Date.Year == DateTime.Now.Year && a.Date.Month == DateTime.Now.Month && a.Date.Day == DateTime.Now.Day).Include(a => a.Employee).ToList();
				viewSales = GetViewSales(sales);
			}

			if (TypeId == 1)
			{
				return Json(viewSales.Where(a => a.EmpId != null), JsonRequestBehavior.AllowGet);
			}
			else if (TypeId == 2)
			{
				return Json(viewSales.Where(a => a.EmpId == null), JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(viewSales, JsonRequestBehavior.AllowGet);
			}

		}

		private List<ViewSales> GetViewSales(List<Sale> sales)
		{
			List<ViewSales> viewSales = new List<ViewSales>();
			foreach (Sale l in sales)
			{
				ViewSales viewsale = new ViewSales();

				viewsale.SalesId = l.SalesId;

				if (l.EmployeeId == null)
				{
					viewsale.Type = "Cash";
				}
				else
				{
					viewsale.Type = "Due";
					viewsale.Name = l.Employee.EmpFullName;
					viewsale.EmpId = l.Employee.Emp_Id;
				}

				
				viewsale.Date = l.Date.ToString("dd-MMMM-yyyy");
				viewsale.Price = l.Discount + l.Amount;
				viewsale.Discount = l.Discount;
				viewsale.Payable = l.Amount;
				viewsale.Note = l.Note;

				viewSales.Add(viewsale);

			}
			return viewSales;
		}
	}
}
