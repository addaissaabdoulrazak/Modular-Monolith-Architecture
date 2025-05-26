using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Apps.Main.Handlers
{
    public partial class Users
    {
        #region>> explanation by adda issa 

        //if you wand to get an users, so you have to get also his role,
        //in this case, it would be necessary to opt for a model conglomerate << user + role >>
        #endregion
        public static Models.UserModel Get(int id, bool includeAccess)
        {
            try
            {
                return Get(new List<long>() { id }, includeAccess).FirstOrDefault();
                ;
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }


        public static List<Models.UserModel> Get(List<long> ids, bool includeAccess)
        {
            try
            {
                return Get(Infrastructure.Data.Access.Tables.COR.UsersAccess.Get(ids), includeAccess);
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }

        public static List<Models.UserModel> Get(List<Infrastructure.Data.Entities.Tables.COR.UsersEntity> usersDb,
            bool includeAccess)
        {
            try
            {
                if (usersDb == null || usersDb.Count == 0)
                {
                    return new List<Models.UserModel>();
                }

                var response = new List<Models.UserModel>();
                foreach (var userDb in usersDb)
                {
                    var user = new Models.UserModel();

                    user.Id = (int)userDb.Id;
                    user.Username = userDb.Username;
                    user.CreationTime = (DateTime)userDb.CreatedAt;
                    user.SelectedLanguage = userDb.SelectedLanguage;

                    if (includeAccess)
                    {

                        user.Role = Core.Identity.Handlers.UserRoles.GetUserRole(user.Id);
                    }
                    else
                    {
                        user.Role = null;
                    }

                    response.Add(user);
                }

                return response;
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }
    }
}
