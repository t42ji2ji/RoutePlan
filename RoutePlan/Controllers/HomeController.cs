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
            try
            {                
                graph.ReadEdge(Server.MapPath("~/DATA/binary/100.edges"));
                graph.ReadNode(Server.MapPath("~/DATA/binary/100.nodes"));

                List<List<Vertex>> lvm;

                List<Path> paths = graph.SkylineQuery("1", "7");

                lvm = graph.TransformPaths(paths);

                return Json(lvm);
            }
            catch (GraphException e)
            {
                return Json("");
            }            
        }

        public ActionResult test(String from, String to)
        {
            try
            {
                graph.ReadEdge(Server.MapPath("~/DATA/binary/california 500(6).edges"));
                graph.ReadNode(Server.MapPath("~/DATA/binary/california.nodes"));

                List<List<Vertex>> lvm = graph.EnumPath("1", "263");

                return Json(lvm[0]);
            }
            catch (GraphException)
            {
                return Json("");
            }

            /*int x = 10;
            int y = 20;
            //List<List<Vertex>>
            List<Vertex> lvm = new List<Vertex>();
            for (int i = 0; i < 10; i++) {
                lvm.Add(new Vertex(i.ToString(), i + 10, i + 20));
            }

            return Json(lvm);*/
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