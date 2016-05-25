namespace Household_Budgeter.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Household_Budgeter.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Household_Budgeter.Models.ApplicationDbContext context)
        {
            string[] D_Categories =
            {
                "Automobile", "Bank charges", "Childcare", "Clothing", "Credit Card Fees", "Education",
                "Events", "Food", "Flowers", "Gifts", "Household", "Healthcare", "Insurance", "Job expenses", "Leisure (daily/non-vacation)",
                "Household", "Hobbies", "Loans", "Pet Care", "Savings", "Taxes", "Utilities", "Vacation"
            };

            if (context.DefaultCategories.Count() == 0)
            {
                foreach (var c in D_Categories)
                {
                    context.DefaultCategories.Add(new DefaultCategory { Name = c });
                }
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var household = context.Households.Add(new Household { Name = "Demo Household" });
            var uStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(uStore);

            if (!userManager.Users.Any(u => u.Email == "zxiong1008@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "zxiong1008@gmail.com",
                    Email = "zxiong1008@gmail.com",
                    FirstName = "Zeng",
                    LastName = "Xiong",
                    HouseholdId = household.Id,
                    EmailConfirmed = true
                }, "Password-1");
            }
            if (!context.Users.Any(u => u.Email == "Admin@HouseholdBudget.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Admin@HouseholdBudget.com",
                    UserName = "Admin@HouseholdBudget.com",
                    FirstName = "Demo-Admin-User",
                    LastName = "Household-Budgeter-Application",
                    HouseholdId = household.Id,
                    EmailConfirmed = true
                }, "Password-1");
            }
            if (!context.Users.Any(u => u.Email == "Husband@HouseholdBudget.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Husband@HouseholdBudget.com",
                    UserName = "Husband@HouseholdBudget.com",
                    FirstName = "Demo-Husband-User",
                    LastName = "Household-Budgeter-Application",
                    HouseholdId = household.Id,
                    EmailConfirmed = true
                }, "Password-1");
            }
            if (!context.Users.Any(u => u.Email == "Wife@HouseholdBudget.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Wife@HouseholdBudget.com",
                    UserName = "Wife@HouseholdBudget.com",
                    FirstName = "Demo-Wife-User",
                    LastName = "Household-Budgeter-Application",
                    HouseholdId = household.Id,
                    EmailConfirmed = true
                }, "Password-1");
            }
            if (!context.Users.Any(u => u.Email == "Son@HouseholdBudget.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Son@HouseholdBudget.com",
                    UserName = "Son@HouseholdBudget.com",
                    FirstName = "Demo-Son-User",
                    LastName = "Household-Budgeter-Application",
                    HouseholdId = household.Id,
                    EmailConfirmed = true
                }, "Password-1");
            }
            if (!context.Users.Any(u => u.Email == "Daughter@HouseholdBudget.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Daughter@HouseholdBudget.com",
                    UserName = "Daughter@HouseholdBudget.com",
                    FirstName = "Demo-Daughter-User",
                    LastName = "Household-Budgeter-Application",
                    HouseholdId = household.Id,
                    EmailConfirmed = true
                }, "Password-1");
            }
        }
    }
}

