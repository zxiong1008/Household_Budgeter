using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Household_Budgeter.Models;
using Microsoft.AspNet.Identity;

namespace Household_Budgeter.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var transactions = db.Transactions.Where(t => t.User.HouseholdId == user.HouseholdId).Include(t => t.Categories);
            return View(transactions.ToList());
        }

        //// GET: Transactions/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(transaction);
        //}

        // GET: Transactions/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var getAccount = db.BankAccounts.Where(u => user.HouseholdId == u.HouseholdId).ToList();

            ViewBag.AccountId = new SelectList(getAccount, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Description,Date,TransactionTypeId,Amount,ReconciledAmount,CategoryId,Reconciled,UserId")] Transaction transaction)
        {
            transaction.Date = new DateTimeOffset(DateTime.Now);

            if (ModelState.IsValid)
            {
                transaction.Date = new DateTimeOffset(DateTime.Now);
                transaction.UserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

                var account = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.AccountId);

                transaction.ReconciledAmount = transaction.Amount;

                if (transaction.ReconciledAmount == transaction.Amount)
                {
                    transaction.Reconciled = true;
                }

                if (transaction.Reconciled == true)
                {
                    account.Balance += transaction.Amount;
                }
                else
                {
                    account.Balance -= transaction.Amount;
                }
                
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            Transaction transaction = db.Transactions.FirstOrDefault(t => t.Id == id);
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.AccountId);
            Household household = db.Households.FirstOrDefault(h => h.Id == bankAccount.HouseholdId);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (transaction == null)
            {
                return HttpNotFound();
            }

            var getAccount = db.BankAccounts.Where(u => user.HouseholdId == u.HouseholdId).ToList();

            ViewBag.AccountId = new SelectList(getAccount, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Description,Date,TransactionTypeId,Amount,ReconciledAmount,CategoryId,Reconciled,UserId")] Transaction transaction)
        {
            transaction.Date = new DateTimeOffset(DateTime.Now);

            if (ModelState.IsValid)
            {
                transaction.Date = new DateTimeOffset(DateTime.Now);
                transaction.UserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

                var original = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);
                var account = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.AccountId);

                if (transaction.ReconciledAmount == transaction.Amount)
                {
                    transaction.Reconciled = true;
                }
                else
                {
                    transaction.Reconciled = false;
                }

                if(transaction.Type == true)
                {
                    account.Balance -= original.Amount;
                }
                else
                {
                    account.Balance += original.Amount;
                }

                if(transaction.Type == true)
                {
                    account.Balance += transaction.Amount;
                }
                else
                {
                    account.Balance -= transaction.Amount;
                }


                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.CategoryId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            Transaction transaction = db.Transactions.FirstOrDefault(t => t.Id == id);
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.AccountId);
            Household household = db.Households.FirstOrDefault(h => h.Id == bankAccount.HouseholdId);
            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            Transaction transaction = db.Transactions.FirstOrDefault(t => t.Id == id);
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.AccountId);
            Household household = db.Households.FirstOrDefault(h => h.Id == bankAccount.HouseholdId);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
