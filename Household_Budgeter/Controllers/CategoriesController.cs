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
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Household household = db.Households.Find(user.HouseholdId);

            if (household == null)
            {
                return RedirectToAction("Create", "Households");
            }

            return View(db.Category);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            //heirarchy where we find the user
            var user = db.Users.Find(User.Identity.GetUserId());

            //then the category's category owner
            Category category = db.Category.FirstOrDefault(b => b.Id == id);

            //then the category's household owner
            Household household = db.Households.FirstOrDefault(h => h.Id == category.Id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            //heirarchy where we find the user
            var user = db.Users.Find(User.Identity.GetUserId());

            //then the category's category owner
            Category category = db.Category.FirstOrDefault(b => b.Id == id);

            //then the category's household owner
            Household household = db.Households.FirstOrDefault(h => h.Id == category.Id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5$
        public ActionResult Delete(int? id)
        {
            //heirarchy where we find the user
            var user = db.Users.Find(User.Identity.GetUserId());

            //then the category's category owner
            Category category = db.Category.FirstOrDefault(b => b.Id == id);

            //then the category's household owner
            Household household = db.Households.FirstOrDefault(h => h.Id == category.Id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //heirarchy where we find the user
            var user = db.Users.Find(User.Identity.GetUserId());

            //then the category's category owner
            Category category = db.Category.FirstOrDefault(b => b.Id == id);

            //then the category's household owner
            Household household = db.Households.FirstOrDefault(h => h.Id == category.Id);

            db.Category.Remove(category);
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
