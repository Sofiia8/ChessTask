using Microsoft.AspNetCore.Mvc;

namespace ChessTask.Controllers
{
    public class HomeController : Controller
    {
        static Algoritm algoritm;
        [HttpGet]
        public IActionResult Start()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Start(int al, int n0, int m0, double t)
        {     
            algoritm = new Algoritm(al, n0, m0);
            ViewBag.coord_x = n0;
            ViewBag.coord_y = m0;
            ViewBag.t = t;

            return View("Draw", algoritm.Path);
        }
    }
}
