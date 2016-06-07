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
    [Authorize]
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Budgets
        public ActionResult Index(int? id)
        {
            //find user in the household
            var user = db.Users.Find(User.Identity.GetUserId());

            Household household = db.Households.Find(user.HouseholdId);


            var budgets = db.Budgets.Where(u => u.HouseHoldId == user.HouseholdId).Include(b => b.Household);
            
            //if user does not have household, create one
            if (household == null)
            {
                return RedirectToAction("Create", "households");
            }

            return View(budgets);
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public PartialViewResult Create()
        {
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name");
            return PartialView();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,HouseHoldId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index", "BudgetItems");
            }

            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", budget.HouseHoldId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            //heirarchy where we find the user
            var user = db.Users.Find(User.Identity.GetUserId());

            //then the user's budget
            Budget budget = db.Budgets.FirstOrDefault(b => b.Id == id);

            //then the budget's household owner
            Household household = db.Households.FirstOrDefault(h => h.Id == budget.Id);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", budget.HouseHoldId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HouseHoldId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", budget.HouseHoldId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            //heirarchy where we find the user
            var user = db.Users.Find(User.Identity.GetUserId());

            //then the user's budget
            Budget budget = db.Budgets.FirstOrDefault(b => b.Id == id);

            //then the budget's household owner
            Household household = db.Households.FirstOrDefault(h => h.Id == budget.Id);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //heirarchy where we find the user
            var user = db.Users.Find(User.Identity.GetUserId());

            //then the user's budget
            Budget budget = db.Budgets.FirstOrDefault(b => b.Id == id);

            //then the budget's household owner
            Household household = db.Households.FirstOrDefault(h => h.Id == budget.Id);

            if (!household.Members.Contains(user))
            {
                return RedirectToAction("Unauthorized", "Error");
            }

            db.Budgets.Remove(budget);
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
