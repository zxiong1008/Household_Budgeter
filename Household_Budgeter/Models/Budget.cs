using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Household_Budgeter.Models
{
    public class Household
    {
        public Household()
        {
            this.Members = new HashSet<ApplicationUser>();
            this.BankAccounts = new HashSet<BankAccount>();
            this.Budgets = new HashSet<Budget>();
            this.Categories = new HashSet<Category>();
            this.Invitations = new HashSet<Invitation>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
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

        public virtual ICollection<Transaction> Transactions { get; set; }

    }

    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }

        [Required]
        //debit/credit
        public bool TransactionTypeId { get; set; }

        public decimal Amount { get; set; }
        public decimal? ReconciledAmount { get; set; }
        public int CategoryId { get; set; }
        public bool Reconciled { get; set; }
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Category Categories { get; set; }
        public virtual TransactionType TransactionTypes { get; set; }
    }

    public class TransactionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Budget
    {
        public Budget()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseHoldId { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
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

    public class DefaultCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
}