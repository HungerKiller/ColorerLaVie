using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoMasterBackend.Repositories;
using System;
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
        public async Task<ActionResult> GetsAsync()
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
        public async Task<ActionResult> GetAsync(int id)
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
        [HttpPost]
        public async Task<ActionResult<DTOs.Photo>> PostAsync([FromBody] DTOs.Photo photo)
        {
            try
            {
                if (photo == null)
                    return BadRequest();

                var createdPhoto = await _photoRepository.AddPhotoAsync(_mapper.Map<Models.Photo>(photo));
                var photoDTO = _mapper.Map<DTOs.Photo>(createdPhoto);
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
        public async Task<ActionResult> PutAsync(int id, [FromBody] DTOs.Photo photo)
        {
            try
            {
                if (id != photo.Id)
                {
                    return BadRequest();
                }
                var photoUpdated = await _photoRepository.UpdatePhotoAsync(_mapper.Map<Models.Photo>(photo));

                if (photoUpdated == null)
                    return NotFound();

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
    }
}
