using Microsoft.AspNetCore.Mvc;
using NexaShopify.Core.Common.Models;
using NexaShopify.Core.Shop.Models.Product;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexaShopify.API.Areas.Shops.Controllers
{
    [Area("StoreManagement")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

                private const string MODULE = "StoreManagement";
        //GET: api/<Product>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { MODULE })]
        [ProducesResponseType(typeof(NexaShopify.Core.Common.Models.ResponseModel<IEnumerable<ProductModel>>), 200)]

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

        // GET api/<Product>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] { MODULE })]
        [ProducesResponseType(typeof(NexaShopify.Core.Common.Models.ResponseModel<ProductModel>), 200)]
        public IActionResult Get(int id)
        {

            try
            {
                var response = new Core.Shop.Handlers.StoreManagement.Product.GetProductHandler(id).Handle();

                return Ok(response);
            }
            catch (Exception e) {

                Infrastructure.Services.Logging.Logger.Log(e);
                throw;  
            }
           

        }

        // POST api/<Product>
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

        // PUT api/<Product>/5
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

        // DELETE api/<Product>/5
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
