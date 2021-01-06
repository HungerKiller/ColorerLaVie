using Microsoft.EntityFrameworkCore;
using PhotoMasterBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly PhotoContext _context;

        public LabelRepository(PhotoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Label>> GetLabelsAsync()
        {
            return await _context.Labels.ToListAsync();
        }

        public async Task<Label> GetLabelAsync(int labelId)
        {
            return await _context.Labels.FindAsync(labelId);
        }

        public async Task<Label> AddLabelAsync(Label label)
        {
            var result = await _context.Labels.AddAsync(label);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Label> UpdateLabelAsync(Label label)
        {
            var result = await _context.Labels.FindAsync(label.Id);

            if (result != null)
            {
                result.Name = label.Name;
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task DeleteLabelAsync(int labelId)
        {
            var result = await _context.Labels.FindAsync(labelId);
            if (result != null)
            {
                _context.Labels.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}
