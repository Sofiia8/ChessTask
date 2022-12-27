using Microsoft.AspNetCore.Mvc;

namespace ChessTask.Controllers
{
    public class HomeController : Controller
    {
        static Algoritm algoritm;
        static int step = 0;
        [HttpGet]
        public IActionResult Start()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Start(int al, int n0, int m0, double t)
        {     
            if (step == 0)
            {
                algoritm = new Algoritm(al, n0, m0);
                step = 1;
                ViewBag.coord_x = n0;
                ViewBag.coord_y = m0;
            }
            else
            {
                if (step < 64)
                {
                    ViewBag.coord_x = algoritm.Path[step][0];
                    ViewBag.coord_y = algoritm.Path[step][1];
                    step++;
                }
                else
                {
                    ViewBag.coord_x = algoritm.Path[0][0];
                    ViewBag.coord_y = algoritm.Path[0][1];
                    ViewBag.path = algoritm.Path;
                    return View("End");
                }
            }
            
            return View("Draw");
        }
    }
}
