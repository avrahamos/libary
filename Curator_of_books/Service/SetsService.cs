using Libray.Data;
using Libray.Models;
using Libray.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Libray.Service
{
    public class SetsService : ISetService
    {
        private readonly ApplicationDbContext _context;

        public SetsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SetModel> Create(SetVM model)
        {
            SetModel set = new()
            {
                SetName = model.SetName
            };
            await _context.AddAsync(set);
            await _context.SaveChangesAsync();
            return set;
        }

        public async Task<SetModel> Delete(long id)
        {
            var toDelete = await _context.Sets.FindAsync(id);
            if (toDelete == null)
            {
                throw new KeyNotFoundException($"Set to delete with ID {id} not found.");
            }
            _context.Sets.Remove(toDelete);
            await _context.SaveChangesAsync();
            return toDelete;
        }

        public async Task<SetModel> Details(long id)
        {
            var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == id);
            if (set == null)
            {
                throw new KeyNotFoundException($"Set with ID {id} not found");
            }
            return set;
        }

        public async Task<SetModel?> GetSetById(long id) => await _context.Sets.FindAsync(id);

        public async Task<List<SetModel>> GetSetModelsAsync() => await _context.Sets.ToListAsync();

        public async Task UpdateSet(SetVM setVM)
        {
            var set = await _context.Sets.FindAsync(setVM.Id);
            if (set == null)
            {
                throw new Exception("Set not found");
            }

            set.SetName = setVM.SetName;
            _context.Sets.Update(set);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SetModel>> GetSetsByShelfId(long shelfId)
        {
            return await _context.Sets.Where(s => s.ShelfId == shelfId).ToListAsync();
        }

        public async Task AddSetToShelf(long shelfId, long setId)
        {
            var set = await _context.Sets.FindAsync(setId);
            if (set != null)
            {
                set.ShelfId = shelfId;
                _context.Sets.Update(set);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CanAddSetToShelf(long shelfId, long setId)
        {
            var shelf = await _context.Shelfs.FindAsync(shelfId);
            var set = await _context.Sets.Include(s => s.books).FirstOrDefaultAsync(s => s.Id == setId);

            if (shelf == null || set == null)
            {
                return false;
            }

            int totalSetWidth = set.books.Sum(b => b.Width);

            if (totalSetWidth > shelf.Width)
            {
                return false;
            }

            if (set.books.Any(b => b.Hige > shelf.High))
            {
                return false;
            }

            int CurrentWidthUpdate = shelf.Width - totalSetWidth;

            if (CurrentWidthUpdate < 0)
            {
                return false;
            }
            shelf.Width=CurrentWidthUpdate;
            return true;
        }

    }
}
