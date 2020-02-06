using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SMCanteenApp.Models
{
	public class Sale
	{
		[Key]
		[Display(Name = "Sales Id")]
		public int SalesId { get; set; }


		public int? EmployeeId { get; set; }
		[ForeignKey("EmployeeId")]
		public virtual Employee Employee { get; set; }

		public virtual List<SalesItemList> SalesItemLists { get; set; }


		[Display(Name = "Date")]
		[Required]
		public DateTime Date { get; set; }


		[Display(Name = "Total Amount")]
		public double Amount {
			get
			{
				if (SalesItemLists.Count > 0)
				{
					return SalesItemLists.Sum(a => a.Amount)-this.Discount;
				}
				else
					return 0;
			}
						  }


		[Display(Name = "Discount")]
		public double Discount { get; set; }

		[Display(Name = "Note")]
		public string Note { get; set; }
		
	}
}