using Infrastructure.Services.Logging;
using NexaShopify.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Identity.Handlers
{
    public partial class UserRoles // partial
    {

        public static Common.Models.ResponseModel<Models.UserRoleModel> Get(int id, Models.UserModel user)
        {
            try
            {
                if (user == null
                    || user.Roles?.Id == 0)
                {
                    throw new SharedKernel.Exceptions.UnauthorizedException();
                }

                var UserRole = Get(id);
                if (UserRole == null)
                {
                    throw new SharedKernel.Exceptions.NotFoundException();
                }

                return Common.Models.ResponseModel<Models.UserRoleModel>.SuccessResponse(UserRole);
            }
            catch (Exception e)
            {
                Logger.Log(e);
                throw;
            }
        }

        public static Models.UserRoleModel Get(int id)
        {
            try
            {
                return Get(new List<int>() { id }).FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.Log(e);
                throw;
            }
        }


        public  static UserRoleModel GetUserRole(int roleId)
        {
            // Version 1: Récupération depuis la DB
            var roleEntity = Infrastructure.Data.Access.Tables.COR.UserRolesAccess.Get(roleId);
            if (roleEntity != null)
            {
                return new UserRoleModel
                {
                    Id = roleEntity.Id,
                    Name = roleEntity.Name,
                    DisplayName = roleEntity.DisplayName,
                    Description = roleEntity.Description,  
                    Level = roleEntity.Level,

                };
            }




            // Version 2: Fallback sur les rôles standards
            return StandardRoles.AllRoles.FirstOrDefault(r => r.Id == roleId);
        }


        public static List<Models.UserRoleModel> Get(List<int> ids)
        {
            try
            {
                return Get(Infrastructure.Data.Access.Tables.COR.UserRolesAccess.Get(ids));
            }
            catch (Exception e)
            {
                Logger.Log(e);
                throw;
            }
        }

        #region>>> Shop rice 

        internal static List<Models.UserRoleModel> Get(List<Infrastructure.Data.Entities.Tables.UserRolesEntity> userRoleDb)
        {
            try
            {
                if (userRoleDb == null || userRoleDb.Count == 0)
                {
                    return new List<Models.UserRoleModel>();
                }





                var response = new List<Models.UserRoleModel>();

                foreach (var item_userRoleDb in userRoleDb)
                {
                    var accessProfile = new Models.UserRoleModel()
                    {
                        Id = item_userRoleDb.Id,
                        Name = item_userRoleDb.Name,

                        DisplayName = item_userRoleDb.DisplayName,
                        Description = item_userRoleDb.Description,
                        Level = item_userRoleDb.Level,
                        //CanManageProducts = item_userRoleDb.CanManageProducts,
                        //CanManageOrders = item_userRoleDb.CanManageOrders,
                        //CanManageShopSettings = item_userRoleDb.CanManageShopSettings,
                        //CanAccessDashboard = item_userRoleDb.CanAccessDashboard,
                        //CanInviteStaff = item_userRoleDb.CanInviteStaff,
                    };


                    response.Add(accessProfile);
                }

                return response;
            }
            catch (Exception e)
            {
                Infrastructure.Services.Logging.Logger.Log(e);
                throw;
            }
        }

        #endregion
    }
}
