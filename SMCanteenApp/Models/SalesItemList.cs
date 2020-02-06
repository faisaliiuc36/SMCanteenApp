using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class SalesItemList
	{
		[Key]
		[Display(Name = "Sales Item Id")]
		public int SalesItemId { get; set; }


		public int SalesId { get; set; }
		[ForeignKey("SalesId")]
		public virtual Sale Sale { get; set; }

		public int ItemId { get; set; }
		[ForeignKey("ItemId")]
		public virtual Item Item { get; set; }


		[Display(Name = "Quantity")]
		public int Quantity { get; set; }

		[Display(Name = "Price/Unit")]
		public double Price { get; set; }


		[Display(Name = "Amount")]
		public double Amount { get { return Quantity * Price; } }
		

		[Display(Name = "Note")]
		public string Note { get; set; }
	}
}