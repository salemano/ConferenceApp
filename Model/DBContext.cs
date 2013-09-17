using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Model.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Migrations;

namespace Model
{
    public class ConferenceContext : DbContext
    {
        public ConferenceContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public ConferenceContext(string connectionstring)
            : base(connectionstring)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<UsersInSessions> UsersInSessions { get; set; }
        public DbSet<FileData> FileData { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().Property(x => x.Type).HasColumnType("int");
            base.OnModelCreating(modelBuilder);
        }
    }
}
