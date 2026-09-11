using System.Diagnostics;
using System.Linq;
using login_risk_detector.Data;
using login_risk_detector.Models;
using Microsoft.AspNetCore.Mvc;

namespace login_risk_detector.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        //IActionResult innebär att returtypen är en webb vy
        public async Task<IActionResult> Index()
        {
            //hämta 50 senaste loginevents
            var recentEvents = await _context.LoginEvents
                .OrderByDescending(equals => e.Timestamp)
                .Take(50
                .ToListAsync();

            //riskscore per specifik användare
            var userRisks = await _context.LoginEvents
                .GroupBy(e => e.UserId)
                .Select(g => g.OrderByDescending e => e.Timestampt).FirsOrDefault())


        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
