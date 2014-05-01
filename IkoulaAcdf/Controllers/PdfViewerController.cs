using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IkoulaACDF.Controllers
{
    public class PdfViewerController : Controller
    {
        //
        // GET: /PdfViewer/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Viewer_Google(string filePath)
        {
            filePath = HttpRuntime.AppDomainAppVirtualPath + "/MyPDFs/" + filePath;
            ViewBag.filePath = filePath;
            return View();
        }
        public ActionResult Viewer_PdfJs(string filePath)
        {
            //build the file path here
            //filePath = "/ACDF/MyPDFs/" + filePath;
            //pass the file path to the View using a viewbag variable
            filePath = HttpRuntime.AppDomainAppVirtualPath + "/MyPDFs/" + filePath;
            ViewBag.filePath = filePath;
            //We could also just return a view along with a query string with a file param pointing to the
            //location of the file on our server, for example: "Viewer?file=/MyPDFs/Pdf1.pdf"
            //but here I've just chosen to change the default URL of the viewer object, which is essentially
            //the same thing
            return View();
        }
        public FileResult GetFile(string fileName)
        {
            string _fileName = "palmares1966.pdf";
            // Force the pdf document to be displayed in the browser
            Response.AppendHeader("Content-Disposition", "inline; filename=" + _fileName + ";");

            string path = AppDomain.CurrentDomain.BaseDirectory + "MyPDFs/";
            return File(path + _fileName, System.Net.Mime.MediaTypeNames.Application.Pdf, _fileName);
        }

    }
}