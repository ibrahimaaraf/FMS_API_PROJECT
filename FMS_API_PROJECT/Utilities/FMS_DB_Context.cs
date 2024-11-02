using FMS_API_PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FMS_API_PROJECT.Utilities
{
    public class FMS_DB_Context: DbContext
    {
        public FMS_DB_Context() : base("ConnectionStrings")
        {
        }

        public DbSet<Login> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public virtual ICollection<ChartOfAccount> ChartOfAccounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>()
                .HasKey(p => p.ID);

            modelBuilder.Entity<Login>()
                .Property(p => p.USER_NAME)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Login>()
                .Property(p => p.EMAIL)
                .HasMaxLength(150);

            modelBuilder.Entity<Account>()
                .HasKey(a => a.ACC_ID);

            modelBuilder.Entity<Account>()
                .Property(a => a.ACCOUNT_NO)
                .IsRequired();

            // Defining the foreign key relationship between ChartOfAccount and Account
            modelBuilder.Entity<ChartOfAccount>()
                .HasKey(c => c.COA_ID);

        }
    }
}