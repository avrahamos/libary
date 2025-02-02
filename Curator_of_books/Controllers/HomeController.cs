using Curator_of_books.Models;
using Libray.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Curator_of_books.Controllers
{
	
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger , ApplicationDbContext context)
		{
			
			_logger = logger;
			_context = context;

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
