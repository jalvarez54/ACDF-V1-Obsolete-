using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IkoulaACDF.Models;
using PagedList;
using IkoulaACDF.Helpers;
using Microsoft.AspNet.Identity;

namespace IkoulaACDF.Controllers
{
    public class PhotoController : Controller
    {
        private AcdfEntities db = new AcdfEntities();

        // GET: /Photo/
        public ActionResult Index(int? page)
        {
            var photos =
                from p in db.AcdfPhotoes
                join c in db.AcdfCategories on p.CategoryId equals c.CategoryId
                join s in db.AcdfSubCategories on p.SubCategoryId equals s.SubCategoryId
                orderby p.CategoryId
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
                };
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Count = photos.Count();

            return View(photos.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RecentPhoto()
        {
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
                 SubCategoryId = p.SubCategoryId,
                 SubCategoryName = s.SubCategoryName,
                 Description = p.Description,
                 Place = p.Place,
                 UserName = p.UserName,
                 SchoolPeriod = p.SchoolPeriod,
                 Date = p.Date
             }).Take(5);

            return View(photos.ToList());

        }
        public ActionResult Category()
        {
            var model = from c in db.AcdfCategories
                        orderby c.CategoryName
                        select new GetCategoryPhotoViewModel
                        {
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName,
                        };
            return View(model.ToList());
        }
        public ActionResult SubCategory(int categoryID)
        {
            int noSubCategory = db.AcdfSubCategories.Count(s => s.CategoryId == categoryID);
            if(noSubCategory == 0)
            {
                // * BUG: Exception in http://www.jow-alva.net/ACDF/Photo/GetPhotos?categoryID=16&subCategoryID=50 no element
                if (db.AcdfCategories.Count(s => s.CategoryId == categoryID) == 0)
                {
                    return RedirectToAction("Category", "Photo", null);
                }
                return RedirectToAction("GetPhotos", new { categoryId = categoryID, subCategoryId = 33});
            }

            var model = from s in db.AcdfSubCategories.Where(c => c.CategoryId == categoryID)
                        orderby s.SubCategoryName ascending
                        select new GetSubCategoryPhotoViewModel
                        {
                            CategoryId = s.CategoryId,
                            SubCategoryName = s.SubCategoryName,
                            SubCategoryId = s.SubCategoryId,
                        };

            return View(model.ToList());
        }

        public ActionResult GetPhotos(int categoryId, int subCategoryId, string periode)
        {
            var per =
                (from p in db.AcdfPhotoes.Where(p => p.CategoryId == categoryId && p.SubCategoryId == subCategoryId).OrderBy(p => p.CategoryId)
                join c in db.AcdfCategories on p.CategoryId equals c.CategoryId
                join s in db.AcdfSubCategories on p.SubCategoryId equals s.SubCategoryId
                orderby p.SchoolPeriod
                select (new { period = p.SchoolPeriod })).Distinct().OrderBy(a => a.period).ToList();

            List<string> l = new List<string>();

            foreach(var p in per)
            {
                l.Add(p.period);
            }
            ViewBag.Periodes = l;

            if (periode == null)
            {
                var photos =
                    from p in db.AcdfPhotoes.Where(p => p.CategoryId == categoryId && p.SubCategoryId == subCategoryId).OrderBy(p => p.CategoryId)
                    join c in db.AcdfCategories on p.CategoryId equals c.CategoryId
                    join s in db.AcdfSubCategories on p.SubCategoryId equals s.SubCategoryId
                    orderby p.SchoolPeriod
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
                        Date = p.Date,
                        Periodes = l,
                    };
                        ViewBag.CatId = categoryId;
                        ViewBag.SubCatId = subCategoryId;
                        ViewBag.Count = photos.Count();
                        // * BUG: Exception in http://www.jow-alva.net/ACDF/Photo/GetPhotos?categoryID=16&subCategoryID=50 no element
                        if (ViewBag.Count == 0)
                        {
                            return RedirectToAction("Category", "Photo", null);
                        }

                        return View(photos.ToList());

            }
            else
            {
                var photos =
                    from p in db.AcdfPhotoes.Where(p => p.CategoryId == categoryId && p.SubCategoryId == subCategoryId && p.SchoolPeriod == periode).OrderBy(p => p.CategoryId)
                    join c in db.AcdfCategories on p.CategoryId equals c.CategoryId
                    join s in db.AcdfSubCategories on p.SubCategoryId equals s.SubCategoryId
                    orderby p.SchoolPeriod
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
                        Date = p.Date,
                        Periodes = l,
                    };
                ViewBag.CatId = categoryId;
                ViewBag.SubCatId = subCategoryId;
                ViewBag.Count = photos.Count();
                // * BUG: Exception in http://www.jow-alva.net/ACDF/Photo/GetPhotos?categoryID=16&subCategoryID=50 no element
                if (ViewBag.Count == 0)
                {
                    return RedirectToAction("Category", "Photo", null);
                }

                return View(photos.ToList());

            }
        }

        // GET: /Photo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfPhoto acdfPhoto = db.AcdfPhotoes.Find(id);
            if (acdfPhoto == null)
            {
                return HttpNotFound();
            }
            PhotoViewModel model = new PhotoViewModel()
            {
                PhotoId = acdfPhoto.PhotoId,
                Path = acdfPhoto.Path,
                CategoryName = db.AcdfCategories.Find(acdfPhoto.CategoryId).CategoryName,
                Description = acdfPhoto.Description,
                Place = acdfPhoto.Place,
                UserName = acdfPhoto.UserName,
                SubCategoryName = db.AcdfSubCategories.Find(acdfPhoto.SubCategoryId).SubCategoryName,
                SchoolPeriod = acdfPhoto.SchoolPeriod,
                Date = acdfPhoto.Date
            };
            return View(model);
        }

        // GET: /Photo/Create
        [Authorize(Roles = "CanEdit")]
        public ActionResult Create(int catId, int subCatId, CreatePhotoMessageID? message = null)
        {
            ViewBag.StatusMessage =
                message == CreatePhotoMessageID.CreateSuccess ? "Vos informations ont été prises en compte."
                : message == CreatePhotoMessageID.Error ? "Une erreur s'est produite."
                : "";

            //http://stackoverflow.com/questions/4729440/validate-a-dropdownlist-in-asp-net-mvc
            var model = new PhotoCreateViewModel
            {
                Categories = db.AcdfCategories.OrderBy(s => s.CategoryName).Where(s => s.CategoryId == catId),
                SubCategories = db.AcdfSubCategories.OrderBy(s => s.SubCategoryName).Where(s => s.SubCategoryId == subCatId),
                CategoryId = catId,
                SubCategoryId = subCatId,
                CategoryName = db.AcdfCategories.Find(catId).CategoryName,
                SubCategoryName = db.AcdfSubCategories.Find(subCatId).SubCategoryName,
            };

            return View(model);
        }

        // POST: /Photo/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CanEdit")]
        public ActionResult Create(PhotoCreateViewModel model)
        {
            AcdfPhoto acdfPhoto = null;

            if (ModelState.IsValid)
            {
                acdfPhoto = new AcdfPhoto()
                {
                    CategoryId = model.CategoryId,
                    SubCategoryId = model.SubCategoryId,
                    Description = model.Description,
                    Place = model.Place,
                    UserName = Helpers.Utils.GetUserName(),
                    SchoolPeriod = model.SchoolPeriod,
                    Date = DateTime.Now,
                    UserId = User.Identity.GetUserId(),
                };
                // Retreive CategoryName and SubCategoryName for path calculation
                var categoryName = db.AcdfCategories.Find(model.CategoryId).CategoryName;
                var subCategoryName = db.AcdfSubCategories.Find(model.SubCategoryId).SubCategoryName;

                // Add Path to acdfPhoto and save
                try
                {
                    System.Web.Helpers.WebImage webImage = System.Web.Helpers.WebImage.GetImageFromRequest();

                    string[] paths = Utils.SaveAcdfPhotoFileToDisk(webImage, this, categoryName, subCategoryName);
                    acdfPhoto.Path = paths[0];
                    acdfPhoto.ThumbPath = paths[1];
                    db.AcdfPhotoes.Add(acdfPhoto);
                    db.SaveChanges();
                    return RedirectToAction("Create", new { catId= model.CategoryId, subCatId = model.SubCategoryId, Message = CreatePhotoMessageID.CreateSuccess });
                }
                catch (Exception)
                {
                    model.Categories = db.AcdfCategories.OrderBy(s => s.CategoryName).Where(s => s.CategoryId == model.CategoryId);
                    model.SubCategories = db.AcdfSubCategories.OrderBy(s => s.SubCategoryName).Where(s => s.SubCategoryId == model.SubCategoryId);
                    return RedirectToAction("Create", new { catId = model.CategoryId, subCatId = model.SubCategoryId, Message = CreatePhotoMessageID.CreateSuccess });
                }

            }
            //// Generate Periodes
            List<System.Web.Mvc.SelectListItem> periode = new List<System.Web.Mvc.SelectListItem>();
            periode.Add(new System.Web.Mvc.SelectListItem { Value = "0000-0000", Text = "Inconnue" });

            for (int y = 1900; y <= DateTime.Now.Year - 1; y++)
            {
                periode.Add(new System.Web.Mvc.SelectListItem { Value = y.ToString() + "-" + (y + 1).ToString(), Text = y.ToString() + "-" + (y + 1).ToString() });
            };
            model.Periode = (IEnumerable<System.Web.Mvc.SelectListItem>)periode;

            model.Categories = db.AcdfCategories.OrderBy(s => s.CategoryName).Where(s => s.CategoryId == model.CategoryId);
            model.SubCategories = db.AcdfSubCategories.OrderBy(s => s.SubCategoryName).Where(s => s.SubCategoryId == model.SubCategoryId);
            return View(model);
        }

        // GET: /Photo/Edit/5
        [Authorize(Roles = "CanEdit")]
        public ActionResult Edit(int? id , EditPhotoMessageID? message = null)
        {
                ViewBag.StatusMessage =
                    message == EditPhotoMessageID.ModifSuccess ? "Vos modifications ont été prises en compte."
                    : message == EditPhotoMessageID.Error ? "Une erreur s'est produite."
                    : "";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoEditViewModel model = null;
            AcdfPhoto acdfPhoto = db.AcdfPhotoes.Find(id);
            if (acdfPhoto == null)
            {
                return HttpNotFound();
            }

            model = new PhotoEditViewModel()
            {
                PhotoId = acdfPhoto.PhotoId,
                CategoryId = acdfPhoto.CategoryId,
                SubCategoryId = acdfPhoto.SubCategoryId,
                Path = acdfPhoto.Path,
                ThumbPath = acdfPhoto.ThumbPath,
                CategoryName = db.AcdfCategories.Find(acdfPhoto.CategoryId).CategoryName,
                Description = acdfPhoto.Description,
                Place = acdfPhoto.Place,
                UserName = Helpers.Utils.GetUserName(),
                SubCategoryName = db.AcdfSubCategories.Find(acdfPhoto.SubCategoryId).SubCategoryName,
                SchoolPeriod = acdfPhoto.SchoolPeriod,
                Date = DateTime.Now,
                UserId = User.Identity.GetUserId(),
            };
            //// Generate Periodes
            List<System.Web.Mvc.SelectListItem> periode = new List<System.Web.Mvc.SelectListItem>();
            periode.Add(new System.Web.Mvc.SelectListItem { Value = "0000-0000", Text = "Inconnue" });

            for (int y = 1900; y <= DateTime.Now.Year - 1; y++)
            {
                periode.Add(new System.Web.Mvc.SelectListItem { Value = y.ToString() + "-" + (y + 1).ToString(), Text = y.ToString() + "-" + (y + 1).ToString() });
            };
            model.Periode = (IEnumerable<System.Web.Mvc.SelectListItem>)periode;

            model.Categories = db.AcdfCategories.OrderBy(s => s.CategoryName);
            model.SubCategories = db.AcdfSubCategories.OrderBy(s => s.SubCategoryName).Where(s => s.IsEnable==true);

            return View(model);
        }

        // POST: /Photo/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CanEdit")]
        public ActionResult Edit(PhotoEditViewModel model)
        {
            AcdfPhoto acdfPhoto = null;

            if (ModelState.IsValid)
            {
                acdfPhoto = new AcdfPhoto()
                {
                    Path = model.Path,
                    ThumbPath = model.ThumbPath,
                    PhotoId = model.PhotoId,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    Place = model.Place,
                    UserName = model.UserName,
                    SubCategoryId = model.SubCategoryId,
                    SchoolPeriod = model.SchoolPeriod,
                    Date = model.Date,
                    UserId = User.Identity.GetUserId(),
                };
                // Retreive CategoryName and SubCategoryName for path calculation
                var categoryName = db.AcdfCategories.Find(model.CategoryId).CategoryName;
                var subCategoryName = db.AcdfSubCategories.Find(model.SubCategoryId).SubCategoryName;

                db.Entry(acdfPhoto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { Message = EditPhotoMessageID.ModifSuccess });
            }
            model.Categories = db.AcdfCategories.OrderBy(s => s.CategoryName);
            model.SubCategories = db.AcdfSubCategories.OrderBy(s => s.SubCategoryName).Where(s => s.IsEnable == true);
            return View(model);
        }

        // GET: /Photo/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcdfPhoto acdfPhoto = db.AcdfPhotoes.Find(id);
            if (acdfPhoto == null)
            {
                return HttpNotFound();
            }
            PhotoViewModel model = new PhotoViewModel()
            {
                PhotoId = acdfPhoto.PhotoId,
                Path = acdfPhoto.Path,
                CategoryName = db.AcdfCategories.Find(acdfPhoto.CategoryId).CategoryName,
                Description = acdfPhoto.Description,
                Place = acdfPhoto.Place,
                UserName = acdfPhoto.UserName,
                SubCategoryName = db.AcdfSubCategories.Find(acdfPhoto.SubCategoryId).SubCategoryName,
                SchoolPeriod = acdfPhoto.SchoolPeriod,
                Date = acdfPhoto.Date
            };
            return View(model);
        }

        // POST: /Photo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            AcdfPhoto acdfphoto = db.AcdfPhotoes.Find(id);
            db.AcdfPhotoes.Remove(acdfphoto);
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

    #region MyApplications auxiliaires
        public enum CreatePhotoMessageID
        {
            CreateSuccess,
            Error,
        }
        public enum EditPhotoMessageID
        {
            ModifSuccess,
            Error,
        }
        public String ThumbUtil()
        {
            string result = "Process terminated";
            var photos =
            from p in db.AcdfPhotoes
            join c in db.AcdfCategories on p.CategoryId equals c.CategoryId
            join s in db.AcdfSubCategories on p.SubCategoryId equals s.SubCategoryId
            orderby p.CategoryId
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
            };
            foreach (PhotoViewModel photo in photos)
            {
                Helpers.Utils.MyThumbUtil(photo, this);
            }
            return result;
        }
    #endregion
    }

}
