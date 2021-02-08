using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PhotoMasterBackend.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // GET: api/Users
        [Authorize(Roles = Models.Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOs.User>>> GetsAsync()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                var usersDTO = users.Select(user => _mapper.Map<DTOs.User>(user));
                return Ok(usersDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while retrieving data from database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // GET api/Users/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<DTOs.User>> GetAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(id);
                var userDTO = _mapper.Map<DTOs.User>(user);
                return Ok(userDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while retrieving data from database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // POST api/Users
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<DTOs.User>> PostAsync([FromBody] DTOs.User user)
        {
            try
            {
                if (user == null)
                    return BadRequest($"User object from body is null.");

                // Validate user
                var (statusCode, msg) = await ValidateUser(user, isCreation: true);
                if (statusCode == StatusCodes.Status400BadRequest)
                {
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status400BadRequest, msg);
                }

                // Create user
                var createdUser = await _userRepository.AddUserAsync(_mapper.Map<Models.User>(user));
                var userDTO = _mapper.Map<DTOs.User>(createdUser);
                return CreatedAtRoute("GetUser", new { id = userDTO.Id }, userDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while creating new user into database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // PUT api/Users/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<DTOs.User>> PutAsync(int id, [FromBody] DTOs.User user)
        {
            try
            {
                if (id != user.Id)
                    return BadRequest($"User id from url and body are not identical.");

                if (await _userRepository.GetUserAsync(id) == null)
                    return NotFound($"User with id '{id}' not found.");

                // Validate user
                var (statusCode, msg) = await ValidateUser(user, isCreation: false);
                if (statusCode == StatusCodes.Status400BadRequest)
                {
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status400BadRequest, msg);
                }

                var userUpdated = await _userRepository.UpdateUserAsync(_mapper.Map<Models.User>(user));
                var userDTO = _mapper.Map<DTOs.User>(userUpdated);
                return Ok(userDTO);
            }
            catch (Exception)
            {
                var msg = "Error occurred while updating data of database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // DELETE api/Users/5
        [Authorize(Roles = Models.Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(id);
                if (user == null)
                    return NotFound($"User with id '{id}' not found.");
                // todo check user photos
                await _userRepository.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                var msg = "Error occurred while deleting data of database.";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        // GET: api/Users
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> Authenticate([FromBody] DTOs.User user)
        {
            var userRetrieved = await _userRepository.GetUserAsync(user.Username, user.Password);
            if (userRetrieved == null)
                return BadRequest("Username or password is incorrect.");

            // Generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSecret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userRetrieved.Id.ToString()),
                    new Claim(ClaimTypes.Role, userRetrieved.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userRetrieved.Token = tokenHandler.WriteToken(token);
            var userDTO = _mapper.Map<DTOs.User>(userRetrieved);
            return Ok(userDTO);
        }

        private async Task<(int, string)> ValidateUser(DTOs.User user, bool isCreation)
        {
            // Username should be null
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return (StatusCodes.Status400BadRequest, $"Username and password could be neither null nor whitespace.");
            }

            // Validate role
            if (!(user.Role == Models.Role.Admin || user.Role == Models.Role.Friend || user.Role == Models.Role.Visitor))
            {
                return (StatusCodes.Status400BadRequest, $"User role should be 'Admin', 'Friend' or 'Visitor'.");
            }

            // Check user existence
            var users = await _userRepository.GetUsersAsync();
            var userRetrieved = users.FirstOrDefault(l => l.Username == user.Username);
            if (userRetrieved == null) return (200, null);

            if (isCreation)
            {
                return (StatusCodes.Status400BadRequest, $"User '{user.Username}' already exists, cannot create.");
            }
            else
            {
                if (userRetrieved.Id != user.Id)
                    return (StatusCodes.Status400BadRequest, $"User '{user.Username}' already exists, cannot update.");
                else
                    return (200, null);
            }
        }
    }
}
