using Curator_of_books.Models;
using Library.Service;
using Libray.Models;
using Libray.Service;
using Libray.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Threading.Tasks;

namespace Libray.Controllers
{
    public class ShelfController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IShelfService _shelfService;
        private readonly ISetService _setService;

        public ShelfController(ILibraryService libraryService, IShelfService shelfService, ISetService setService)
        {
            _libraryService = libraryService;
            _shelfService = shelfService;
            _setService = setService;
        }

        public async Task<IActionResult> Index()
        {
            var shelves = await _shelfService.GetShelfListAsync();
            return View(shelves);
        }
        public IActionResult AddShelf()
        {
            ViewBag.Libraries = new SelectList(_libraryService.GetAllLibraries().Result, "Id", "Ganre");
            return View(new ShelfVM());
        }

        [HttpPost]
        public async Task<IActionResult> AddShelf(ShelfVM shelfVM)
        {
            if (ModelState.IsValid)
            {
                await _shelfService.Create(shelfVM);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Libraries = new SelectList(await _libraryService.GetAllLibraries(), "Id", "Ganre");
            return View(shelfVM);
        }




        [HttpGet]
        public async Task<IActionResult> EditShelf(long id)
        {
            var shelf = await _shelfService.GetShelfById(id);
            if (shelf == null)
            {
                return NotFound();
            }

            var shelfVM = new ShelfVM
            {
                High = shelf.High,
                Width = shelf.Width,
                Id = id,
                LibaryId = shelf.LibaryId 
            };

            var libaries = await _libraryService.GetAllLibraries();
            ViewBag.Libraries = new SelectList(libaries, "Id", "Ganre", shelf.LibaryId);
            return View(shelfVM);
        }
        



        [HttpPost]
        public async Task<IActionResult> UpdateShelf(ShelfVM shelfVM)
        {

            if (!ModelState.IsValid)
            {
                return View("EditShelf", shelfVM);
            }
            try
            {
                await _shelfService.UpdateShelfAsync(shelfVM);
            }
            catch (KeyNotFoundException) 
            {
                return NotFound();
            }
            return RedirectToAction("Index");

        }
    }
}
