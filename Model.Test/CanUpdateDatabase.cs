using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Migrations;
using Model.Migrations;

namespace Model.Test
{
    [TestClass]
    public class CanUpdateDatabase
    {
        [TestMethod]
        [TestCategory("Slow")]
        public void CreateFromScratch()
        {
            var connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=Session_" + Guid.NewGuid().ToString() + ";Trusted_Connection=True;";
            try
            {
                var dbConnection = new DbConnectionInfo(connectionString, "System.Data.SqlClient");
                var settings = new Model.Migrations.Configuration();
                settings.TargetDatabase = dbConnection;
                var migrator = new DbMigrator(settings);

                migrator.Update(targetMigration: "201307170913098_AddedUserIsDeletedField");
            }
            finally
            {
                var context = new ConferenceContext(connectionString);
                context.Database.Delete();
            }
        }
    }
}
