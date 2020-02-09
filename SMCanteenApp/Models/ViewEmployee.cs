using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class ViewEmployee
	{
		public int Id { get; set; }

		[Display(Name = "Employee Id")]
		public string Emp_Id { get; set; }

		[Display(Name = "Employee Full Name")]
		public string EmpFullName { get; set; }

		[Display(Name = "Badge Number")]
		public string BadgeNumber { get; set; }

		[Display(Name = "Mobile No")]
		public string MobileNo { get; set; }

		public string Unit { get; set; }

		[Display(Name = "In Active?")]
		public bool InActive { get; set; }
	}
}