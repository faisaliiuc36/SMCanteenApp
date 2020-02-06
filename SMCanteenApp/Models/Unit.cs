using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMCanteenApp.Models
{
	public class Unit
	{
		
		public int UnitId { get; set; }


		public virtual List<Employee> Employees { get; set; }

		[Required]
		[Display(Name="Unit Name")]
		public string Name { get; set; }

	}
}