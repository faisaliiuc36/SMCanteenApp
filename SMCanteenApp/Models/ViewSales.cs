using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class ViewSales
	{
		public int SalesId { get; set; }


	
		public string Type { get; set; }
		public string Name { get; set; }
		public string EmpId { get; set; }

		public string Date { get; set; }


		public double Price { get; set; }
		public double Discount { get; set; }
		public double Payable { get; set; }

		
		public string Note { get; set; }
	}
}