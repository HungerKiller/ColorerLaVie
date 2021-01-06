using PhotoMasterBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Repositories
{
    public interface ILabelRepository
    {
        Task<IEnumerable<Label>> GetLabels();
        Task<Label> GetLabel(int labelId);
        Task<Label> AddLabel(Label label);
        Task<Label> UpdateLabel(Label label);
        Task DeleteLabel(int labelId);
    }
}
