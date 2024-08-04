using Libray.Data;
using Libray.Models;
using Libray.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Libray.Service
{
    public class BooksService : IBooksServis
    {
        private readonly ApplicationDbContext _context;
        public BooksService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookModel> Create(BookVM bookVM)
        {
            BookModel book = new()
            {
                Ganre = bookVM.Ganre,
                Title = bookVM.Title,
                Hige = bookVM.High,
                Width = bookVM.Width,
                SetId = bookVM.SetId
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<BookModel?> Delete(long id)
        {
            BookModel? toDelete = await _context.Books.FindAsync(id);
            if (toDelete != null)
            {
                _context.Books.Remove(toDelete);
                await _context.SaveChangesAsync();
                return toDelete;
            }
            return null;
        }

        public async Task<BookModel?> Details(long id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<BookModel?> Edit(BookVM bookVM, long id)
        {
            BookModel? book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                book.Ganre = bookVM.Ganre;
                book.Title = bookVM.Title;
                book.Hige = bookVM.High;
                book.Width = bookVM.Width;
                book.SetId = bookVM.SetId;

                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return book;
            }
            return null;
        }

        public async Task<IEnumerable<BookModel>> GetBookModelAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<BookModel?> GetBookById(long id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<(int totalHeight, int totalWidth)> GetSetDimensionsAsync(long setId)
        {
            var booksInSet = await _context.Books.Where(b => b.SetId == setId).ToListAsync();

            if (!booksInSet.Any())
            {
                return (0, 0); 
            }

            int totalHeight = booksInSet.Max(b => b.Hige); 
            int totalWidth = booksInSet.Sum(b => b.Width);

            return (totalHeight, totalWidth);
        }
    }
}
