using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectTrade.Models
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerDetail> CustomersDetail { get; set; }
        public DbSet<SupplierDetail> SupplierDetail { get; set; }
        public DbSet<MeetingDetail> MeetingDetail { get; set; }
        public DbSet<TradeTransactions> TradeTransactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TradeTransactions>().HasNoKey();
        }

    }
    
}


