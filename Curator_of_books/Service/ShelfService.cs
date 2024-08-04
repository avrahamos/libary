using Libray.Data;
using Libray.Models;
using Libray.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libray.Service
{
    public class ShelfService : IShelfService
    {
        private readonly ApplicationDbContext _context;

        public ShelfService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShelfModel> Create(ShelfVM shelfVM)
        {
            if (shelfVM == null)
                throw new ArgumentNullException(nameof(shelfVM));

            var newShelf = new ShelfModel
            {
                High = shelfVM.High,
                Width = shelfVM.Width,
                LibaryId = shelfVM.LibaryId
            };

            await _context.Shelfs.AddAsync(newShelf);
            await _context.SaveChangesAsync();
            return newShelf;
        }

        public async Task<ShelfModel?> Delete(long id)
        {
            var toDelete = await _context.Shelfs.FindAsync(id);
            if (toDelete != null)
            {
                _context.Shelfs.Remove(toDelete);
                await _context.SaveChangesAsync();
                return toDelete;
            }
            return null;
        }

        public async Task<ShelfModel> Details(long id)
        {
            var shelf = await _context.Shelfs
                .Include(s => s.sets)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shelf == null)
                throw new KeyNotFoundException($"Shelf with ID {id} not found.");

            return shelf;
        }

        public async Task<ShelfModel?> GetShelfById(long id) => await _context.Shelfs.FindAsync(id);

        public async Task<List<ShelfModel>> GetShelfListAsync() =>
            await _context.Shelfs.ToListAsync();

        public async Task UpdateShelfAsync(ShelfVM shelfVm)
        {
            var shelf = await _context.Shelfs.FindAsync(shelfVm.Id);
            if (shelf == null)
                throw new InvalidOperationException("Shelf not found");

            shelf.High = shelfVm.High;
            shelf.Width = shelfVm.Width;

            _context.Shelfs.Update(shelf);
            await _context.SaveChangesAsync();
        }

        public async Task<ShelfModel?> GetSuitableShelf(long libraryId, int height, int width)
        {
            return await _context.Shelfs
                .Where(s => s.LibaryId == libraryId && s.High >= height && s.Width >= width)
                .OrderBy(s => s.High)
                .ThenBy(s => s.Width)
                .FirstOrDefaultAsync();
        }

        public async Task<ShelfModel> CreateShelfIfNeeded(long libraryId, int height, int width)
        {
            var shelf = await GetSuitableShelf(libraryId, height, width);

            if (shelf != null)
            {
                return shelf;
            }

            var newShelf = new ShelfModel
            {
                High = Math.Min(height, 100),
                Width = width,
                LibaryId = libraryId
            };

            return await Create(new ShelfVM
            {
                High = newShelf.High,
                Width = newShelf.Width,
                LibaryId = newShelf.LibaryId
            });
        }
    }
}
