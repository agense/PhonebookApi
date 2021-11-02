using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhonebookApi.Dto.Requests;
using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Interfaces;
using System;
using System.Threading.Tasks;

namespace PhonebookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthentication _authenticationService;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IAuthenticationResponseMapper _authResponseMapper;
        private readonly IApplicationUserRequestMapper _registrationRequestMapper;


        public AuthController(
            ILogger<AuthController> logger,
            IAuthentication authenticationService,
            IApplicationUserRepository userRepository,
            IAuthenticationResponseMapper authResponseMapper,
            IApplicationUserRequestMapper registrationRequestMapper
        )
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _authResponseMapper = authResponseMapper;
            _registrationRequestMapper = registrationRequestMapper;
        }

        /// <summary>
        /// Create a new application user and authenticate
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Authentication response including user and token</returns>
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<AuthenticationResponse>> Register(
            [FromBody] RegistrationRequest request, 
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions
            ) {
            try
            {
                if (await _userRepository.UserEmailExists(request.Email))
                {
                    ModelState.AddModelError(nameof(request.Email), "This email is already taken.");
                    return BadRequest(apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext));
                }

                var user = _registrationRequestMapper.Map(request);

                var created = await _userRepository.Create(user);
                if (created == null) {
                    throw new Exception("User creation failed.");
                }

                var token = _authenticationService.Authenticate(created);

                return Created(nameof(Login),_authResponseMapper.Map(created, token));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Authentication response including authenticated user and token</returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<AuthenticationResponse>> Login(
            [FromBody] LoginRequest request, 
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions
        )
        {
            try
            {
                if (!await _userRepository.CredentialsValid(request.Email, request.Password))
                {
                    ModelState.AddModelError(nameof(request.Email), "Invalid Credentials");
                    ModelState.AddModelError(nameof(request.Password), "Invalid Credentials");
                    return BadRequest(apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext));
                }
                else
                {
                    var user = await _userRepository.GetOneByEmail(request.Email);

                    var token = _authenticationService.Authenticate(user);

                    return Ok(_authResponseMapper.Map(user, token));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }  
        }

    }
}

