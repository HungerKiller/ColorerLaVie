using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoMasterBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoMasterBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILogger<LabelsController> _logger;
        private readonly IMapper _mapper;
        private readonly ILabelRepository _labelRepository;

        public LabelsController(ILogger<LabelsController> logger, IMapper mapper, ILabelRepository labelRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _labelRepository = labelRepository;
        }

        // GET: api/Labels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOs.Label>>> GetsAsync()
        {
            try
            {
                var labels = await _labelRepository.GetLabelsAsync();
                var labelsDTO = labels.Select(label => _mapper.Map<DTOs.Label>(label));
                return Ok(labelsDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while retrieving data from database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // GET api/Labels/5
        [HttpGet("{id}", Name = "GetLabel")]
        public async Task<ActionResult<DTOs.Label>> GetAsync(int id)
        {
            try
            {
                var label = await _labelRepository.GetLabelAsync(id);
                var labelDTO = _mapper.Map<DTOs.Label>(label);
                return Ok(labelDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while retrieving data from database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // POST api/Labels
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<DTOs.Label>> PostAsync([FromBody] DTOs.Label label)
        {
            try
            {
                if (label == null)
                    return BadRequest($"Label object from body is null.");

                // Validate label
                var (statusCode, msg) = await ValidateLabel(label, isCreation: true);
                if (statusCode == StatusCodes.Status400BadRequest)
                {
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status400BadRequest, msg);
                }

                // Create Label
                var createdLabel = await _labelRepository.AddLabelAsync(_mapper.Map<Models.Label>(label));
                var labelDTO = _mapper.Map<DTOs.Label>(createdLabel);
                return CreatedAtRoute("GetLabel", new { id = labelDTO.Id }, labelDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while creating new label into database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // PUT api/Labels/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<DTOs.Label>> PutAsync(int id, [FromBody] DTOs.Label label)
        {
            try
            {
                if (id != label.Id)
                    return BadRequest($"Label id from url and body are not identical.");

                if (await _labelRepository.GetLabelAsync(id) == null)
                    return NotFound($"Label with id '{id}' not found.");

                // Validate label
                var (statusCode, msg) = await ValidateLabel(label, isCreation: false);
                if (statusCode == StatusCodes.Status400BadRequest)
                {
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status400BadRequest, msg);
                }

                var labelUpdated = await _labelRepository.UpdateLabelAsync(_mapper.Map<Models.Label>(label));
                var labelDTO = _mapper.Map<DTOs.Label>(labelUpdated);
                return Ok(labelDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while updating data of database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // DELETE api/Labels/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var label = await _labelRepository.GetLabelWithPhotosAsync(id);
                if (label == null)
                    return NotFound($"Label with id '{id}' not found.");
                // Find all photos who use this label
                if (label.PhotoLabels != null && label.PhotoLabels.Count() != 0)
                    return BadRequest($"Can not remove this label, cause it is in use.");
                await _labelRepository.DeleteLabelAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                var msg = "Error occurred while deleting data of database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        private async Task<(int, string)> ValidateLabel(DTOs.Label label, bool isCreation)
        {
            // Label could be neither null nor whitespace
            if (string.IsNullOrWhiteSpace(label.Name))
            {
                return (StatusCodes.Status400BadRequest, $"Label could be neither null nor whitespace.");
            }

            // Check label existence
            var labels = await _labelRepository.GetLabelsAsync();
            var labelRetrieved = labels.FirstOrDefault(l => l.Name == label.Name);

            if (labelRetrieved == null) return (200, null);

            if (isCreation)
            {
                return (StatusCodes.Status400BadRequest, $"Label '{label.Name}' already exists, cannot create.");
            }
            else
            {
                if (labelRetrieved.Id != label.Id)
                    return (StatusCodes.Status400BadRequest, $"Label '{label.Name}' already exists, cannot update.");
                else
                    return (200, null);
            }
        }
    }
}
