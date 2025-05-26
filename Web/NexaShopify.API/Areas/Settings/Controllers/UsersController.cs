using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace NexaShopify.API.Areas.Settings.Controllers
{
	[Authorize]
    [Area("Settings")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private const string MODULE = "Settings";
        private const string ACCESS_PROFILE = "";


        // GET: api/<CategoriesController>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { $"{MODULE}" })]
        public IActionResult Get()
        {
            try
            {

                return Ok();
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, e.Message);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] { $"{MODULE}" })]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, e.Message);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // POST api/<CategoriesController>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { $"{MODULE}" })]
        public IActionResult Post([FromBody] string value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, e.Message);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new[] { $"{MODULE}" })]
        public IActionResult Put(int id, [FromBody] string value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, e.Message);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { $"{MODULE}" })]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, e.Message);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

    }
}

