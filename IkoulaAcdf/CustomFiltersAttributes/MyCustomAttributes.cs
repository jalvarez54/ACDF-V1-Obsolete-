﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace IkoulaACDF.CustomFiltersAttributes
{
    public class DateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value.ToString() == "01/01/1950" || !(value is DateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DateTestAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value.ToString() == "01/01/1901" || !(value is DateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public class DateRangeAttribute : ValidationAttribute
    {
        private const string DateFormat = "yyyy/MM/dd";
        private const string DefaultErrorMessage =
     "'{0}' must be a date between {1:d} and {2:d}.";

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public DateRangeAttribute(string minDate, string maxDate)
            : base(DefaultErrorMessage)
        {
            MinDate = ParseDate(minDate);
            MaxDate = ParseDate(maxDate);
        }

        public override bool IsValid(object value)
        {
            if (value == null || !(value is DateTime))
            {
                return true;
            }
            DateTime dateValue = (DateTime)value;
            return MinDate <= dateValue && dateValue <= MaxDate;
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
     ErrorMessageString,
                name, MinDate, MaxDate);
        }

        private static DateTime ParseDate(string dateValue)
        {
            return DateTime.ParseExact(dateValue, DateFormat,
     CultureInfo.InvariantCulture);
        }
    }


    /// <summary>
    /// Customized data annotation validator for uploading file
    /// http://www.dotnet-tricks.com/Tutorial/mvc/aX9D090113-File-upload-with-strongly-typed-view-and-model-validation.html
    /// </summary>
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string uri = HttpContext.Current.Request.Url.ToString();
            int maxContentLength = 1024 * 1024 * 3; // photo color = 3 x 8 bits = 3 bytes
            int maxHeight = 200;
            int maxWidth = 200;
            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png"};
            var file = value as HttpPostedFileBase;
            // user don't want photo is permitted
            if (file == null)
                return true;
                // test photo type
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = string.Format( "Please upload Your Photo of type: {0}" , string.Join(", ", AllowedFileExtensions));
                return false;
            }
                // test photo size
            else if (file.ContentLength > maxContentLength)
            {
                ErrorMessage = string.Format("Your Photo is too large, maximum allowed size is : {0} MB" , (maxContentLength / 1024).ToString());
                return false;
            }
            else
            {
                // test photo dimensions
                using (System.Drawing.Image myImage = System.Drawing.Image.FromStream( file.InputStream))
                {
                    if (myImage.Height > maxHeight && myImage.Width > maxWidth)
                    {
                        ErrorMessage = string.Format("Your Photo is too large, maximum allowed size is : Width {0} x Height {1} pixels", maxWidth, maxHeight);
                        return false;
                    }
                }
            }
         return true;
        }
    }
    /// <summary>
    /// http://visualstudiomagazine.com/articles/2013/08/28/asp_net-authentication-filters.aspx
    /// </summary>
    public class MyAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        void IAuthenticationFilter.OnAuthentication(AuthenticationContext filterContext)
        {
        }
        void IAuthenticationFilter.OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            string Name = filterContext.HttpContext.User.Identity.Name;

            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();

                // Authentication challenge trace centralyzed
                Helpers.MyTracer.MyTrace(System.Diagnostics.TraceLevel.Info, this.GetType(),
                      controllerName, actionName, "OnAuthenticationChallenge", null);
            }
        }
    }
    public class MyRequestLogFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Don't show filter multiple times when using Html.RenderAction or Html.Action.
            if (filterContext.IsChildAction == true)
                return;

            // Action trace centralyzed
            Helpers.MyTracer.MyTrace(System.Diagnostics.TraceLevel.Info, this.GetType(),
                  filterContext.Controller.ToString(), filterContext.ActionDescriptor.ActionName, "Passage dans une Action", null);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}