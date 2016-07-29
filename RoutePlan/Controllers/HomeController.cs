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
        private static Graph graph = new Graph(7);
        public ActionResult Index()
        {
            graph.ReadEdge("C:\\Users\\奕中老師\\Desktop\\res\\res\\salu_edge.txt");
            graph.ReadVertices("C:\\Users\\奕中老師\\Desktop\\res\\res\\salu_node.txt");
            
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
           
            List<string> path = graph.shortestPathQuery("1", "917");
            List<Vertex> vertexPath = graph.transformPath(path);
            Vertex v1, v2;

            v1 = vertexPath[0];
            v2 = vertexPath[1];

            List<LocationViewModel> lvm = new List<LocationViewModel>();

           lvm.Add(new LocationViewModel { ID = v1.ID, longitude = v1.Longitude, latitude = v1.Latitude});
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