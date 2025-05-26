using NexaShopify.Core.Common.Models;
using NexaShopify.Core.SharedKernel.Interfaces;
using NexaShopify.Core.Shop.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Store.Handlers.StoreManagement.Shop
{
    public class GetAllShopHandler : IHandle<Identity.Models.UserModel, ResponseModel<List<ShopModel>>>
    {

        private Identity.Models.UserModel _user;

        public GetAllShopHandler(Identity.Models.UserModel user)
        {
            _user = user;
        }
        public ResponseModel<List<ShopModel>> Handle()
        {

            try
            {
                var validationResponse = this.Validate();

                if (!validationResponse.Success)
                {
                    return validationResponse;
                }
                // Get All Shops from DataBase 
             //   var storeList = Infrastructure.Data.Access.Tables.Store.ShopAccess.Get();

                var response = new List<ShopModel>();

                //    response = storeList.Select(x => new ShopModel(x)).ToList();

                return null;

                return  ResponseModel<List<ShopModel>>.SuccessResponse(response);
            }
            catch (Exception ex) {
                Infrastructure.Services.Logging.Logger.Log(ex);
                throw;
              
            }

        }

        public ResponseModel<List<ShopModel>> Validate()
        {
            // Logic here 
            if (this._user == null /*|| this._user.Roles.*/ )
            {
                ResponseModel<List<ShopModel>>.AccessDeniedResponse();
            }

           // if (ShopAccess.Get() == null)
             //   return ResponseModel<List<ShopModel>>.FailureResponse("Shops not found");

            return ResponseModel<List<ShopModel>>.SuccessResponse();
        }
    }
}
