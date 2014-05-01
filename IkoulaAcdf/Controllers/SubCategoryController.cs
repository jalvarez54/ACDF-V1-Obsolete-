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
    public class SubCategoryController : Controller
    {
        private AcdfEntities db = new AcdfEntities();

        // GET: /SubCategory/
        public ActionResult Index()
        {
            var acdfsubcategories = db.AcdfSubCategories.Include(a => a.AcdfCategory);
            return View(acdfsubcategories.ToList());
        }

        // GET: /SubCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfSubCategory acdfsubcategory = db.AcdfSubCategories.Find(id);
            if (acdfsubcategory == null)
            {
                return HttpNotFound();
            }
            return View(acdfsubcategory);
        }

        // GET: /SubCategory/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.AcdfCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: /SubCategory/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="SubCategoryId,SubCategoryName,IsEnable,CategoryId")] AcdfSubCategory acdfsubcategory)
        {
            if (ModelState.IsValid)
            {
                string catName = db.AcdfCategories.Find(acdfsubcategory.CategoryId).CategoryName;
                Helpers.Utils.MakeSubCategoryFolder(catName, acdfsubcategory.SubCategoryName, this);
                db.AcdfSubCategories.Add(acdfsubcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.AcdfCategories, "CategoryId", "CategoryName", acdfsubcategory.CategoryId);
            return View(acdfsubcategory);
        }

        // GET: /SubCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfSubCategory acdfsubcategory = db.AcdfSubCategories.Find(id);
            if (acdfsubcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.AcdfCategories, "CategoryId", "CategoryName", acdfsubcategory.CategoryId);
            return View(acdfsubcategory);
        }

        // POST: /SubCategory/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="SubCategoryId,SubCategoryName,IsEnable,CategoryId")] AcdfSubCategory acdfsubcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acdfsubcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.AcdfCategories, "CategoryId", "CategoryName", acdfsubcategory.CategoryId);
            return View(acdfsubcategory);
        }

        // GET: /SubCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfSubCategory acdfsubcategory = db.AcdfSubCategories.Find(id);
            if (acdfsubcategory == null)
            {
                return HttpNotFound();
            }
            return View(acdfsubcategory);
        }

        // POST: /SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AcdfSubCategory acdfsubcategory = db.AcdfSubCategories.Find(id);
            db.AcdfSubCategories.Remove(acdfsubcategory);
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
