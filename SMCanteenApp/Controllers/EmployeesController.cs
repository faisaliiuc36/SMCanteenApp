using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMCanteenApp.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace SMCanteenApp.Controllers
{
    public class EmployeesController : Controller
    {
        private SMCanteenDBContext db = new SMCanteenDBContext();


		// GET: Employees
		public ActionResult EmployeeDueReport()
		{
			ViewBag.UnitId = new SelectList(db.units, "UnitId", "Name");
			return View();
		}
		[HttpPost]
		// GET: Employees
		public ActionResult EmployeeDueReport(DateTime Date, int UnitId)
		{
			List<Sale> sales = db.sales.Where(a => a.Date.Month == Date.Month && a.Date.Year == Date.Year).ToList();
			List<Employee> employees = db.employees.Where(a => a.UnitId==UnitId).ToList();
			List<ViewEmployeeDueReport> viewEmployeeDueReports = new List<ViewEmployeeDueReport>();
			int Count = 0;
			if (employees.Count>0)
			{
				
				foreach(Employee  employee in employees)
				{
					ViewEmployeeDueReport viewEmployeeDueReport = new ViewEmployeeDueReport();
					Count = Count + 1;
					viewEmployeeDueReport.SL = Count;
					viewEmployeeDueReport.EmpId = employee.Emp_Id;
					viewEmployeeDueReport.EmpName = employee.EmpFullName;
					viewEmployeeDueReport.MobileNo = employee.MobileNo;


					viewEmployeeDueReport.Discount = sales.Where(a => a.EmployeeId == employee.Id).Sum(a => a.Discount);

					viewEmployeeDueReport.Due = sales.Where(a => a.EmployeeId == employee.Id).Sum(a => a.Amount);
					viewEmployeeDueReport.Payable = viewEmployeeDueReport.Due + viewEmployeeDueReport.Discount;

					viewEmployeeDueReports.Add(viewEmployeeDueReport);

				}
			}

			Unit unit = db.units.Find(UnitId);
			// Print

			try
			{
				ReportDocument rd = new ReportDocument();
				rd.Load(Path.Combine(Server.MapPath("~/"), "EmployeeDueReport.rpt"));
				rd.SetDataSource(viewEmployeeDueReports.ToList());
				rd.SetParameterValue("Unit",unit.Name);
				rd.SetParameterValue("Month",Date.ToString("MMMM-yyyy"));
				rd.SetParameterValue("Count",Count);
				
				Response.Buffer = false;
				Response.ClearContent();
				Response.ClearHeaders();


				rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
				rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
				rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

				Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
				stream.Seek(0, SeekOrigin.Begin);

				return File(stream, "application/pdf", unit.Name+"("+ Date.ToString("MMMM-yyyy") + ").pdf");

			}
			catch
			{

			}




			ViewBag.UnitId = new SelectList(db.units, "UnitId", "Name", UnitId);
			return View();
		}

		// GET: Employees
		public ActionResult Index()
        {
            return View(db.employees.ToList());
        }
		private List<Employee> GetNewEmployee(string DBName, string UnitName)
		{
			List<Employee> employees = new List<Employee>();





			//string connectionString = @"Data Source=10.10.0.84/"+DBName+";Initial Catalog=SecurityAttendanceDB;Persist Security Info=True;User ID=sa;Password=SM3xpress20";
			string connectionString = @"Data Source=10.10.0.84\SQLEXPRESS;Initial Catalog="+DBName+";Persist Security Info=True;User ID=sa;Password=SM3xpress20";
			//string connectionString = @"Data Source=10.10.0.84\SQLEXPRESS;Initial Catalog=SaadMusaAttendance;Persist Security Info=True;User ID=sa;Password=SM3xpress20";
			SqlConnection connection = new SqlConnection(connectionString);

			string query = "select Emp_Id,BadgeNumber, EmpFullName,PhoneNumber,ActiveStatus from employees";

			SqlCommand command = new SqlCommand(query, connection);
			//int UpdateRows = 0;
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				while (reader.Read())
				{

					Employee emp = new Employee();
					emp.Emp_Id = (int)reader["Emp_Id"];
					bool IsActive = (bool)reader["ActiveStatus"];
					emp.BadgeNumber = reader["BadgeNumber"].ToString();
					emp.EmpFullName = reader["EmpFullName"].ToString();
					emp.MobileNo = reader["PhoneNumber"].ToString();
					emp.InActive= (bool)reader["ActiveStatus"];

					employees.Add(emp);


				}
			}

			reader.Close();
			connection.Close();




			List<Employee> employees2 = employees.Where(a => a.BadgeNumber.StartsWith("100000")).ToList();



			return employees2;
		}

		private Unit CreateUnit(string unitName)
		{
			Unit unit = new Unit();
			unit.Name = unitName;
			db.units.Add(unit);
			db.SaveChanges();
			return unit;
		}

		
		public ActionResult ImportNewEmployee()
		{

			return View();
		}
		[HttpPost]
		public ActionResult ImportNewEmployee(string DBName, string UnitName)
		{

			List<Employee> employees = new List<Employee>();
			try
			{
				employees = GetNewEmployee(DBName, UnitName);
			}
			catch
			{
				ViewBag.Message = "Please Input Correct DB Name";
				return View();
			}


			int UpdateRows = 0;
			int AddRows = 0;

			if (employees.Count > 0)
			{
				foreach (Employee emp in employees)
				{



					//Check UserId

					var IsEmployeeExist = db.employees.FirstOrDefault(a => a.Emp_Id == emp.Emp_Id);

					//var ISUserIdExist

					if (IsEmployeeExist == null && emp.InActive == true)
					{

						var UnitSearch = db.units.FirstOrDefault(a => a.Name == UnitName);

						if (UnitSearch == null)
						{
							Unit unit = CreateUnit(UnitName);
							emp.UnitId = unit.UnitId;
						}
						else
						{
							emp.UnitId = UnitSearch.UnitId;
						}
						emp.InActive = false;
						db.employees.Add(emp);
						db.SaveChanges();
						AddRows = AddRows + 1;

					}

					else if (IsEmployeeExist != null && emp.InActive == false)
					{
						
						IsEmployeeExist.InActive = true;
						db.Entry(IsEmployeeExist).State = EntityState.Modified;
						db.SaveChanges();
						UpdateRows = UpdateRows + 1;

					}

					 if (IsEmployeeExist != null && IsEmployeeExist.UnitId == null)
					{

						var UnitSearch = db.units.FirstOrDefault(a => a.Name == UnitName);

						if (UnitSearch == null)
						{
							Unit unit = CreateUnit(UnitName);
							IsEmployeeExist.UnitId = unit.UnitId;
						}
						else
						{
							IsEmployeeExist.UnitId = UnitSearch.UnitId;
						}

						//IsEmployeeExist.InActive = true;
						db.Entry(IsEmployeeExist).State = EntityState.Modified;
						db.SaveChanges();
						UpdateRows = UpdateRows + 1;

					}



				}
			}

			ViewBag.Message = "Updated Rows=" + UpdateRows + ",Added Rows=" + AddRows ;
			return View("Index",db.employees.ToList());
		}

		// GET: Employees/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Emp_Id,EmpFullName,BadgeNumber,MobileNo")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.employees.Find(id);
            db.employees.Remove(employee);
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


		public JsonResult GetEmployeeById(string EmpId)
		{
			try
			{
				int empid = Convert.ToInt32(EmpId);
				List<Employee> employees = db.employees.Where(a => a.Emp_Id==empid || a.MobileNo==EmpId).ToList();
				List<ViewEmployee> viewEmployees = new List<ViewEmployee>();
				foreach(Employee employee in employees)
				{
					ViewEmployee viewEmployee = new ViewEmployee { Id = employee.Id, EmpFullName = employee.EmpFullName, BadgeNumber = employee.BadgeNumber, Emp_Id = employee.Emp_Id, InActive = employee.InActive, MobileNo = employee.MobileNo, Unit = employee.Unit.Name };
					viewEmployees.Add(viewEmployee);
				}
				return Json(viewEmployees, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				List<Employee> employees = db.employees.Where(a => a.MobileNo == EmpId).ToList();

				List<ViewEmployee> viewEmployees = new List<ViewEmployee>();
				foreach (Employee employee in employees)
				{
					ViewEmployee viewEmployee = new ViewEmployee { Id = employee.Id, EmpFullName = employee.EmpFullName, BadgeNumber = employee.BadgeNumber, Emp_Id = employee.Emp_Id, InActive = employee.InActive, MobileNo = employee.MobileNo, Unit = employee.Unit.Name };
					viewEmployees.Add(viewEmployee);
				}
				return Json(viewEmployees, JsonRequestBehavior.AllowGet);
			}
		}
	}
}
