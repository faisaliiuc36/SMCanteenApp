using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class Item
	{
		[Key]
		[Display(Name = "Item Code")]
		public int ItemCode { get; set; }


		[Display(Name = "Item Name")]
		[Required]
		public string ItemName { get; set; }


		[Display(Name = "Item Price")]
		[Required]
		public double Price { get; set; }


		public string ImageFile { get; set; }
		


		[Display(Name = "Note")]
		public string Note { get; set; }

		[Display(Name ="In Active?")]
		public bool InActive { get; set; }
		
	}
}