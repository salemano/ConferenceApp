using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Model.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Model
{
    // put InitializeDatabaseConnection here

    //public class MyDatabaseInitialize : DropCreateDatabaseAlways<ConferenceContext>
    //{
    //    protected override void Seed(ConferenceContext context)
    //    {
    //        SeedMembership();
    //    }

    //    private void SeedMembership()
    //    {
    //        WebSecurity.InitializeDatabaseConnection("ConferenceContextConnection", "User",
    //            "UserId", "FirstName", autoCreateTables: true);
    //    }
    //}

    public class ConferenceContext : DbContext
    {
        static ConferenceContext()
        {
            Database.SetInitializer<ConferenceContext>(null);
        }

        public ConferenceContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }

        //added next ->

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }
}
