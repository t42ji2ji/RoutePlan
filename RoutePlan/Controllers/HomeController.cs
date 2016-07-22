using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RoutePlan.ViewModels;

namespace RoutePlan.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string form_from,string form_to)
        {
            LocationViewModel lvm = new LocationViewModel();
            
            return View();
        }

        public ActionResult Update(String from,String to)
        {

            List<LocationViewModel> lvm = new List<LocationViewModel>();

            lvm.Add(new LocationViewModel { ID = from, longitude = 94.7, latitude = 99.9});
            lvm.Add(new LocationViewModel { ID = to, longitude = 0.2323, latitude = 0.2565656 });

            return Json(lvm);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View(new ReportViewModel());
        }

        [HttpPost]
        public ActionResult Contact(string reportName)
        {
            ReportViewModel rv = new ReportViewModel();
            rv.Name = reportName;
            return View(rv);
        }



    }
}