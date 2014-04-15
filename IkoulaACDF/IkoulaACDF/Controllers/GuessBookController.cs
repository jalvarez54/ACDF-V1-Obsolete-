using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IkoulaACDF.Models;
using Microsoft.AspNet.Identity;

namespace IkoulaACDF.Controllers
{
    public class GuessBookController : Controller
    {
        private AcdfEntities db = new AcdfEntities();

        // GET: /GuessBook/
        public ActionResult Index()
        {
            var acdfguessbooks = db.AcdfGuessBooks.Include(a => a.AspNetUser).OrderByDescending(m => m.Date) ;
            return View(acdfguessbooks.ToList());
        }

        // GET: /GuessBook/Create
        [Authorize(Roles = "User")]
        public ActionResult Create()
        {

            // Cause we want to show the photo and username too
            var userManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser user = userManager.FindByName(User.Identity.Name);
            AspNetUser au = new AspNetUser()
            {
                PhotoUrl = user.PhotoUrl,
                UserName = user.UserName,
            };
            var model = new AcdfGuessBook()
            {
                AspNetUser = au,
                //http://stackoverflow.com/questions/18448637/how-to-get-current-user-and-how-to-use-user-class-in-mvc5/18451374#18451374
                UserId = User.Identity.GetUserId(),
                Date = DateTime.Now,
            };

            return View(model);
        }

        // POST: /GuessBook/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Create([Bind(Include = "Id,Message,UserId,Date")] AcdfGuessBook acdfguessbook)
        {
            if (ModelState.IsValid)
            {
                db.AcdfGuessBooks.Add(acdfguessbook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = acdfguessbook.UserId;
            return View(acdfguessbook);
        }

        // GET: /GuessBook/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfGuessBook acdfguessbook = db.AcdfGuessBooks.Find(id);
            if (acdfguessbook == null)
            {
                return HttpNotFound();
            }

            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", acdfguessbook.UserId);
            return View(acdfguessbook);
        }

        // POST: /GuessBook/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Message,UserId,Date")] AcdfGuessBook acdfguessbook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acdfguessbook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "UserName", acdfguessbook.UserId);
            return View(acdfguessbook);
        }

        // GET: /GuessBook/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfGuessBook acdfguessbook = db.AcdfGuessBooks.Find(id);
            if (acdfguessbook == null)
            {
                return HttpNotFound();
            }
            return View(acdfguessbook);
        }

        // POST: /GuessBook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AcdfGuessBook acdfguessbook = db.AcdfGuessBooks.Find(id);
            db.AcdfGuessBooks.Remove(acdfguessbook);
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
