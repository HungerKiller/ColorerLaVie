using PhotoMasterBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Repositories
{
    public interface ILabelRepository
    {
        Task<IEnumerable<Label>> GetLabelsAsync();
        Task<Label> GetLabelAsync(int labelId);
        Task<Label> GetLabelWithPhotosAsync(int labelId);
        Task<Label> AddLabelAsync(Label label);
        Task<Label> UpdateLabelAsync(Label label);
        Task DeleteLabelAsync(int labelId);
    }
}
