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
    public class HomeController : Controller
    {
        private AcdfEntities db = new AcdfEntities();

        public ActionResult Archives()
        {
            ViewBag.Message = "Divers archives.";

            return View();

        }
        public ActionResult Anecdotes()
        {
            ViewBag.Message = "Vos anecdotes.";

            return View();

        }
        public ActionResult Tramp()
        {
            string trampsPath = Server.MapPath("~/Medias/LogosTramp");
            var DI = new System.IO.DirectoryInfo(trampsPath);
            System.IO.FileInfo[] fiArr = DI.GetFiles();
            var model = from f in fiArr
                        orderby f.Name
                        select new TrampViewModel()
                        {
                            FilePath = System.IO.Path.Combine(HttpRuntime.AppDomainAppVirtualPath, @"Medias/LogosTramp", f.Name),
                            MetaData = f.Name.Split('_', '.')[1]
                        };
           return View(model);
        }
        public ActionResult Index()
        {
            var aspnetUsers = (from u in db.AspNetUsers
                               where u.ConfirmedEmail == true
                               //where u.RegistrationDate.Value.Day <= DateTime.Now.Day && u.RegistrationDate.Value.Month == DateTime.Now.Month
                               orderby u.RegistrationDate descending
                               select u);
            this.HttpContext.ApplicationInstance.Application.Add("CountAspNetUsers", aspnetUsers.Count());

            var guessBooks = (from gb in db.AcdfGuessBooks
                                   orderby gb.Date descending
                                   select gb);
            this.HttpContext.ApplicationInstance.Application.Add("CountAcdfGuessBooks", guessBooks.Count());

            var photos =
                //from p in db.AcdfPhotoes.Where(p => p.Date >= baselineDate)
            (from p in db.AcdfPhotoes
             //where p.Date.Value.Day <= DateTime.Now.Day && p.Date.Value.Month == DateTime.Now.Month
             join c in db.AcdfCategories on p.CategoryId equals c.CategoryId
             join s in db.AcdfSubCategories on p.SubCategoryId equals s.SubCategoryId
             orderby p.Date descending
             select new PhotoViewModel
             {
                 PhotoId = p.PhotoId,
                 Path = p.Path,
                 ThumbPath = p.ThumbPath,
                 CategoryName = c.CategoryName,
                 SubCategoryName = s.SubCategoryName,
                 Description = p.Description,
                 Place = p.Place,
                 UserName = p.UserName,
                 SchoolPeriod = p.SchoolPeriod,
                 Date = p.Date
             });

            this.HttpContext.ApplicationInstance.Application.Add("CountAcdfPhotoes", photos.Count());

            var model = new HomePageViewModel(photos, guessBooks, aspnetUsers);

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Page de description de l'application.";

            return View();
        }
        public ActionResult Todo()
        {
            ViewBag.Message = "En construction.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Page contacts.";

            return View();
        }


    }
}