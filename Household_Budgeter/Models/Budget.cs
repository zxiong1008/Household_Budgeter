using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Household_Budgeter.Models
{
    public class Household
    {
        public Household()
        {
            this.Members = new HashSet<ApplicationUser>();
            this.BankAccounts = new HashSet<BankAccount>();
            this.Budgets = new HashSet<Budget>();
            this.Category = new HashSet<Category>();
            this.Invitations = new HashSet<Invitation>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        //already in ACCOUNTS, dont need to ask twice
        //public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
    }

    public class BankAccount
    {
        public BankAccount()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }

        public decimal Balance { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal ReconcileBalance { get; set; }
        public decimal WarningBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

    }

    public class Transaction
    {
        public int Id { get; set; }
        public int BankAccountsId { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }

        [Required]
        //debit/credit
        public bool Types { get; set; }
        public bool Void { get; set; }

        public decimal Amount { get; set; }
        public decimal? ReconciledAmount { get; set; }
        public int CategoryId { get; set; }
        public bool Reconciled { get; set; }
        //userId
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual BankAccount BankAccounts { get; set; }
    }

    public class Budget
    {
        public Budget()
        {
            //this.Transactions = new HashSet<Transaction>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Category = new HashSet<Category>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseHoldId { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public virtual ICollection<Category> Category { get; set; }
        public virtual Household Household { get; set; }
    }

    //BudgetItem income and expenses
    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }

        public decimal Amount { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual Category Category { get; set; }
    }

    public class Category
    {
        public Category()
        {
            this.Households = new HashSet<Household>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Household> Households { get; set; }
    }

    //need an inviation object that has householdid and userid for current logged user
    //use sendgrid system, set up the message, destination,
    //guid built inside the body, build callbackurl that is going to provide a link to the destination
    //CallBackUrl = Url.Action("Joined", "Household", new { joincode = invitation.JoinCode}, protocol:Request.Url.Scheme);
    public class Invitation
    {
        public int Id { get; set; }
        public Guid JoinCode { get; set; }
        public string ToEmail { get; set; }
        public bool Joined { get; set; }

        public int HouseholdId { get; set; }
        public virtual Household Households { get; set; }

        public string userId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class SendGridCredential
    {
        [Key]//setting up a 1-1 relationship using UserId as primarykey & foreignkey
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class DemoLogin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }

    public class DashboardViewModels
    {
        public Household Households { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        public Budget Budgets { get; set; }
        public ICollection<BankAccount> BankAccounts { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Category> Categories { get; set; }
        public BudgetItem BudgetItems { get; set; }

        public int GetBudgetId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTimeOffset begin { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTimeOffset end { get; set; }


        public int setLowBalance { get; set; }


    }
}