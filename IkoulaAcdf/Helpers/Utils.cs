using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;

namespace IkoulaACDF.Helpers
{
    public class Utils
    {

        public static void MakeCategoryFolder(string category, IkoulaACDF.Controllers.CategoryController controller)
        {
            if (category != string.Empty)
            {
                var folder = controller.Server.MapPath("~/Medias/_Photos/" + category);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }

        }
        public static void MakeSubCategoryFolder(string category, string subCategory, IkoulaACDF.Controllers.SubCategoryController controller)
        {
            if (category != string.Empty)
            {
                if(subCategory != string.Empty)
                {
                var folder = controller.Server.MapPath("~/Medias/_Photos/" + category + "/" + subCategory);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                }
            }

        }
        public static string GetGlobalValue(HttpContextBase cela, string key)
        {
            try
            {
                return cela.ApplicationInstance.Application.Get(key).ToString();
            }
            catch (Exception)
            {

            }
            return null;
        }

        //public static string GetCountGuessBooks(HttpContextBase cela)
        //{
        //    try
        //    {
        //        return cela.ApplicationInstance.Application.Get("CountGuessBooks").ToString();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return null;
        //}
        //public static string GetCountMembers(HttpContextBase cela)
        //{
        //    try
        //    {
        //        return cela.ApplicationInstance.Application.Get("CountMembers").ToString();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return null;
        //}
        public static void SendMail(string body, string subject, string firstName, string lastName, string email)
        {
            string myBody = string.Empty;
            string mySubject = string.Empty;

            if (body == "") { myBody = ConfigurationManager.AppSettings["cdf54.DefaultBody"].ToString(); } else { myBody = body; };
            if (subject == "") { 
                mySubject = ConfigurationManager.AppSettings["cdf54.DefaultSubject"].ToString();
                myBody = string.Format("{0}: {1} {2} avec email: {3}  [ Ne pas répondre à ce mail ]", myBody, firstName, lastName, email);
            } 
            else 
            { 
                mySubject = subject; 
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    var msg = new MailMessage()
                    {
                        Body = myBody,
                        Subject = mySubject
                    };

                    string[] toUsers = ConfigurationManager.AppSettings["cdf54.EmailsRegistration"].ToString().Split(',');
                    foreach (string destination in toUsers)
                    {
                        msg.To.Add(destination);
                    }
                    client.Send(msg);
                    System.Diagnostics.Debug.WriteLine(msg);
                }
           }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static void MyThumbUtil(IkoulaACDF.Models.PhotoViewModel photo, IkoulaACDF.Controllers.PhotoController controller)
        {
            try
            {
                //string thumbFileName = "";
                string fileName = "";

                string subCatName = photo.SubCategoryName.Contains("AUCUNE") ? string.Empty : photo.SubCategoryName;
                string path = @"/Medias\_Photos\" + photo.CategoryName + @"\" + subCatName;
                string[] tab = photo.Path.Split('\\');
                fileName = subCatName == string.Empty ? tab[4] : tab[5];
                string filePath = Path.Combine(controller.Server.MapPath(path), fileName);
                System.Web.Helpers.WebImage webImage = new System.Web.Helpers.WebImage(filePath);
                webImage.AddTextWatermark("http://jow-alva.net/ACDF", "White", 15);
                webImage.Save(filePath);
                //thumbFileName = fileName.Split('.')[0] + "_thumb_." + fileName.Split('.')[1];
                //string thumbFilePath = Path.Combine(controller.Server.MapPath(path), thumbFileName);
                //webImage.Resize(100, 100, preserveAspectRatio: true).Save(thumbFilePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Save photo to disk, used by Edit and Register with two different models.
        /// </summary>
        /// <param name="photo">HttpPostedFileWrapper</param>
        /// <param name="controller">Controller calling</param>
        /// <returns>Path where photo is stored with it's calculated filename, or default photo "BlankPhoto.jpg" or null on error</returns>
        public static string SavePhotoFileToDisk(HttpPostedFileWrapper photo, IkoulaACDF.Controllers.AccountController controller, string oldPhotoUrl, bool isNoPhotoChecked)
        {
            string photoPath = string.Empty;
            string fileName = string.Empty;

            // If photo is uploaded calculate his name
            if (photo != null)
            {
                fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            }
            else
            {
                // if user want to remove his photo
                if (oldPhotoUrl != null && isNoPhotoChecked == true)
                {
                    if (!oldPhotoUrl.Contains("BlankPhoto.jpg"))
                    {
                        string fileToDelete = Path.GetFileName(oldPhotoUrl);
                        var path = Path.Combine(controller.Server.MapPath("~/uploads"), fileToDelete);
                        FileInfo fi = new FileInfo(path);
                        if (fi.Exists)
                            fi.Delete();
                    }
                }

                // If no previews photo it's a new user who don't provide photo
                if (oldPhotoUrl == null || isNoPhotoChecked == true)
                {
                    fileName = "BlankPhoto.jpg";
                }
                else
                {
                    // User don't want to change his photo
                    return oldPhotoUrl;
                }
            }
            // We save the new/first photo on disk
            try
            {
                string path;
                path = Path.Combine(controller.Server.MapPath("~/uploads"), fileName);
                photoPath = Path.Combine(HttpRuntime.AppDomainAppVirtualPath, "uploads", fileName);
                // We save the new/first photo or nothing because BlankPhoto is in the folder
                if (photo != null) photo.SaveAs(path);
            }
            catch (Exception ex)
            {
                // Handled exception catch code
                Helpers.Utils.SignalExceptionToElmahAndTrace(ex, controller);
                return null;
            }
            return photoPath;
        }

        /// <summary>
        /// Save photo to disk, used by Edit and Register with two different models. model.Photo, this, string model.CategoryName, string model.SubCategoryName
        /// </summary>
        /// <param name="photo">HttpPostedFileWrapper</param>
        /// <param name="controller">Controller calling</param>
        /// <returns>Path where photo is stored with it's calculated filename, or default photo "BlankPhoto.jpg" or null on error</returns>
        public static string[] SaveAcdfPhotoFileToDisk(System.Web.Helpers.WebImage webImage,IkoulaACDF.Controllers.PhotoController controller, string categoryName, string subCategoryName)
        {
            string[] paths = new string[2];
            string photoPath = string.Empty;
            string thumbPath = string.Empty;
            string fileName = string.Empty;
            string thumbFileName = string.Empty;
            string subCatName = subCategoryName.Contains("AUCUNE") ? string.Empty : subCategoryName;


            if (webImage != null)
            {
                // If photo is uploaded calculate his name
                string guid = Guid.NewGuid().ToString();
                fileName = guid + "_" + webImage.FileName;
                thumbFileName = guid + "_thumb_" + webImage.FileName;
                //Calculate photoUrl and save photo on disk
                try
                {
                    string path = @"/ACDF/Medias\_Photos\" + categoryName + @"\" + subCatName;
                    //string path = @"/ACDF/Medias\_Photos\";

                    string filePath = Path.Combine(controller.Server.MapPath(path), fileName);
                    webImage.AddTextWatermark("http://jow-alva.net/ACDF", "White", 15);
                    webImage.Save(filePath);
                    photoPath = Path.Combine(HttpRuntime.AppDomainAppVirtualPath, path , fileName);

                    string thumbFilePath = Path.Combine(controller.Server.MapPath(path), thumbFileName);
                    webImage.Resize(150,150,preserveAspectRatio:true).Save(thumbFilePath);
                    thumbPath = Path.Combine(HttpRuntime.AppDomainAppVirtualPath, path, thumbFileName);

                    paths[0] = photoPath;
                    paths[1] = thumbPath;

                }
                catch (Exception ex)
                {
                    // Handled exception catch code
                    Helpers.Utils.SignalExceptionToElmahAndTrace(ex, controller);
                    return null;
                }
            }
            else
            {
                // Handled exception catch code
//TODO:         Helpers.Utils.SignalExceptionToElmahAndTrace(null, controller);
                return null;
            }
            return paths;
        }

        /// <summary>
        /// This function is used to check specified file being used or not
        /// http://dotnet-assembly.blogspot.fr/2012/10/c-check-file-is-being-used-by-another.html
        /// </summary>
        /// <param name="file">FileInfo of required file</param>
        /// <returns>If that specified file is being processed 
        /// or not found is return true</returns>
        public static Boolean IsFileLocked(string file)
        {
            FileStream stream = null;
            try
            {
                //Don't change FileAccess to ReadWrite, 
                //because if a file is in readOnly, it fails.
                stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked
            return false;
        }

        public static void SignalExceptionToElmahAndTrace(Exception ex, System.Web.Mvc.Controller lui)
        {
            MyTracer.MyTrace(
                System.Diagnostics.TraceLevel.Error,
                lui.GetType(),
                lui.ControllerContext.RouteData.Values["controller"].ToString(),
                lui.ControllerContext.RouteData.Values["action"].ToString(),
                ex.Message, ex);

            //Elmah.ErrorSignal.FromCurrentContext().Raise(ex); //ELMAH Signaling
        }


        // http://www.nullskull.com/a/10450951/aspnet-mvc-display-images-directly-from-the-viewmodel-into-your-views.aspx
        public static String ByteToStringImage(byte[] picture)
        {
            byte[] photo = picture;
            string imageSrc = string.Empty;
            if (photo != null)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(photo, 78, photo.Length - 78); // strip out 78 byte OLE header (don't need to do this for normal images)
                string imageBase64 = Convert.ToBase64String(ms.ToArray());
                imageSrc = string.Format("data:image/png;base64,{0}", imageBase64);
            }
            return imageSrc;
        }
        public static string GetAssemblyName()
        {
            Assembly assembly = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly;
            return assembly.GetName().Name;
        }
        public static DateTime GetAssemblyDateTime()
        {
            Assembly assembly = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;
            return lastModified;
        }
        public static string GetAssemblyInformationnalVersion()
        {
            Assembly assembly = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly;
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }
        public static string GetAssemblyVersion()
        {
            Assembly assembly = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly;
            return assembly.GetName().Version.ToString();
        }
        public static string GetAssemblyProduct()
        {
            Assembly assembly = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly;
            AssemblyProductAttribute assemblyProductAttribut = (AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute));
            return assemblyProductAttribut.Product;
        }
        public static string GetAssemblyCompany()
        {
            Assembly assembly = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly;
            AssemblyCompanyAttribute assemblyCompanyAttribute = (AssemblyCompanyAttribute)(Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute), false));
            return assemblyCompanyAttribute.Company;
        }
        public static string GetAssemblyCopyright()
        {
            Assembly assembly = HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly;
            AssemblyCopyrightAttribute assemblyCompanyAttribute = (AssemblyCopyrightAttribute)(Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute), false));
            return assemblyCompanyAttribute.Copyright;
        }
        public static string GetConfigCompanyName()
        {
            return ConfigurationManager.AppSettings["cdf54.CompanyName"].ToString();
        }
        public static string GetConfigCompanyAddress()
        {
            return ConfigurationManager.AppSettings["cdf54.CompanyAddress"].ToString();
        }
        public static string GetGoogleMapKey()
        {
            return ConfigurationManager.AppSettings["cdf54.GoogleMapKey"].ToString();
        }
        public static string GetUserName()
        {
            string returned = HttpContext.Current.User.Identity.Name;
            returned = returned == "" ? "Guest" : returned;
            return returned;
        }
        public static string GetCulture()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
        }
        public static string GetUiCulture()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
        }
        public static string GetCnxNORTHWNDEntities()
        {
            string northwindEntities = "No ConnectionString";
            string cnx = string.Empty;
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            if ((cnx = connections["NORTHWNDEntities"].ConnectionString) != string.Empty)
                northwindEntities = cnx;
            return northwindEntities;
        }
        public static string GetCnxDefaultConnection()
        {
            string defaultConnection = "No ConnectionString";
            string cnx = string.Empty;
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            if ((cnx = connections["DefaultConnection"].ConnectionString) != string.Empty)
                defaultConnection = cnx;
            return defaultConnection;
        }
        public static string GetApplicationStatus()
        {
            return ConfigurationManager.AppSettings["cdf54.Status"].ToString();
        }
        public static bool IsAspNetTraceEnabled()
        {
            return HttpContext.Current.Trace.IsEnabled;
        }
        public static bool IsCustomErrorEnabled()
        {
            return HttpContext.Current.IsCustomErrorEnabled;
        }
        public static bool IsDebuggingEnabled()
        {
            return HttpContext.Current.IsDebuggingEnabled;
        }
        public static string GetCompilationMode()
        {
            string compilationMode = "Release";
            if (IsDebuggingEnabled())
            {
                compilationMode = "Debug";
            }
            return compilationMode;
        }
        public static bool IsDemoExceptionLinksEnabled()
        {
            return (ConfigurationManager.AppSettings["ShowDemoExceptionLinks"].ToString() == "true");
        }
    }
}