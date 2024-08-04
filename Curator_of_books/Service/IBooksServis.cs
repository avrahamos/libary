using Libray.Models;
using Libray.ViewModel;

namespace Libray.Service
{
    public interface IBooksServis
    {
        Task<BookModel> Create(BookVM bookVM);
        Task<BookModel?> Delete(long id);
        Task<BookModel?> Details(long id);
        Task<BookModel?> Edit(BookVM bookVM, long id);
        Task<IEnumerable<BookModel>> GetBookModelAsync();
        Task<BookModel?> GetBookById(long id);

        Task<(int totalHeight, int totalWidth)> GetSetDimensionsAsync(long setId);
    }
}
