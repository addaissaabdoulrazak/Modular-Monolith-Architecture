//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;

//namespace NexaShopify.API.Areas.ManagementOverview.Controllers
//{
//    [Area("ManagementOverview")]
//    [Route("api/[area]/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private const string MODULE = "Management Overview | User";
//        private const string USER = "";
//       // GET: api/<CategoriesController>
//        [HttpGet]
//        [SwaggerOperation(Tags = new[] { $"{MODULE}{USER}" })]
//        [ProducesResponseType(typeof(NexaShopify.Core.Common.Models.ResponseModel<string>), 200)]
//        public IActionResult Get()
//            {
//                try
//                {
//                    return Ok();
//                }
//                catch (Exception e)
//                {
//                    //  _logger.LogError(e, e.Message);
//                    return StatusCode(500, "Unknown exception occurred");
//                }

//            }

//            // GET api/<CategoriesController>/5
//            [HttpGet("{id}")]
//            [SwaggerOperation(Tags = new[] { $"{MODULE}{USER}" })]
//            public IActionResult Get(int id)
//            {
//                try
//                {
//                    return Ok();
//                }
//                catch (Exception e)
//                {
//                    //  _logger.LogError(e, e.Message);
//                    return StatusCode(500, "Unknown exception occurred");
//                }

//            }

//            // POST api/<CategoriesController>
//            [HttpPost]
//        [SwaggerOperation(Tags = new[] { $"{MODULE}{USER}" })]
//        public IActionResult Post([FromBody] string value)
//            {
//                try
//                {
//                    return Ok();
//                }
//                catch (Exception e)
//                {
//                    //  _logger.LogError(e, e.Message);
//                    return StatusCode(500, "Unknown exception occurred");
//                }

//            }

//            // PUT api/<CategoriesController>/5
//            [HttpPut("{id}")]
//            [SwaggerOperation(Tags = new[] { $"{MODULE}{USER}" })]
//            public IActionResult Put(int id, [FromBody] string value)
//            {
//                try
//                {
//                    return Ok();
//                }
//                catch (Exception e)
//                {
//                    //  _logger.LogError(e, e.Message);
//                    return StatusCode(500, "Unknown exception occurred");
//                }

//            }

//            // DELETE api/<CategoriesController>/5
//            [HttpDelete("{id}")]
//            [SwaggerOperation(Tags = new[] { $"{MODULE}{USER}" })]
//            public IActionResult Delete(int id)
//            {
//                try
//                {
//                    return Ok();
//                }
//                catch (Exception e)
//                {
//                    //  _logger.LogError(e, e.Message);
//                    return StatusCode(500, "Unknown exception occurred");
//                }

//            }
        
//    }
//}
