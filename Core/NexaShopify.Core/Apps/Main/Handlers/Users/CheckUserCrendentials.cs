using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// Dont forget to add create method inside our DAL (Data Access Layer File ) Adda --> New Idea --> new task 12/2025 ---> TODO

namespace NexaShopify.Core.Apps.Main.Handlers
{
    public partial class Users
    {
        public static Core.Models.ResponseModel<Models.UserModel> CheckUserCredentials(string username, string password)
        {
            try
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(username))
                {
                    errors.Add("username cannot be empty");
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    errors.Add("password cannot be empty");
                }

                if (errors.Count > 0)
                {
                    return new Core.Models.ResponseModel<Models.UserModel>()
                    {
                        Success = false,
                        Errors = errors
                    };
                }
                #region>> Active directory
                //var userAd = Program.ActiveDirectoryManager.CheckUserCrendentials(username, password); // --> Chopp rice
                //if (username.ToLower() == "adminit")
                //{
                //    if (password != Core.Program.ADdwp)
                //    {
                //        return new Core.Models.ResponseModel<Models.UserModel>()
                //        {
                //            Success = false,
                //            Errors = new List<string>()
                //            {
                //                "Wrong Active Directory credentials"
                //            }
                //        };
                //    }
                //}
                //else
                //{
                //    if (userAd == false)
                //    {
                //        Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Wrong Active Directory credentials");
                //        return new Core.Models.ResponseModel<Models.UserModel>()
                //        {
                //            Success = false,
                //            Errors = new List<string>()
                //            {
                //                "Wrong Active Directory credentials"
                //            }
                //        };
                //    }
                //}
                #endregion
                var userDb = Infrastructure.Data.Access.Tables.COR.UsersAccess.GetByUsername(username);
                if (userDb == null)
                {
                    Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "user not found in database");
                    return new Core.Models.ResponseModel<Models.UserModel>()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "User not found"
                        }
                    };
                }


                return new Core.Models.ResponseModel<Models.UserModel>()
                {
                    Success = true,
                    Body = Get((int)userDb.Id, true)
                };
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }
    }
}
