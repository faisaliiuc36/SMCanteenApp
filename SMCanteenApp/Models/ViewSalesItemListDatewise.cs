using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class ViewSalesItemListDatewise
	{
		public int    ItemCode { get; set; }
		public string ItemName  { get; set; }
		public string FileName  { get; set; }
		public int    Quantity     { get; set; }
		public double    DuePrice     { get; set; }
		public double    CashPrice     { get; set; }
		public double    TotalPrice     { get; set; }
	}
}