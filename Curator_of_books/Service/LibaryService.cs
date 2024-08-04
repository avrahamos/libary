using Libray.Models;
using Libray.Data;
using Libray.ViewModel;
using Microsoft.EntityFrameworkCore;


namespace Library.Service
{
    public class LibraryService : ILibraryService
	{
		private readonly ApplicationDbContext _context;
		public LibraryService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<LibaryModel> Create(LibaryVM libraryVm)
		{

			LibaryModel model = new()
			{
				Ganre = libraryVm.Ganre
			};
			await _context.Libaries.AddAsync(model);
			await _context.SaveChangesAsync();
			return model;
		}

        public async Task<LibaryModel?> Delete(long id)
        {
            LibaryModel? toDelete =await _context.Libaries.FindAsync(id);
			if (toDelete != null)
			{
				_context.Libaries.Remove(toDelete);
				_context.SaveChanges();
				return toDelete;
			}
			return null;
        }

        public async Task<LibaryModel?> Details(long id)
        {
            LibaryModel? libary=await _context.Libaries.FindAsync(id);
			if(libary == null)
			{
				return null;
			}
			return libary;
        }

		public async Task<LibaryModel?> 
			GetLibaryById(long id) => await _context.Libaries.FindAsync(id);
        

        public async Task<IEnumerable<LibaryModel>> GetAllLibraries() =>
		   await _context.Libaries.ToListAsync();


        public async Task UpdateLiabary(LibaryVM liabryvm)
        {
            var liabry = await _context.Libaries.FindAsync(liabryvm.Id);
            if (liabry == null)
            {
                throw new Exception("liabry not found");
            }

            liabry.Ganre = liabryvm.Ganre;

            _context.Libaries.Update(liabry);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateLibrary(LibaryVM libraryVm)
        {
            var library = await _context.Libaries.FindAsync(libraryVm.Id);
            if (library == null)
            {
                throw new KeyNotFoundException("Library not found.");
            }

            library.Ganre = libraryVm.Ganre;

            _context.Libaries.Update(library);
            await _context.SaveChangesAsync();
        }
        public async Task<LibaryModel?> GetLibraryByGenreAsync(string genre)
        {
            return await _context.Libaries
                .FirstOrDefaultAsync(l => l.Ganre == genre);
        }

    }
}