﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhotoMasterBackend.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly ILogger<PhotosController> _logger;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        private readonly ILabelRepository _labelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PhotosController(ILogger<PhotosController> logger, IMapper mapper, IPhotoRepository photoRepository, ILabelRepository labelRepository, IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _photoRepository = photoRepository;
            _labelRepository = labelRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOs.Photo>>> GetsAsync()
        {
            try
            {
                var photos = await _photoRepository.GetPhotosAsync();
                // Specfic filter - visitor get photos ==> return visitor_admin's photo
                var (userId, userRole) = GetCurrentUserInfo();
                if (userRole != Models.Role.Admin)
                {
                    var currentUser = await _userRepository.GetUserAsync(userId);
                    var relatedAdminUser = await _userRepository.GetUserAsync($"{currentUser.Username}_admin");
                    photos = photos.Where(p => p.UserId == relatedAdminUser.Id);
                }

                var photosDTO = photos.Select(photo => _mapper.Map<DTOs.Photo>(photo));
                return Ok(photosDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while retrieving data from database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // GET api/Photos/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<ActionResult<DTOs.Photo>> GetAsync(int id)
        {
            try
            {
                var photo = await _photoRepository.GetPhotoAsync(id);
                var photoDTO = _mapper.Map<DTOs.Photo>(photo);
                return Ok(photoDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while retrieving data from database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // POST api/Photos
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<DTOs.Photo>> PostAsync([FromBody] DTOs.Photo photo)
        {
            try
            {
                if (photo == null)
                    return BadRequest($"Photo object from body is null.");
                // Create photo
                var (userId, userRole) = GetCurrentUserInfo();
                var photoModel = _mapper.Map<Models.Photo>(photo);
                photoModel.UserId = userId;
                var createdPhoto = await _photoRepository.AddPhotoAsync(photoModel);
                // Get created photo
                var photoDTO = _mapper.Map<DTOs.Photo>(await _photoRepository.GetPhotoAsync(createdPhoto.Id));
                return CreatedAtRoute("GetPhoto", new { id = photoDTO.Id }, photoDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while creating new photo into database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // PUT api/Photos/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<DTOs.Photo>> PutAsync(int id, [FromBody] DTOs.Photo photo)
        {
            try
            {
                if (id != photo.Id)
                    return BadRequest($"Photo id from url and body are not identical.");

                if (await _photoRepository.GetPhotoAsync(id) == null)
                    return NotFound($"Photo with id '{id}' not found.");

                // Check label's id
                if (photo.Labels != null)
                {
                    foreach (var l in photo.Labels)
                    {
                        var label = await _labelRepository.GetLabelAsync(l.Id);
                        if (label == null)
                            return BadRequest($"Label with id '{l.Id}' not found!");
                    }
                }

                // Update photo
                var photoUpdated = await _photoRepository.UpdatePhotoAsync(_mapper.Map<Models.Photo>(photo), true);
                var photoDTO = _mapper.Map<DTOs.Photo>(photoUpdated);
                return Ok(photoDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while updating data of database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // DELETE api/Photos/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var photo = await _photoRepository.GetPhotoAsync(id);
                if (photo == null)
                    return NotFound($"Photo with id '{id}' not found.");

                // Get file path on disk
                var lenPrefix = _configuration.GetSection("StaticFilesUrlPath").Value.Length;
                if (photo.Path != null)
                {
                    var path = Path.Combine(_configuration.GetSection("StaticFilesFolder").Value, photo.Path[lenPrefix..]);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                await _photoRepository.DeletePhotoAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                var msg = "Error occurred while deleting data of database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // GET api/Photos/5
        [HttpGet("ByLabels")]
        public async Task<ActionResult<IEnumerable<DTOs.Photo>>> GetPhotosByLabelsAsync([FromQuery] int[] ids)
        {
            try
            {
                if (ids == null || ids.Count() == 0)
                    return BadRequest($"Label ids should not be null.");
                // Get first label
                var firstLabel = await _labelRepository.GetLabelWithPhotosAsync(ids[0]);
                var photoIdsOfFirstLabel = firstLabel.PhotoLabels.Select(pl => pl.PhotoId);
                // Find photos
                var photos = new List<Models.Photo>();
                foreach (var pid in photoIdsOfFirstLabel)
                {
                    var photo = await _photoRepository.GetPhotoAsync(pid);
                    var labelIds = photo.PhotoLabels.Select(pl => pl.LabelId);
                    if (labelIds.Intersect(ids).Count() == ids.Count())
                        photos.Add(photo);
                }
                // Specfic filter - visitor get photos ==> return visitor_admin's photo
                var (userId, userRole) = GetCurrentUserInfo();
                if (userRole != Models.Role.Admin)
                {
                    var currentUser = await _userRepository.GetUserAsync(userId);
                    var relatedAdminUser = await _userRepository.GetUserAsync($"{currentUser.Username}_admin");
                    photos = photos.Where(p => p.UserId == relatedAdminUser.Id).ToList();
                }

                var photosDTO = photos.Select(photo => _mapper.Map<DTOs.Photo>(photo));
                return Ok(photosDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while retrieving data from database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // POST api/Photos/Upload/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPost("Upload/{id}"), DisableRequestSizeLimit]
        public async Task<ActionResult<DTOs.Photo>> UploadAsync(int id)
        {
            try
            {
                var photo = await _photoRepository.GetPhotoAsync(id);
                if (photo == null)
                    return NotFound($"Photo with id '{id}' not found.");

                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var pathToSave = _configuration.GetSection("StaticFilesFolder").Value;

                if (!CheckPhotoExtension(file.FileName))
                    return BadRequest($"File is not a image.");

                if (file.Length > 0)
                {
                    // Try delete old file on disk
                    var lenPrefix = _configuration.GetSection("StaticFilesUrlPath").Value.Length;
                    if (photo.Path != null)
                    {
                        var path = Path.Combine(pathToSave, photo.Path[lenPrefix..]);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                    // Copy file
                    var fileName = $"{id}_{ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')}";
                    using (var stream = new FileStream(Path.Combine(pathToSave, fileName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    // Set new path
                    photo.Path = Path.Combine(_configuration.GetSection("StaticFilesUrlPath").Value[1..], fileName);
                    var photoUpdated = await _photoRepository.UpdatePhotoAsync(photo, false);
                    var photoDTO = _mapper.Map<DTOs.Photo>(photoUpdated);
                    return Ok(photoDTO);
                }
                else
                {
                    return BadRequest($"File length is less than 0.");
                }
            }
            catch (Exception)
            {
                var msg = "Error occurred while uploading image to server.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // POST api/Photos/MultiUpload
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPost("MultiUpload"), DisableRequestSizeLimit]
        public async Task<ActionResult<IEnumerable<DTOs.Photo>>> MultiUploadAsync()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var files = formCollection.Files;
                var pathToSave = _configuration.GetSection("StaticFilesFolder").Value;

                var photos = new List<Models.Photo>();
                foreach (var file in files)
                {
                    if (file.Length == 0)
                        continue;
                    if (!CheckPhotoExtension(file.FileName))
                        continue;

                    // Create new photo
                    var (userId, userRole) = GetCurrentUserInfo();
                    var photo = await _photoRepository.AddPhotoAsync(new Models.Photo() { UserId = userId});
                    // Save photo on disk
                    var fileName = $"{photo.Id}_{ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')}";
                    using (var stream = new FileStream(Path.Combine(pathToSave, fileName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    // Update path
                    photo.Path = Path.Combine(_configuration.GetSection("StaticFilesUrlPath").Value[1..], fileName);
                    var photoUpdated = await _photoRepository.UpdatePhotoAsync(photo, false);
                    photos.Add(photoUpdated);
                }
                var photosDTO = photos.Select(photo => _mapper.Map<DTOs.Photo>(photo));
                return Ok(photosDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while uploading image to server.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        private bool CheckPhotoExtension(string fileName)
        {
            var extensions = new string[] { ".tif", ".tiff", ".bmp", ".jpg", ".jpeg", ".gif", ".png" };
            var extension = Path.GetExtension(fileName);
            return extensions.Contains(extension);
        }

        [Obsolete]
        private IEnumerable<Models.Photo> FindPhotosContainsLabels(IEnumerable<Models.Photo> photos, IEnumerable<int> ids)
        {
            foreach (var photo in photos)
            {
                var labelIds = photo.PhotoLabels.Select(pl => pl.LabelId);
                if (labelIds.Intersect(ids).Any())
                    yield return photo;
            }
        }

        private (int, string) GetCurrentUserInfo()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name).Value);
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            return (userId, userRole);
        }
    }
}
