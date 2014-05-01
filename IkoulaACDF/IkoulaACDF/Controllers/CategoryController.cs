using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IkoulaACDF.Models;

namespace IkoulaACDF.Controllers
{
    public class CategoryController : Controller
    {
        private AcdfEntities db = new AcdfEntities();

        // GET: /Category/
        public ActionResult Index()
        {
            return View(db.AcdfCategories.ToList());
        }

        // GET: /Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfCategory acdfcategory = db.AcdfCategories.Find(id);
            if (acdfcategory == null)
            {
                return HttpNotFound();
            }
            return View(acdfcategory);
        }

        // GET: /Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CategoryId,CategoryName")] AcdfCategory acdfcategory)
        {
            if (ModelState.IsValid)
            {
                Helpers.Utils.MakeCategoryFolder(acdfcategory.CategoryName,this);
                db.AcdfCategories.Add(acdfcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(acdfcategory);
        }

        // GET: /Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfCategory acdfcategory = db.AcdfCategories.Find(id);
            if (acdfcategory == null)
            {
                return HttpNotFound();
            }
            return View(acdfcategory);
        }

        // POST: /Category/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CategoryId,CategoryName")] AcdfCategory acdfcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acdfcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(acdfcategory);
        }

        // GET: /Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfCategory acdfcategory = db.AcdfCategories.Find(id);
            if (acdfcategory == null)
            {
                return HttpNotFound();
            }
            return View(acdfcategory);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AcdfCategory acdfcategory = db.AcdfCategories.Find(id);
            db.AcdfCategories.Remove(acdfcategory);
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
