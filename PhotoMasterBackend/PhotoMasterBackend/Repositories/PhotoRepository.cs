using Microsoft.EntityFrameworkCore;
using PhotoMasterBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoContext _context;

        public PhotoRepository(PhotoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            return await _context.Photos.Include(p => p.PhotoLabels).ThenInclude(pl => pl.Label).ToListAsync();
        }

        public async Task<Photo> GetPhotoAsync(int photoId)
        {
            return await _context.Photos.Include(p => p.PhotoLabels).ThenInclude(pl => pl.Label).FirstOrDefaultAsync(p => p.Id == photoId);
        }

        public async Task<Photo> AddPhotoAsync(Photo photo)
        {
            var result = await _context.Photos.AddAsync(photo);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Photo> UpdatePhotoAsync(Photo photo)
        {
            var result = await GetPhotoAsync(photo.Id);

            if (result != null)
            {
                result.Date = photo.Date;
                result.Location = photo.Location;
                result.Description = photo.Description;
                result.PhotoLabels = photo.PhotoLabels;
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task DeletePhotoAsync(int photoId)
        {
            var result = await _context.Photos.FindAsync(photoId);
            if (result != null)
            {
                _context.Photos.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}
