using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhonebookApi.Dto.Requests;
using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Interfaces;
using PhonebookBusiness.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhonebookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IApplicationUserResponseMapper _userResponseMapper;
        private readonly IApplicationUserInDetailResponseMapper _userInDetailResponseMapper;
        private readonly IMessageResponseMapper _messageResponseMapper;

        public UsersController(
            ILogger<UsersController> logger,
            IApplicationUserRepository userRepository,
            IApplicationUserResponseMapper userResponseMapper,
            IApplicationUserInDetailResponseMapper userInDetailResponseMapper,
            IMessageResponseMapper messageResponseMapper
            )
        {
            _logger = logger;
            _userRepository = userRepository;
            _userResponseMapper = userResponseMapper;
            _userInDetailResponseMapper = userInDetailResponseMapper;
            _messageResponseMapper = messageResponseMapper;
        }

        /// <summary>
        /// Get a collection of application users
        /// </summary>
        /// <returns>A collection of application users</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<ApplicationUserResponse>>> Get()
        {
            try
            {
                var users = await _userRepository.GetAll();
                return Ok(_userResponseMapper.MapList(users));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Search applications users by name
        /// </summary>
        /// <returns>A collection of application users matching search</returns>
        [HttpGet]
        [Route("Search")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<ApplicationUserResponse>>> Search([FromQuery] string search)
        {
            try
            {
                if (search == String.Empty || search == null) {
                    return BadRequest(_messageResponseMapper.Map("Search field is required"));
                }
                var users = await _userRepository.Search(search);
                return Ok(_userResponseMapper.MapList(users));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }

        }

        /// <summary>
        /// Gets a single application user
        /// </summary>
        /// <param name="key">User id</param>
        /// <returns>Single user</returns>
        [HttpGet]
        [Route("{key:int}")]
        [Authorize(Policy = "IsAccountOwner")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApplicationUserResponse>> GetOne(int key)
        {
            try
            {
                if (!await _userRepository.Exists(key))
                    return NotFound();

                var userModel = await _userRepository.GetOneById(key);
                return Ok(_userInDetailResponseMapper.Map(userModel));

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Updates a single application users data
        /// </summary>
        /// <param name="key">User id</param>
        /// <returns>Single user</returns>
        [HttpPut]
        [Route("{key:int}")]
        [Authorize(Policy = "IsAccountOwner")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApplicationUserResponse>> Update(
            [FromRoute] int key,
            [FromBody] AccountUpdateRequest request,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions )
        {
            try
            {
                if (!await _userRepository.Exists(key))
                    return NotFound();

                var model = await _userRepository.GetOneById(key);

                if (!ModelState.IsValid)
                {
                    return BadRequest(apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext));
                }

                if (model.Email != request.Email && await _userRepository.UserEmailExists(request.Email))
                {
                    ModelState.AddModelError(nameof(request.Email), "This email is already taken.");
                    return BadRequest(apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext));
                }

                if (!await _userRepository.CredentialsValid(model.Email, request.Password))
                {
                    ModelState.AddModelError(nameof(request.Password), "Invalid Current Password");
                    return BadRequest(apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext));
                }

                model.FirstName = request.FirstName;
                model.LastName = request.LastName;
                model.Email = request.Email;
                model.Password = request.Password;

                await _userRepository.Update(key, model, request.NewPassword);
               
                return Ok(_userInDetailResponseMapper.Map(model));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }


        /// <summary>
        /// Deletes an existing application user
        /// </summary>
        /// <param name="key">User id</param>
        /// <returns>Void</returns>
        [HttpDelete("{key:int}")]
        [Authorize(Policy = "IsAccountOwner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Delete(int key)
        {
            try
            {
                if (!await _userRepository.Exists(key))
                    return NotFound();

                if (!await _userRepository.Delete(key))
                {
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

    }
}


