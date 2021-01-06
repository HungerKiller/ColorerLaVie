﻿using PhotoMasterBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Repositories
{
    interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotosAsync();
        Task<Photo> GetPhotoAsync(int photoId);
        Task<Photo> AddPhotoAsync(Photo photo);
        Task<Photo> UpdatePhotoAsync(Photo photo);
        Task DeletePhotoAsync(int photoId);
    }
}
