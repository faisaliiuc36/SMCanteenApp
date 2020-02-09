using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class Employee
	{
		public int Id { get; set; }

		[Display(Name = "Employee Id")]
		public string Emp_Id { get; set; }

		[Display(Name = "Employee Full Name")]
		public string EmpFullName { get; set; }


		[Display(Name = "Mobile No")]
		public string MobileNo { get; set; }

		public int? UnitId { get; set; }
		[ForeignKey("UnitId")]
		public virtual Unit Unit { get; set; }

		[Display(Name ="In Active?")]
		public bool InActive { get; set; }



	}
}