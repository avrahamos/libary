
using Libray.ViewModel;
using Libray.Models;
namespace Library.Service
{
	public interface ILibraryService
	{
		Task<LibaryModel> Create(LibaryVM libraryVm);
		Task<IEnumerable<LibaryModel>> GetAllLibraries();
		Task<LibaryModel?> GetLibaryById( long id);
		Task<LibaryModel?> Details(long id);
		Task<LibaryModel?> Delete(long id);
		Task UpdateLiabary(LibaryVM liabryvm);
		Task UpdateLibrary(LibaryVM libraryVm);
        Task<LibaryModel?> GetLibraryByGenreAsync(string genre);

    }
}