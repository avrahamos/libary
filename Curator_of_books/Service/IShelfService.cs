using Libray.Models;
using Libray.ViewModel;

namespace Libray.Service
{
    public interface IShelfService
    {
        public Task<List<ShelfModel>> GetShelfListAsync();
        public Task<ShelfModel> Create( ShelfVM shelf);
        public Task<ShelfModel?> GetShelfById( long id );
        public Task<ShelfModel?> Delete(long id);
        public Task<ShelfModel> Details(long id);
        public Task UpdateShelfAsync(ShelfVM shelfVm);
        public Task<ShelfModel?> GetSuitableShelf(long libraryId, int height, int width);
       public Task<ShelfModel> CreateShelfIfNeeded(long libraryId, int height, int width);

    }
}
