using Library.Service;
using Libray.Service;
using Libray.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libray.Controllers
{
    public class BookController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IShelfService _shelfService;
        private readonly ISetService _setService;
        private readonly IBooksServis _booksService;

        public BookController(ILibraryService libraryService, IShelfService shelfService, ISetService setService, IBooksServis booksService)
        {
            _libraryService = libraryService;
            _shelfService = shelfService;
            _setService = setService;
            _booksService = booksService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _booksService.GetBookModelAsync();

            var bookVMs = books.Select(book => new BookVM
            {
                Id = book.Id,
                Title = book.Title,
                Ganre = book.Ganre,
                High = book.Hige,
                Width = book.Width,
                SetId = book.SetId
            }).ToList();

            return View(bookVMs);
        }


        [HttpGet]
        public IActionResult CreateBook(long setId)
        {
           
            var bookVM = new BookVM
            {

                SetId = setId,
                Ganre = "",
                High = 0,
                Width = 0,
                Title ="",
            };

            return View(bookVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookVM bookVM)
        {
            if (ModelState.IsValid)
            {
                await _booksService.Create(bookVM);
                return RedirectToAction(nameof(Index)); 
            }

            return View(bookVM);
        }





        [HttpGet]
        public async Task<IActionResult> EditBook(long id)
        {
            var book = await _booksService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookVM = new BookVM
            {
                Id = id,
                Ganre = book.Ganre,
                Title = book.Title,
                High = book.Hige,
                Width = book.Width,
                SetId = book.SetId
            };

            ViewBag.Sets = new SelectList(await _setService.GetSetModelsAsync(), "Id", "SetName");
            return View(bookVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(BookVM bookVM)
        {
            if (ModelState.IsValid)
            {
                await _booksService.Edit(bookVM, bookVM.Id);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Sets = new SelectList(await _setService.GetSetModelsAsync(), "Id", "SetName");
            return View(bookVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddBooksToSet(long setId)
        {
            var set = await _setService.GetSetById(setId);
            if (set == null)
            {
                return NotFound();
            }

            var books = await _booksService.GetBookModelAsync();
            var shelves = await _shelfService.GetShelfListAsync();

            var viewModel = new AddBooksToSetVM
            {
                SetId = setId,
                Books = books.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Title
                }),
                Shelfes = shelves.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"Shelf {s.Id} ({s.Width} cm width)"
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooksToSet(AddBooksToSetVM model)
        {
            if (ModelState.IsValid)
            {
                var set = await _setService.GetSetById(model.SetId);
                if (set == null)
                {
                    return NotFound();
                }

                var books = await _booksService.GetBookModelAsync();
                var selectedBooks = books.Where(b => model.SelectedBookIds.Contains(b.Id)).ToList();

                foreach (var book in selectedBooks)
                {
                    book.SetId = model.SetId;
                    await _booksService.Edit(new BookVM
                    {
                        Id = book.Id,
                        Ganre = book.Ganre,
                        Title = book.Title,
                        High = book.Hige,
                        Width = book.Width,
                        SetId = book.SetId
                    }, book.Id);
                }

                var (totalHeight, totalWidth) = await _booksService.GetSetDimensionsAsync(model.SetId);

                var library = await _libraryService.GetLibraryByGenreAsync(selectedBooks.FirstOrDefault()?.Ganre);

                if (library != null)
                {
                    var suitableShelf = (await _shelfService.GetShelfListAsync())
                        .FirstOrDefault(s => s.LibaryId == library.Id && s.High >= totalHeight && s.Width >= totalWidth);

                    if (suitableShelf != null)
                    {
                        await _setService.AddSetToShelf(suitableShelf.Id, model.SetId);
                    }
                    else
                    {
                        var newShelf = new ShelfVM
                        {
                            High = totalHeight > 100 ? 100 : totalHeight,
                            Width = totalWidth,
                            LibaryId = library.Id
                        };
                        await _shelfService.Create(newShelf);
                        await _setService.AddSetToShelf(newShelf.Id, model.SetId);
                    }
                }
                else
                {
                    var newLibrary = await _libraryService.Create(new LibaryVM { Ganre = selectedBooks.FirstOrDefault()!.Ganre });
                    var newShelf = new ShelfVM
                    {
                        High = totalHeight > 100 ? 100 : totalHeight,
                        Width = totalWidth,
                        LibaryId = newLibrary.Id
                    };
                    await _shelfService.Create(newShelf);
                    await _setService.AddSetToShelf(newShelf.Id, model.SetId);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Sets = new SelectList(await _setService.GetSetModelsAsync(), "Id", "SetName");
            ViewBag.Shelves = new SelectList(await _shelfService.GetShelfListAsync(), "Id", "Width");

            return View(model);
        }
    }
}
