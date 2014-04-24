using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IkoulaACDF.Models
{
    public class GetCategoryPhotoViewModel
    {
        public Nullable<int> CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        public IEnumerable<Models.AcdfCategory> Categories { get; set; }
    }
    public class GetSubCategoryPhotoViewModel
    {
        [Required]
        public Nullable<int> SubCategoryId { get; set; }
        [Display(Name = "Subcategory")]
        public string SubCategoryName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public IEnumerable<Models.AcdfSubCategory> SubCategories { get; set; }
    }
    public class PhotoViewModel
    {
        public string UserId { get; set; }
        public int PhotoId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public IEnumerable<Models.AcdfCategory> Categories { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public IEnumerable<Models.AcdfSubCategory> SubCategories { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Classe/Place")]
        public string Place { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Display(Name = "Subcategory")]
        public string SubCategoryName { get; set; }
        [Display(Name = "Period")]
        public string SchoolPeriod { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString="{0:d}" , HtmlEncode=false)]
        [Display(Name = "Save date")]
        public Nullable<System.DateTime> Date { get; set; }
        [Display(Name = "Photo")]
        public string Path { get; set; }
        public string ThumbPath { get; set; }
        public IEnumerable<string> Periodes { get; set; }

    }
    public class PhotoEditViewModel
    {
        public PhotoEditViewModel()
        {
            //// Generate Periodes
            List<System.Web.Mvc.SelectListItem> periode = new List<System.Web.Mvc.SelectListItem>();
            periode.Add(new System.Web.Mvc.SelectListItem { Value = "0000-0000", Text = "Inconnue" });

            for (int y = 1900; y <= DateTime.Now.Year - 1; y++)
            {
                periode.Add(new System.Web.Mvc.SelectListItem { Value = y.ToString() + "-" + (y + 1).ToString(), Text = y.ToString() + "-" + (y + 1).ToString() });
            };
            this.Periode = (IEnumerable<System.Web.Mvc.SelectListItem>)periode;
        }
        public string UserId { get; set; }
        public int PhotoId { get; set; }
        [Required]
        public Nullable<int> CategoryId { get; set; }
        public IEnumerable<Models.AcdfCategory> Categories { get; set; }
        [Required]
        public Nullable<int> SubCategoryId { get; set; }
        public IEnumerable<Models.AcdfSubCategory> SubCategories { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Classe/Place")]
        public string Place { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Display(Name = "Subcategory")]
        public string SubCategoryName { get; set; }
        [Required]
        [Display(Name = "Period")]
        public string SchoolPeriod { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name = "Save date")]
        public Nullable<System.DateTime> Date { get; set; }
        [Display(Name = "Photo")]
        public string Path { get; set; }
        public string ThumbPath { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Periode { get; set; }
    }

    public class PhotoCreateViewModel
    {
        public PhotoCreateViewModel()
        {
            //// Generate Periodes
            List<System.Web.Mvc.SelectListItem> periode = new List<System.Web.Mvc.SelectListItem>();
            periode.Add(new System.Web.Mvc.SelectListItem { Value = "0000-0000", Text = "Inconnue" });

            for (int y = 1900; y <= DateTime.Now.Year - 1; y++)
            {
                periode.Add(new System.Web.Mvc.SelectListItem { Value = y.ToString() + "-" + (y + 1).ToString(), Text = y.ToString() + "-" + (y + 1).ToString() });
            };
            this.Periode = (IEnumerable<System.Web.Mvc.SelectListItem>)periode;
        }

        public string UserId { get; set; }
        public int PhotoId { get; set; }
        [Required]
        public Nullable<int> CategoryId { get; set; }
        public IEnumerable<Models.AcdfCategory> Categories { get; set; }
        [Required]
        public Nullable<int> SubCategoryId { get; set; }
        public IEnumerable<Models.AcdfSubCategory> SubCategories { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Classe/Place")]
        public string Place { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Display(Name = "Subcategory")]
        public string SubCategoryName { get; set; }
        [Required]
        [Display(Name = "Period")]
        public string SchoolPeriod { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name = "Save date")]
        public Nullable<System.DateTime> Date { get; set; }
        [Display(Name = "Photo")]
        public string Path { get; set; }
        public string ThumbPath { get; set; }
        [Required]
        public HttpPostedFileWrapper Photo { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Periode { get; set; }

    }

}