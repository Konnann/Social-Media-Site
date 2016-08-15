using System.Web.UI.WebControls;

namespace ATHNN.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ATHNN.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ATHNN.Models.ApplicationDbContext";
        }

        protected override void Seed(ATHNN.Models.ApplicationDbContext context)
        {
           //Ako bazata danni e prazna, tuk pishem nqkakvi gotovi kato runnvame da si izlizat.
            if (!context.Users.Any())
            {
               
            }
        }
    }
}
