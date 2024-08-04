using Libray.Models;
using Libray.ViewModel;

namespace Libray.Service
{
    public interface ISetService
    {
        Task<List<SetModel>> GetSetModelsAsync();
        Task<SetModel> Create(SetVM model);
        Task<SetModel?> GetSetById(long id);
        Task<SetModel> Delete(long id);
        Task<SetModel> Details(long id);
        Task UpdateSet(SetVM setVM);

        Task<List<SetModel>> GetSetsByShelfId(long shelfId);
        Task AddSetToShelf(long shelfId, long setId);
        Task<bool> CanAddSetToShelf(long shelfId, long setId);
    }
}
