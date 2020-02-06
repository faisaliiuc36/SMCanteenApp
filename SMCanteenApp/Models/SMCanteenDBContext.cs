using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace SMCanteenApp.Models
{
	public class SMCanteenDBContext : DbContext
	{
		public DbSet<Employee> employees { get; set; }
		public DbSet<Item> items { get; set; }
		public DbSet<Sale> sales { get; set; }
		public DbSet<SalesItemList> salesItemLists { get; set; }
		public DbSet<Unit> units { get; set; }
	}
}