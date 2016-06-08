﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Household_Budgeter.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Household_Budgeter.Controllers
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BankAccounts
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var account = db.BankAccounts.Where(u => user.HouseholdId == u.HouseholdId).ToList();

            Household household = db.Households.Find(user.HouseholdId);
            if (household == null)
            {
                return RedirectToAction("Create", "Households");
            }

            return View(account);
        }

        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public PartialViewResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return PartialView();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Name,Created,Balance,InitialBalance,ReconcileBalance")] BankAccount bankAccount)
        {
            bankAccount.Created = new DateTimeOffset(DateTime.Now);

            if (ModelState.IsValid)
            {
                bankAccount.HouseholdId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId.Value;
                bankAccount.Created = new DateTimeOffset(DateTime.Now);

                bankAccount.ReconcileBalance = bankAccount.Balance;

                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(b => b.Id == id);
            Household household = db.Households.FirstOrDefault(h => h.Id == bankAccount.HouseholdId);

            //if (!household.Members.Contains(user))
            //{
            //    return RedirectToAction("Unauthorized", "Error");
            //}

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (bankAccount == null)
            {
                return HttpNotFound();
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name,Created,Balance,InitialBalance,ReconcileBalance")] BankAccount bankAccount)
        {
            bankAccount.Created = new DateTimeOffset(DateTime.Now);

            if (ModelState.IsValid)
            {
                bankAccount.HouseholdId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId.Value;
                bankAccount.Created = new DateTimeOffset(DateTime.Now);

                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            return View(bankAccount);
        }

        public ActionResult Update(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(b => b.Id == id);
            Household household = db.Households.FirstOrDefault(h => h.Id == bankAccount.HouseholdId);

            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "Id,HouseholdId,Name,Created,Balance,InitialBalance,ReconcileBalance")] BankAccount bankAccount)
        {
            bankAccount.Created = new DateTimeOffset(DateTime.Now);

            if (ModelState.IsValid)
            {
                bankAccount.HouseholdId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId.Value;
                bankAccount.Created = new DateTimeOffset(DateTime.Now);

                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(b => b.Id == id);
            Household household = db.Households.FirstOrDefault(h => h.Id == bankAccount.HouseholdId);

            //if (!household.Members.Contains(user))
            //{
            //    return RedirectToAction("Unauthorized", "Error");
            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(x => x.Id == id);
            Household household = db.Households.FirstOrDefault(x => x.Id == bankAccount.HouseholdId);

            //if (!household.Members.Contains(user))
            //{
            //    return RedirectToAction("Unauthorized", "Error");
            //}

            db.BankAccounts.Remove(bankAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetBal(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BankAccount bankAccount = db.BankAccounts.FirstOrDefault(x => x.Id == id);
            Household household = db.Households.FirstOrDefault(x => x.Id == bankAccount.HouseholdId);

            //Gets all bank account ids for household which contains the incoming 'id'
            if (household.Id == id)
            {
                var account = db.BankAccounts.Find(id);
                //generic object in c#
                return Json(new { bal = account.Balance, recBal = account.ReconcileBalance }, JsonRequestBehavior.AllowGet);
            }

            return null;
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
