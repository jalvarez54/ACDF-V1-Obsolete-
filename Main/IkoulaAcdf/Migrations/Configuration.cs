namespace IkoulaACDF.Migrations
{
    using IkoulaACDF.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IkoulaACDF.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(IkoulaACDF.Models.ApplicationDbContext context)
        {
            this.AddUserAndRoles();
        }
        bool AddUserAndRoles()
        {
            bool success = false;
            var idManager = new IdentityManager();
            success = idManager.CreateRole("Admin");
            if (!success == true) return success;
            success = idManager.CreateRole("CanEdit");
            if (!success == true) return success;
            success = idManager.CreateRole("User");
            if (!success) return success;
            DateTime naissance;
            DateTime.TryParse("07/02/1955", out naissance);
            var newUser = new ApplicationUser()
            {
                UserName = "admin",
                FirstName = "José",
                LastName = "Alvarez",
                Email = "jose.alvarez54@free.fr",
                BirthDate = naissance,
                FirstYearSchool = 1968,
                LastYearSchool = 1975,
                LastClass = "Terminale",
                ActualCity = "Nancy",
                ActualCountry = "France",
            };
            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            success = idManager.CreateUser(newUser, "p@ssword");
            if (!success) return success;
            success = idManager.AddUserToRole(newUser.Id, "Admin");
            if (!success) return success;
            success = idManager.AddUserToRole(newUser.Id, "CanEdit");
            if (!success) return success;
            success = idManager.AddUserToRole(newUser.Id, "User");
            if (!success) return success;
            return success;
        }
    }
}

