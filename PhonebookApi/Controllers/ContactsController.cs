using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhonebookApi.Dto.Requests;
using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhonebookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IAuthenticatedUser _authUserService;
        private readonly IContactRepository _contactRepository;
        private readonly IContactResponseMapper _contactResponseMapper;
        private readonly IContactInDetailResponseMapper _contactDetailedResponseMapper;
        private readonly IContactRequestMapper _contactRequestMapper;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IApplicationUserResponseMapper _userResponseMapper;
        private readonly IMessageResponseMapper _messageResponseMapper;


        public ContactsController(
            ILogger<ContactsController> logger,
            IAuthenticatedUser authUserService,
            IContactRepository contactRepository,
            IContactResponseMapper contactResponseMapper,
            IContactInDetailResponseMapper contactDetailedResponseMapper,
            IContactRequestMapper contactRequestMapper,
            IApplicationUserRepository userRepository,
            IApplicationUserResponseMapper userResponseMapper,
            IMessageResponseMapper messageResponseMapper
            )
        {
            _logger = logger;
            _authUserService = authUserService;
            _contactRepository = contactRepository;
            _contactResponseMapper = contactResponseMapper;
            _contactDetailedResponseMapper = contactDetailedResponseMapper;
            _contactRequestMapper = contactRequestMapper;
            _userRepository = userRepository;
            _userResponseMapper = userResponseMapper;
            _messageResponseMapper = messageResponseMapper;

        }

        /// <summary>
        /// Get a collection of authenticated users contacts  
        /// </summary>
        /// <returns>A collection of authenticated users contacts</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ContactResponse>>> Get()
        {
            try
            {
                var userContacts = await _contactRepository.GetUsersContacts(_authUserService.AuthenticatedUserId);
                return Ok(_contactResponseMapper.MapList(userContacts));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Search authenticated users contacts by name 
        /// </summary>
        /// <returns>A collection of authenticated users contacts matching search</returns>
        [HttpGet]
        [Route("Search")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ContactResponse>>> Search([FromQuery] string search)
        {
            try
            {
                if (search == String.Empty || search == null)
                {
                    return BadRequest(_messageResponseMapper.Map("Search field is required"));
                }
                var userContacts = await _contactRepository.SearchUsersContacts(search, _authUserService.AuthenticatedUserId);
                return Ok(_contactResponseMapper.MapList(userContacts));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Get a collection of authenticated users owned contacts  
        /// </summary>
        /// <returns>A collection of authenticated users owned contacts</returns>
        [HttpGet]
        [Route("Owned")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]

        public async Task<ActionResult<IEnumerable<ContactResponse>>> GetOwned()
        {
            try
            {
                var ownedContacts = await _contactRepository.GetUsersOwnedContacts(_authUserService.AuthenticatedUserId);
                return Ok(_contactResponseMapper.MapList(ownedContacts));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Gets a single contact by id
        /// </summary>
        /// <param name="key">Contact id</param>
        /// <returns>Single contact</returns>
        [HttpGet]
        [Route("{key:int}")]
        [Authorize(Policy = "IsContactUser")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ContactInDetailResponse>> GetOne(int key)
        {
            try
            {
                if (!await _contactRepository.Exists(key))
                    return NotFound();

                var contact = await _contactRepository.GetOneById(key);
                return Ok(_contactDetailedResponseMapper.Map(contact));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="request">Contact request data</param>
        /// <returns>Created Contact</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ContactInDetailResponse>> Create([FromBody] ContactRequest request)
        {
            try
            {
                var newContact = _contactRequestMapper.Map(request, _authUserService.AuthenticatedUserId);
                var contact = await _contactRepository.Create(newContact);

                return CreatedAtAction(
                    nameof(GetOne), 
                    new { key = contact.Id },
                    _contactDetailedResponseMapper.Map(contact));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }

        }

        /// <summary>
        /// Update Existing Contact Data
        /// </summary>
        /// <param name="key">Contact id</param>
        /// <param name="request">Conatct request data</param>
        /// <returns>Updated Contact</returns>
        [HttpPut("{key:int}")]
        [Authorize(Policy ="IsContactOwner")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ContactInDetailResponse>> Update(
            [FromRoute] int key, [FromBody] ContactRequest request)
        {
            try
            {
                if (!await _contactRepository.Exists(key))
                    return NotFound();

                var contact = _contactRequestMapper.Map(request, _authUserService.AuthenticatedUserId);
                contact = await _contactRepository.Update(key, contact);

                return Ok(_contactDetailedResponseMapper.Map(contact));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="key">Contact id</param>
        /// <returns>Void</returns>
        [HttpDelete("{key:int}")]
        [Authorize(Policy = "IsContactOwner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int key)
        {
            try
            {
                if (!await _contactRepository.Exists(key))
                    return NotFound();

                if (!await _contactRepository.Delete(key)) {
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

        /// <summary>
        /// Enables specific Contact Sharing With Specific User
        /// </summary>
        /// <param name="key">Contact Id</param>
        /// <param name="userId">The id of the user with whom to share the contact</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{key:int}/Sharing/{userId:int}")]
        [Authorize(Policy = "IsContactOwner")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<MessageResponse>> EnableContactSharing([FromRoute] int key, int userId)
        {
            try
            {
                if (!await _contactRepository.Exists(key))
                    return NotFound(_messageResponseMapper.Map("This contact does not exist"));

                if (!await _userRepository.Exists(userId))
                {
                    return BadRequest(_messageResponseMapper.Map("This user does not exist"));
                }

                if (await _contactRepository.ExistsInUsersContacts(key, userId))
                {
                    return BadRequest(_messageResponseMapper.Map("User already has this contact"));
                }
                if (await _contactRepository.AddContactUser(key, userId))
                {
                    return Ok(_messageResponseMapper.Map("Contact sharing enabled"));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Contact sharing failed");

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Disables Specific Contact Sharing With Specific User
        /// </summary>
        /// <param name="key">Contact Id</param>
        /// <param name="userId">The id of the user with whom to stop sharing the contact</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{key:int}/Sharing/{userId:int}")]
        [Authorize(Policy = "IsContactOwner")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<MessageResponse>> DisableContactSharing([FromRoute] int key, int userId)
            {
            try
            {
                if (!await _contactRepository.Exists(key))
                    return NotFound(_messageResponseMapper.Map("This contact does not exist"));

                if (!await _userRepository.Exists(userId))
                {
                    return NotFound(_messageResponseMapper.Map("This user does not exist"));
                }

                if (!await _contactRepository.ExistsInUsersContacts(key, userId))
                {
                    return BadRequest(_messageResponseMapper.Map("You are not sharing this contact with this user"));
                }
                if (await _contactRepository.RemoveContactUser(key, userId))
                {
                    return Ok(_messageResponseMapper.Map("Contact sharing stopped"));
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Contact sharing disabling failed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Gets users connected to specific contact
        /// </summary>
        /// <param name="key">Contact Id</param>
        /// <returns>A collection of users connected to specific contact</returns>
        [HttpGet("{key:int}/Users")]
        [Authorize(Policy = "IsContactOwner")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<ApplicationUserResponse>>> GetContactUsers(int key)
        {
            try
            {
                if (!await _contactRepository.Exists(key))
                    return NotFound();

                var contactUsers = await _contactRepository.GetContactUsers(key);
                return Ok(_userResponseMapper.MapList(contactUsers));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Source} - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }
    }
}

