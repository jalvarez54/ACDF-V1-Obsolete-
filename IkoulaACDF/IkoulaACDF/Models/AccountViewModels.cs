using IkoulaACDF.CustomFiltersAttributes;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace IkoulaACDF.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Nom d’utilisateur")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }


    public class RestrictedUsersListViewModel
    {
        public RestrictedUsersListViewModel(ApplicationUser user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.BirthDate = user.BirthDate;
            this.LastYearSchool = user.LastYearSchool;
            this.LastClass = user.LastClass;
            this.ActualCountry = user.ActualCountry;
            this.PhotoUrl = user.PhotoUrl;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public System.DateTime BirthDate { get; set; }
        public int LastYearSchool { get; set; }
        public string LastClass { get; set; }
        public string ActualCountry { get; set; }
        public string PhotoUrl { get; set; }
        public HttpPostedFileWrapper Photo { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nom d’utilisateur")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Mémoriser le mot de passe ?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            //// Generate Years
            List<System.Web.Mvc.SelectListItem> years = new List<System.Web.Mvc.SelectListItem>();
            for (int y = 1900; y <= DateTime.Now.Year; y++)
            {
                years.Add(new System.Web.Mvc.SelectListItem { Value = y.ToString(), Text = y.ToString() });
            };
            this.YearFirst = (IEnumerable<System.Web.Mvc.SelectListItem>)years;
            this.YearLast = (IEnumerable<System.Web.Mvc.SelectListItem>)years;

        }

        public bool ConfirmedEmail { get; set; }

        [Required]
        [Display(Name = "Nom d’utilisateur")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe ")]
        [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }


        // New Fields added to extend Application User class:

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$", ErrorMessage = "Your email address is not in a valid format. Example of correct format: joe.example@example.org")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public System.DateTime BirthDate { get; set; }
        [Required]
        public int FirstYearSchool { get; set; }
        [Required]
        public int LastYearSchool { get; set; }
        [Required]
        public string LastClass { get; set; }
        [Required]
        public string ActualCity { get; set; }
        [Required]
        public string ActualCountry { get; set; }
        public string PhotoUrl { get; set; }
        [NotMapped]
        [Display(Name = "Change")]
        [ValidateFile]
        public HttpPostedFileWrapper Photo { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> YearFirst { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> YearLast { get; set; }

        // Return a pre-poulated instance of AppliationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                BirthDate = this.BirthDate,
                FirstYearSchool = this.FirstYearSchool,
                LastYearSchool = this.LastYearSchool,
                LastClass = this.LastClass,
                ActualCity = this.ActualCity,
                ActualCountry = this.ActualCountry,
                PhotoUrl = this.PhotoUrl,
                Photo = this.Photo,
                RegistrationDate = this.RegistrationDate,
            };
            return user;
        }

    }

    // Class added
    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditUserViewModel(ApplicationUser user)
        {
            this.ConfirmedEmail = user.ConfirmedEmail;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.BirthDate = user.BirthDate;
            this.FirstYearSchool = user.FirstYearSchool;
            this.LastYearSchool = user.LastYearSchool;
            this.LastClass = user.LastClass;
            this.ActualCity = user.ActualCity;
            this.ActualCountry = user.ActualCountry;
            this.PhotoUrl = user.PhotoUrl;
            //// Generate Years
            List<System.Web.Mvc.SelectListItem> years = new List<System.Web.Mvc.SelectListItem>();
            for (int y = 1900; y <= DateTime.Now.Year; y++)
            {
                years.Add(new System.Web.Mvc.SelectListItem { Value = y.ToString(), Text = y.ToString() });
            };
            this.YearFirst = (IEnumerable<System.Web.Mvc.SelectListItem>)years;
            this.YearLast = (IEnumerable<System.Web.Mvc.SelectListItem>)years;
            this.RegistrationDate = user.RegistrationDate;
        }
        [Required]
        [Display(Name = "Confirmed Email")]
        public bool ConfirmedEmail { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Your email address is not in a valid format. Example of correct format: joe.example@example.org")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public System.DateTime BirthDate { get; set; }
        [Required]
        public int FirstYearSchool { get; set; }
        [Required]
        public int LastYearSchool { get; set; }
        [Required]
        public string LastClass { get; set; }
        [Required]
        public string ActualCity { get; set; }
        [Required]
        public string ActualCountry { get; set; }
        public string PhotoUrl { get; set; }
        [NotMapped]
        [Display(Name = "Change")]
        [ValidateFile]
        public HttpPostedFileWrapper Photo { get; set; }
        [NotMapped]
        [Display(Name = "Remove if checked")]
        public bool IsNoPhotoChecked { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> YearFirst { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> YearLast { get; set; }
        //public IEnumerable<Models.ApplicationDbContext> BirthDates { get; set; }
        // Return a pre-populated instance of AppliationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                ConfirmedEmail = this.ConfirmedEmail,
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                BirthDate = this.BirthDate,
                FirstYearSchool = this.FirstYearSchool,
                LastYearSchool = this.LastYearSchool,
                LastClass = this.LastClass,
                ActualCity = this.ActualCity,
                ActualCountry = this.ActualCountry,
                PhotoUrl = this.PhotoUrl,
                Photo = this.Photo,
                RegistrationDate = this.RegistrationDate,
            };
            return user;
        }

    }

    // SelectUserRolesViewModel – Revisited:
    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }

        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;

            var Db = new ApplicationDbContext();

            // Add all available roles to the list of EditorViewModels:
            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var userRole in user.Roles)
            {
                var checkUserRole =
                    this.Roles.Find(r => r.RoleName == userRole.Role.Name);
                checkUserRole.Selected = true;
            }
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    // Used to display a single role with a checkbox, within a list structure:
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        public SelectRoleEditorViewModel(IdentityRole role)
        {
            this.RoleName = role.Name;
        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }
    }

}
