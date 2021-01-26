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

        public async Task<Photo> UpdatePhotoAsync(Photo photo, bool isUploadPhoto = true)
        {
            var result = await GetPhotoAsync(photo.Id);

            if (result != null)
            {
                if (isUploadPhoto)
                {
                    result.Date = photo.Date;
                    result.Location = photo.Location;
                    result.Description = photo.Description;
                    // todo 用这种方式更新ICollection，即使只想改一个值，也会删除DB中所有行，再重新添加
                    // 所以为了perfo，可能需要直接操作关系表
                    result.PhotoLabels = photo.PhotoLabels;
                }
                else
                {
                    result.Path = photo.Path;
                }

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
