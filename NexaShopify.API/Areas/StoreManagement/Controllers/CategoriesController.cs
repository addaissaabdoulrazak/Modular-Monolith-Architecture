using Microsoft.AspNetCore.Mvc;
using NexaShopify.Core.Common.Models;
using NexaShopify.Core.Shop.Models.Category;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexaShopify.API.Areas.Shops.Controllers
{
    [Area("StoreManagement")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private const string MODULE = "StoreManagement";


        // GET: api/<CategoriesController>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { MODULE })]
        [ProducesResponseType(typeof(NexaShopify.Core.Common.Models.ResponseModel<IEnumerable<CategoryModel>>), 200)]
        public IActionResult Get()
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, e.Message);
                Infrastructure.Services.Logging.Logger.Log(e);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] { MODULE })]
        [ProducesResponseType(typeof(NexaShopify.Core.Common.Models.ResponseModel<CategoryModel>), 200)]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // POST api/<CategoriesController>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { MODULE })]
        [ProducesResponseType(typeof(ResponseModel<int>), 200)]
        public IActionResult Post([FromBody] string value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new[] { MODULE })]
        [ProducesResponseType(typeof(ResponseModel<int>), 200)]
        public IActionResult Put(int id, [FromBody] string value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { MODULE })]
        [ProducesResponseType(typeof(ResponseModel<int>), 200)]

        public IActionResult Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                return StatusCode(500, "Unknown exception occurred");
            }

        }
    }
}
