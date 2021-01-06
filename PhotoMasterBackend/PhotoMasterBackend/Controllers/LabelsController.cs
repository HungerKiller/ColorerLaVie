using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoMasterBackend.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoMasterBackend.Controllers
{
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
        public async Task<ActionResult> GetsAsync()
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
        public async Task<ActionResult> GetAsync(int id)
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
        [HttpPost]
        public async Task<ActionResult<DTOs.Label>> PostAsync([FromBody] DTOs.Label label)
        {
            try
            {
                if (label == null)
                    return BadRequest();

                // Validate label
                var (statusCode, msg) = await ValidateLabel(label);
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
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] DTOs.Label label)
        {
            try
            {
                if (id != label.Id)
                {
                    return BadRequest();
                }

                // Validate label
                var (statusCode, msg) = await ValidateLabel(label);
                if(statusCode == StatusCodes.Status400BadRequest)
                {
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status400BadRequest, msg);
                }

                var labelUpdated = await _labelRepository.UpdateLabelAsync(_mapper.Map<Models.Label>(label));
                if (labelUpdated == null)
                    return NotFound();

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
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

        private async Task<(int, string)> ValidateLabel(DTOs.Label label)
        {
            // Check label existence
            var labels = await _labelRepository.GetLabelsAsync();
            if (labels.Any(l => l.Name == label.Name))
            {
                return (StatusCodes.Status400BadRequest, $"Label '{label.Name}' already exists.");
            }
            // Label should not contain character whitespace
            if (label.Name.Contains(' '))
            {
                return (StatusCodes.Status400BadRequest, $"Label should not contain character whitespace.");
            }
            // Label length should be greater than 5 characters
            if (label.Name.Length <= 5)
            {
                return (StatusCodes.Status400BadRequest, $"Label's length should be greater than 5 characters.");
            }

            return (200, null);
        }
    }
}
