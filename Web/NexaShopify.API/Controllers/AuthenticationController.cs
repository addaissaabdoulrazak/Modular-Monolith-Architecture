using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexaShopify.API.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace NexaShopify.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "No Area" })]
        public IActionResult Login(Models.Authentication.LoginViewModel data)
        {
            try
            {
                var checkResponse = Core.Apps.Main.Handlers.Users.CheckUserCredentials(data.Username, data.Password);
                if (!checkResponse.Success || checkResponse.Body == null)
                {
                    return Ok(new
                    {
                        response = new Models.Response<object>
                        {
                            Errors = checkResponse.Errors,
                        }
                    });
                }

                return Ok(new
                {
                    response = new Models.Response<string>
                    {
                        Success = true,
                        ResponseBody = Core.Apps.Main.Handlers.Security.TokensManager.GenerateToken(checkResponse.Body),
                    }
                });
            }
            catch (Exception e)
            {
                return this.HandleException(e, data);
                //return BadRequest(new { response = new Models.Response<object> { Success = false } });
            }
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "No Area" })]
        public IActionResult Logout()
        {
            return Ok(new { response = new Models.Response<string> { Errors = new List<string>(), ResponseBody = "", Success = true } });
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { "No Area" })]
        [ProducesResponseType(typeof(Core.Identity.Models.UserModel), 200)]
        public IActionResult GetUserData()
        {
            try
            {
                return Ok(this.GetCurrentUser());
            }
            catch (Exception e)
            {
                return this.HandleException(e);
            }
        }
        #region
        //[HttpGet]
        //[SwaggerOperation(Tags = new[] { "No Area" })]
        //[ProducesResponseType(typeof(Core.Identity.Models.UserModel), 200)]
        //public IActionResult GetUserADData()
        //{
        //    try
        //    {
        //        var clientHost = "";
        //        var clientPort = 895;
        //        var clientUsername = "";
        //        var clientPassword = "";
        //        var clientSslEnabled = false;
        //        var emailAdress = "";
        //        var emailDisplayName = "";
        //        var eSender = new Infrastructure.Services.Email.EmailSender(clientHost, clientPort, clientUsername, clientPassword, clientSslEnabled, emailAdress, emailDisplayName);

        //        return Ok(Core.Apps.Settings.Handlers.Users.GetADInfo(0));
        //    }
        //    catch (Exception e)
        //    {
        //        return this.HandleException(e);
        //    }
        //}

        #endregion

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "No Area" })]
        public IActionResult RefreshToken(string token, string Password)
        {
            string DecodedPassword = Base64Decode(Password);

            try
            {
                var user = Core.Apps.Main.Handlers.Security.TokensManager.getUserByToken(token);

                var checkResponse = Core.Apps.Main.Handlers.Users.CheckUserCredentials(user.Username, DecodedPassword);
                if (!checkResponse.Success || checkResponse.Body == null)
                {
                    return Ok(new
                    {
                        response = new Models.Response<object>
                        {
                            Errors = checkResponse.Errors,
                        }
                    });
                }
                else
                {
                    return Ok(new
                    {
                        response = new Models.Response<string>
                        {
                            Success = true,
                            ResponseBody = Core.Apps.Main.Handlers.Security.TokensManager.GenerateRefreshToken(token),
                        }
                    });
                }

            }
            catch (Exception e)
            {
                return this.HandleException(e);
                //return BadRequest(new { response = new Models.Response<object> { Success = false } });
            }

        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
