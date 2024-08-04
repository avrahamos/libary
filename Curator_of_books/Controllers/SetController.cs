using Library.Service;
using Libray.Service;
using Libray.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Libray.Controllers
{
    public class SetController : Controller
    {
        private readonly IShelfService _shelfService;
        private readonly ISetService _setService;

        public SetController(IShelfService shelfService, ISetService setService)
        {
            _shelfService = shelfService;
            _setService = setService;
        }

        public async Task<IActionResult> Index()
        {
            var sets = await _setService.GetSetModelsAsync();
            var setVMs = sets.Select(set => new SetVM
            {
                Id = set.Id,
                SetName = set.SetName
            }).ToList();
            return View(setVMs);
        }

        [HttpGet]
        public IActionResult CreateSet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSet(SetVM setVM)
        {
            if (ModelState.IsValid)
            {
                await _setService.Create(setVM);
                return RedirectToAction(nameof(Index));
            }
            return View(setVM);
        }

        [HttpGet]
        public async Task<IActionResult> EditSet(long id)
        {
            var set = await _setService.GetSetById(id);
            if (set == null)
            {
                return NotFound();
            }

            var setVM = new SetVM
            {
                Id = id,
                SetName = set.SetName
            };

            return View(setVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditSet(SetVM setVM)
        {
            if (ModelState.IsValid)
            {
                await _setService.UpdateSet(setVM);
                return RedirectToAction(nameof(Index));
            }
            return View(setVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddBooksToSet(long id)
        {
            var set = await _setService.GetSetById(id);
            if (set == null)
            {
                return NotFound();
            }

            ViewBag.SetId = id;
            return RedirectToAction("CreateBook", "Book", new { setId = id });
        }
    }
}
