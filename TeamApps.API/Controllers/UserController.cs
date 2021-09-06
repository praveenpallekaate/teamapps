using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamApps.Shared;

namespace TeamApps.API.Controllers
{
    /// <summary>
    /// User endpoint
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManagement _userManagement = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// ctor
        /// </summary>
        /// <param name="userManagement">User service</param>
        public UserController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }

        /// <summary>
        /// Fetch user details
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userManagement
                .GetItemDetailsAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="userDetail">user details</param>
        /// <returns>Result</returns>
        [HttpPost]
        public async Task<IActionResult> Post(UserDetail userDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Check if user exists
            var filter = await _userManagement
                .CheckIfUserExists(userDetail);

            if (filter == null)
            {
                return BadRequest();
            }

            if (filter > 0)
            {
                return Ok(filter);
            }
            else
            {
                var result = await _userManagement
                    .InsertItemAsync(userDetail);

                return Ok(result);
            }
        }
    }
}
