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
        private Graph graph = new Graph(new Graph.BinaryReader());

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
            graph.ReadEdge(Server.MapPath("~/DATA/binary/ming.edges"));
            graph.ReadNode(Server.MapPath("~/DATA/binary/ming.nodes"));

            List<LocationViewModel> lvm = new List<LocationViewModel>();

            //List<Path> paths = graph.SkylineQuery("會同館", "翟城驛");

           /* foreach(Vertex vertex in graph.TransformPath(paths[0]))            
                lvm.Add(new LocationViewModel { ID = vertex.ID,
                                                longitude = vertex.Longitude,
                                                latitude = vertex.Latitude });*/
            
            return Json(lvm);
        }

        public ActionResult test(String from, String to)
        {

            int x = 10;
            int y = 20;
            //List<List<Vertex>>
            List<Vertex> lvm = new List<Vertex>();
            for (int i = 0; i < 10; i++) {
                lvm.Add(new Vertex(i.ToString(), i + 10, i + 20));
            }

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