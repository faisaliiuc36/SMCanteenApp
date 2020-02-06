using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class ViewSalesItem
	{
		



		public int SalesItemId { get; set; }


		public int SalesId { get; set; }
	


		[Display(Name = "Quantity")]
		public int Quantity { get; set; }

		[Display(Name = "Price/Unit")]
		public double Price { get; set; }


		

		[Display(Name = "Note")]
		public string Note { get; set; }
		public string ImageFile { get; set; }
		public string Name { get; set; }
		public int ItemId { get; set; }
	}
}