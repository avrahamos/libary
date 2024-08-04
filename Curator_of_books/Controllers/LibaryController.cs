

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Libray.Service;
using Libray.ViewModel;
using Library.Service;
using Libray.Models;
namespace Libary.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IShelfService _shelfService;
        public LibraryController(ILibraryService libraryService, IShelfService shelfService)
        {
            _libraryService = libraryService;
            _shelfService = shelfService;
        }
        public async Task<IActionResult> Index() =>
            View(await _libraryService.GetAllLibraries());
        public IActionResult AddLibrary()
        {
            return View();
        }
        public async Task<IActionResult> EditLibrary(long id)
        {
            var library = await _libraryService.GetLibaryById(id);
            if (library == null)
            {
                return NotFound();
            }
            var libraryVm = new LibaryVM
            {
                Id = id,
                Ganre = library.Ganre
            };
            return View(libraryVm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLibrary(LibaryVM libraryVm)
        {
            if (!ModelState.IsValid)
            {
                return View("EditLibrary", libraryVm);
            }

            try
            {
                await _libraryService.UpdateLibrary(libraryVm);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddLibrary(LibaryVM libraryVm)
        {
            if (ModelState.IsValid)
            {
                await _libraryService.Create(libraryVm);
                return RedirectToAction(nameof(Index));
            }
            return View(libraryVm);
        }

       
    }
}