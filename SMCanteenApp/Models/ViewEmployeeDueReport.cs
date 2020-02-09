using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class ViewEmployeeDueReport
	{
		public int SL { get; set; }
		public string EmpId { get; set; }
		public string EmpName { get; set; }
		public string MobileNo { get; set; }
		public double Due { get; set; }
		public double Discount { get; set; }
		public double Payable { get; set; }
	}
}