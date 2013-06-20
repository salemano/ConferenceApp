using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Model.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Model
{
    public class ConferenceContext : DbContext
    {
        static ConferenceContext()
        {
            Database.SetInitializer<ConferenceContext>(null);
        }

        public ConferenceContext() { }

        public ConferenceContext(string connectionString)
            : base(connectionString)
        {
        }

        public IDbSet<User> Users { get; set; }
    }
}
