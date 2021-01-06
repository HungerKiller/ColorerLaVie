using PhotoMasterBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Repositories
{
    interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotos();
        Task<Photo> GetPhoto(int photoId);
        Task<Photo> AddPhoto(Photo photo);
        Task<Photo> UpdatePhoto(Photo photo);
        Task DeletePhoto(int photoId);
    }
}
