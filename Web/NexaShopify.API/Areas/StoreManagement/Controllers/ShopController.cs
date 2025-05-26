using Microsoft.AspNetCore.Mvc;
using NexaShopify.API.Extensions;
using NexaShopify.Core.Common.Models;
using NexaShopify.Core.Shop.Models.Shop;
using Swashbuckle.AspNetCore.Annotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexaShopify.API.Areas.Shops.Controllers
{
    [Area("StoreManagement")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        // GET: api/<ShopController>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "StoreManagement" })]
        [ProducesResponseType(typeof(NexaShopify.Core.Common.Models.ResponseModel<IEnumerable<ShopModel>>), 200)]
        public IActionResult Get()
        {
            try
            {
               
               
               // var response = new Core.Shop.Handlers.StoreManagement.Shop.GetAllShopHandler(this.GetCurrentUser()).Handle();
               // return Ok(response);
               return Ok(new ShopModel());  
            }
            catch (Exception e)
            {
                //  _logger.LogError(e, e.Message);
                Infrastructure.Services.Logging.Logger.Log(e);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // GET api/<ShopController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] { "StoreManagement" })]
        [ProducesResponseType(typeof(NexaShopify.Core.Common.Models.ResponseModel<ShopModel>), 200)]
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

        // POST api/<ShopController>
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "StoreManagement" })]
        [ProducesResponseType(typeof(ResponseModel<int>), 200)]
        public IActionResult Post([FromBody] string value)
        {
            try
            {
             ;
                return Ok();
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                return StatusCode(500, "Unknown exception occurred");
            }

        }

        // PUT api/<ShopController>/5
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new[] { "StoreManagement" })]
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

        // DELETE api/<ShopController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { "StoreManagement" })]
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


        //
    }
}
