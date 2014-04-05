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

        public ActionResult Index()
        {
            // BirthDays with ViewBag
            //ViewBag.Birthdays = from b in db.AspNetUsers
            //                    where b.BirthDate.Value.Day <= DateTime.Now.Day && b.BirthDate.Value.Month == DateTime.Now.Month
            //                    orderby b.BirthDate.Value.Day descending
            //                    select b;

            // BirthDays with model
            var birthdays = from b in db.AspNetUsers
                                where b.BirthDate.Value.Day <= DateTime.Now.Day && b.BirthDate.Value.Month == DateTime.Now.Month
                                orderby b.BirthDate.Value.Day descending
                                select b;
            ViewBag.CountBirthDays = birthdays.Count();

            var aspnetUsers = (from u in db.AspNetUsers
                             orderby u.RegistrationDate descending
                             select u).Take(5);
            ViewBag.CountUsers = aspnetUsers.Count();


            var guessBooks = (from gb in db.AcdfGuessBooks
                             orderby gb.Date descending
                             select gb).Take(5);
            ViewBag.CountGuessBooks = guessBooks.Count();


            //var baselineDate = DateTime.Now.AddDays(-7);
            var photos =
            //from p in db.AcdfPhotoes.Where(p => p.Date >= baselineDate)
            (from p in db.AcdfPhotoes
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
            }).Take(5);
            ViewBag.CountPhotos = photos.Count();

            // BirthDays with ViewBag
            //var model = new HomePageViewModel()
            //{
            //    HomePagePhotoViewModel = photos,
            //    HomePageGuessBookViewModel = guessBooks,
            //    HomePageAspNetUsersViewModel = aspnetUsers,
            //};

            // BirthDays with model
            var model = new HomePageViewModel(photos, guessBooks, aspnetUsers, birthdays);

            return View(model);
        }
        public ActionResult ViewPartials()
        {
            var birthdays = from b in db.AspNetUsers
                            where b.BirthDate.Value.Day <= DateTime.Now.Day && b.BirthDate.Value.Month == DateTime.Now.Month
                            orderby b.BirthDate.Value.Day descending
                            select b;

            var aspnetUsers = (from u in db.AspNetUsers
                               orderby u.RegistrationDate descending
                               select u).Take(5);

            var guessBooks = (from gb in db.AcdfGuessBooks
                              orderby gb.Date descending
                              select gb).Take(5);

            //var baselineDate = DateTime.Now.AddDays(-7);
            var photos =
                //from p in db.AcdfPhotoes.Where(p => p.Date >= baselineDate)
            (from p in db.AcdfPhotoes
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
             }).Take(5);

            // BirthDays with ViewBag
            //var model = new HomePageViewModel()
            //{
            //    HomePagePhotoViewModel = photos,
            //    HomePageGuessBookViewModel = guessBooks,
            //    HomePageAspNetUsersViewModel = aspnetUsers,
            //};

            // BirthDays with model
            var model = new HomePageViewModel(photos, guessBooks, aspnetUsers, birthdays);

            return View(model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult View1()
        {
            ViewBag.Message = "For testing.";

            return View();
        }


    }
}