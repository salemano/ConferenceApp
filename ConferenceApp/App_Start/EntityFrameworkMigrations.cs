[assembly: WebActivator.PostApplicationStartMethod(typeof(ConferenceApp.App_Start.EntityFrameworkMigrations), "PostStart")]

namespace ConferenceApp.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.Migrations;
    using Model.Migrations;

    public static class EntityFrameworkMigrations
    {
        public static void PostStart()
        {
            DbMigrator dbMigrator = new DbMigrator(new Configuration());
            dbMigrator.Update();
        }
    }
}
