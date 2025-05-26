using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NexaShopify.Core.Apps.Main.Handlers.Security
{
    public class TokensManager
    {
        #region > Security Key
        //private static SymmetricSecurityKey _securityKey = null;
        //public static void SetSecret(string secret)
        //{
        //    if (string.IsNullOrEmpty(secret))
        //    {
        //        throw new Exception("secret must have a value");
        //    }

        //    var base64String = Convert.FromBase64String(secret);

        //    _securityKey = new SymmetricSecurityKey(base64String);
        //}
       
        private  static SymmetricSecurityKey _securityKey;

        // REM : this method allows me to avoid using DI by constructor
        public static void Initialize(IConfiguration configuration)
        {
            var secretKey = configuration["JwtSettings:TokenSecret"];


            // 2. Validez la clé AVANT de l'utiliser
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("JWT Secret non configuré");


            // 3
            var secretBytes = Convert.FromBase64String(secretKey);
            _securityKey = new SymmetricSecurityKey(secretBytes);

            //ValidateKey();
        }

        public static SymmetricSecurityKey GetSecurityKey() => _securityKey;
        private static void ValidateKey()
        {
            if (_securityKey.KeySize < 512)
                throw new ArgumentException("Clé JWT insuffisante (min 512 bits)");
        }

        #endregion

        public static string GenerateToken(Models.UserModel user)
        {
            

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "Issuer", //_configuration["Jwt:Issuer"],
                Audience = "Audience", //_configuration["Jwt:Issuer"]
                SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddHours(+24),
                //Expires = DateTime.UtcNow.AddMinutes(+1),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("MembershipId", "user-" + user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("Username", (user.Username)),
                })
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(descriptor);

            return tokenHandler.WriteToken(token);
        }



        public static string ValidateToken(string token)
        {
            string username = null;

            var principal = GetPrincipal(token);
            if (principal == null)
            {
                return null;
            }

            ClaimsIdentity identity = null;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim usernameClaim = identity.FindFirst("Username");
            username = usernameClaim.Value;

            return username;
        }

        public static Models.UserModel GetUserByAuthorizationHeader(string authorizationHeader)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    Infrastructure.Services.Logging.Logger.Log("string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith('Bearer ')");
                    return null;
                }

                var token = authorizationHeader.Substring(7, authorizationHeader.Length - 7);
                if (string.IsNullOrEmpty(token))
                {
                    Infrastructure.Services.Logging.Logger.Log("token == null");
                    return null;
                }

                return getUserByToken(token);
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }
        public static Models.UserModel getUserByToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    Infrastructure.Services.Logging.Logger.Log("jwtToken == null");
                    return null;
                }

                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = _securityKey,
                    ValidateLifetime = false,
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

                if (principal == null || principal.Claims == null)
                {
                    Infrastructure.Services.Logging.Logger.Log("principal == null || principal.Claims == null");
                    return null;
                }

                var idStr = principal.Claims.FirstOrDefault(e => e.Type == "Id");
                if (idStr == null || string.IsNullOrEmpty(idStr.Value))
                {
                    Infrastructure.Services.Logging.Logger.Log("idStr == null || string.IsNullOrEmpty(idStr.Value)");
                    return null;
                }

                var id = -1;
                if (!int.TryParse(idStr.Value, out id))
                {
                    Infrastructure.Services.Logging.Logger.Log("!int.TryParse(idStr.Value, out id)");
                    return null;
                }

                return Handlers.Users.Get(id, true);
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    return null;
                }

                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = _securityKey,
                };

                Microsoft.IdentityModel.Tokens.SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

                return principal;
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }

        public static Models.UserModel GetUser(IHeaderDictionary headers)
        {
            var token = getTokenFromRequesHeader(headers);
            var user = getUserByToken(token);

            return user;
        }

        private static string getTokenFromRequesHeader(IHeaderDictionary headers)
        {
            StringValues value;
            var got = headers.TryGetValue("Authorization", out value);
            value = value.ToString().Substring(7); //remove Bearer
            return value;
        }

        public static string GenerateRefreshToken(string ExpiredToken)
        {
            var user = getUserByToken(ExpiredToken);
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(ExpiredToken);
            if (jwtToken == null)
            {
                Infrastructure.Services.Logging.Logger.Log("jwtToken == null");
                return null;
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "Issuer", //_configuration["Jwt:Issuer"],
                Audience = "Audience", //_configuration["Jwt:Issuer"]
                SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature),
                //Expires = DateTime.UtcNow.AddHours(+24),
                Expires = DateTime.UtcNow.AddMinutes(+1),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("MembershipId", "user-" + user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Name),
                    new Claim("Username", (user.Username)),
                })
            };


            var refreshToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            return tokenHandler.WriteToken(refreshToken);
        }


    }
}
