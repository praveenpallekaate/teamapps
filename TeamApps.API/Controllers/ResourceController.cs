using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Resource endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceManagement _resourceManagement = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceController"/> class.
        /// </summary>
        /// <param name="resourceManagement"></param>
        public ResourceController(IResourceManagement resourceManagement)
        {
            _resourceManagement = resourceManagement;
        }

        /// <summary>
        /// Fetch application details
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

            var result = await _resourceManagement
                .GetItemDetailsAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Fetch resource filters
        /// </summary>
        /// <returns></returns>
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilters()
        {
            var result = await _resourceManagement
                .GetFiltersAsync();

            return Ok(result);
        }

        /// <summary>
        /// Fetch application details
        /// </summary>
        /// <param name="resourceDetail">Resource filter details</param>
        /// <returns>Application details</returns>
        [HttpPost("teamresources")]
        public async Task<IActionResult> GetForTeam(ResourceDetail resourceDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _resourceManagement
                .GetResourcesForTeamAsync(resourceDetail);

            return Ok(result);
        }

        /// <summary>
        /// Add resource
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns>Result</returns>
        [HttpPost]
        public async Task<IActionResult> AddResourceAsync(ResourceDetail resourceDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _resourceManagement
                .InsertItemAsync(resourceDetail);

            return Ok(result);
        }

        /// <summary>
        /// Update resource
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns>Result</returns>
        [HttpPut("updateresource")]
        public async Task<IActionResult> UpdateResourceAsync(ResourceDetail resourceDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _resourceManagement
                .UpdateItemAsync(resourceDetail);

            return Ok(result);
        }
    }
}
