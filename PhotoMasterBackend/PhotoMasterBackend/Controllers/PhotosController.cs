using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoMasterBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly ILogger<PhotosController> _logger;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        private readonly ILabelRepository _labelRepository;

        public PhotosController(ILogger<PhotosController> logger, IMapper mapper, IPhotoRepository photoRepository, ILabelRepository labelRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _photoRepository = photoRepository;
            _labelRepository = labelRepository;
        }

        // GET: api/Photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOs.Photo>>> GetsAsync()
        {
            try
            {
                var photos = await _photoRepository.GetPhotosAsync();
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

        // POST api/Photos
        [HttpPost]
        public async Task<ActionResult<DTOs.Photo>> PostAsync([FromBody] DTOs.Photo photo)
        {
            try
            {
                if (photo == null)
                    return BadRequest($"Photo object from body is null.");
                // Create photo
                var createdPhoto = await _photoRepository.AddPhotoAsync(_mapper.Map<Models.Photo>(photo));
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
                foreach (var l in photo.Labels)
                {
                    var label = await _labelRepository.GetLabelAsync(l.Id);
                    if (label == null)
                        return BadRequest($"Label with id '{l.Id}' not found!");
                }

                // Update photo
                var photoUpdated = await _photoRepository.UpdatePhotoAsync(_mapper.Map<Models.Photo>(photo));
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await _photoRepository.GetPhotoAsync(id) == null)
                    return NotFound($"Photo with id '{id}' not found.");

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
    }
}
