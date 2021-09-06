using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamApps.Shared;

namespace TeamApps.API.Controllers
{
    /// <summary>
    /// Application endpoint
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationManagement _applicationManagement = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController"/> class.
        /// ctor
        /// </summary>
        /// <param name="applicationManagement"></param>
        public ApplicationController(IApplicationManagement applicationManagement)
        {
            _applicationManagement = applicationManagement;
        }

        /// <summary>
        /// Get application details
        /// </summary>
        /// <param name="id">Application id</param>
        /// <returns>Application details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _applicationManagement
                .GetItemDetailsAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Fetch teams
        /// </summary>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <param name="userId"
        /// <returns>Team details collection</returns>
        [HttpGet("{skip}/{limit}/{userId}")]
        public async Task<IActionResult> GetTeamsAsync(int skip, int limit, int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ApplicationDetail applicationDetail = new ApplicationDetail { UpdatedBy = userId };

            var result = await _applicationManagement
                .GetTeamDetailsAsync(skip, limit, applicationDetail);

            return Ok(result);
        }

        /// <summary>
        /// Fetch all teams
        /// </summary>
        /// <returns>Keyvalue collection</returns>
        [HttpGet("allteams")]
        public async Task<IActionResult> GetAllTeamsAsync()
        {
            var result = await _applicationManagement
                .GetAllTeamsAsync();

            return Ok(result);
        }

        /// <summary>
        /// Add application
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        [HttpPost]
        public async Task<IActionResult> AddApplicationAsync(ApplicationDetail applicationDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _applicationManagement
                .InsertItemAsync(applicationDetail);

            return Ok(result);
        }

        /// <summary>
        /// Add Team
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        [HttpPost("addteam")]
        public async Task<IActionResult> AddTeamAsync(ApplicationDetail applicationDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _applicationManagement
                .InsertTeamAsync(applicationDetail);

            return Ok(result);
        }

        /// <summary>
        /// Update application
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        [HttpPut("updateapplication")]
        public async Task<IActionResult> UpdateApplicationAsync(ApplicationDetail applicationDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _applicationManagement
                .UpdateItemAsync(applicationDetail);

            return Ok(result);
        }
    }
}
