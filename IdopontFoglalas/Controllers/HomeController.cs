using System.Diagnostics;
using IdopontFoglalas.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdopontFoglalas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var foglalasok = MySQL.FoglalasLista();
            return View(foglalasok);
        }

        public IActionResult UjFoglalas()
        {
            return View();
        }

        public IActionResult Foglalas()
        {
            var lista = MySQL.FoglalasLista();
            return View(lista);
        }

        [HttpPost]
        public IActionResult UjFoglalas(Foglalas foglalas)
        {
            if(MySQL.SzabadeADatum(foglalas.FoglalasIdopont))
            {
                ViewBag.Hiba = "Ez az idopont foglalt! K�rlek v�lassz m�sikat";
                return View(foglalas);
            }

            bool sikerult = MySQL.UjFoglalas(foglalas);
            if (sikerult)
            {
                TempData["Uzenet"] = "A foglal�s sikeresen l�trej�tt!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Hiba = "Hiba t�rt�nt a foglal�s ment�sekor.";
                return View(foglalas);
            }
        }

        public IActionResult Torles( int id)
        {
            MySQL.FoglalasTorles(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
