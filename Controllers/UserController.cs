using Microsoft.AspNetCore.Mvc;
using receive_ID.Interfaces;
using receive_ID.Models;

namespace receive_ID.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IDbService _dbService;

        public UserController(ILogger<UserController> logger, IDbService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> GetUser(int userid)
        {
            try
            {
                var userData = await _dbService.GetUser(userid);

                if (userData != null)
                {
                    _logger.LogInformation("User data retrieved: {@UserData}", userData);
                    return Ok(userData);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser(UserData userData)
        {
            try
            {
                await _dbService.SaveUser(userData);
                _logger.LogInformation("User data saved to database: {@UserData}", userData);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving user data to database");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}