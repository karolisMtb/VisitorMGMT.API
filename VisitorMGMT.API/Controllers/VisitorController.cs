using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using VisitorMGMT.API.BusinessLogic.Interfaces;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        private readonly ILogger<VisitorController> _logger;
        private readonly IVisitorService _visitorService;
        private readonly IProfileService _profileService;
        private readonly IImageService _imageService;
        public VisitorController(
            ILogger<VisitorController> logger, 
            IVisitorService visitorService, 
            IProfileService profileService, 
            IImageService imageService)
        {
            _logger = logger;
            _visitorService = visitorService;
            _profileService = profileService;
            _imageService = imageService;
        }

        /// <summary>
        /// Signs up new visitor
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SignUp([FromForm]VisitorDTO visitorDTO)
        {
            try
            {
                var profileImage = await _imageService.GenerateProfileImageAsync(visitorDTO.ProfileImage);
                Visitor newVisitor = await _visitorService.SignUpNewAsync(visitorDTO);
                await _imageService.UploadProfileImageAsync(profileImage, newVisitor);

                return Ok($"User {visitorDTO.Username} was created successfully.");
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Client side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        /// <summary>
        /// Logs in existing visitor
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromQuery]LoginDTO visitorLogin)
        {
            try
            {
                string visitorToken = await _visitorService.AuthenticateAsync(visitorLogin);

                if(visitorToken == null)
                {
                    return BadRequest("Invalid username or password");
                }

                return Ok(visitorToken);
            }
            catch(FileNotFoundException e)
            {
                _logger.LogError($"Client side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpDelete]
        [Route("Delete")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(Guid visitorId)
        {
            try
            {
                await _visitorService.DeleteAsync(visitorId);
                return Ok();
            }
            catch (FileNotFoundException e)
            {
                _logger.LogError($"Client side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Updates user's first name
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpPut]
        [Route("Name")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Name([FromBody]string name)
        {
            var currentVisitor = GetCurrentUser();

            try
            {
                await _profileService.UpdateNameAsync(currentVisitor, name);
                return Ok();
            }
            catch(ArgumentException e)
            {
                _logger.LogError($"Client side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Updates user's personal identity number
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>        
        [HttpPut]
        [Route("IdentityNumber")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> IdentityNumber([FromBody]int identityNumber)
        {
            var currentVisitor = GetCurrentUser();

            try
            {
                await _profileService.UpdateIdentityNumberAsync(currentVisitor, identityNumber);
                return Ok();
            }
            catch (FileNotFoundException e)
            {
                _logger.LogError($"Client side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Updates user's address
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Server side error</response>
        [HttpPut]
        [Route("Address")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Address([FromBody]AddressDTO addressDTO)
        {
            var currentVisitor = GetCurrentUser();
            try
            {
                await _profileService.UpdateAddressAsync(currentVisitor, addressDTO);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Updates user's phone number
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Server side error</response>
        [HttpPut]
        [Route("PhoneNumber")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> PhoneNumber([FromBody]string phoneNumber)
        {
            var currentVisitor = GetCurrentUser();
            try
            {
                await _profileService.UpdatePhoneNumberAsync(currentVisitor, phoneNumber);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Gets visitor information by id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Server side error</response>
        [HttpGet]
        [Route("VisitorInformation")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> VisitorInformation(Guid id)
        {

            // turiu gauti image byte masyve
            try
            {
                var visitor = await _visitorService.GetVisitorByIdAsync(id);
                return Ok(visitor);
            }
            catch(Exception e)
            {
                _logger.LogError($"Server side error occured: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        private Visitor GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new Visitor
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

    }
}
