using NexaShopify.Core.Common.Models;
using NexaShopify.Core.SharedKernel.Interfaces;
using NexaShopify.Core.Shop.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Shop.Handlers.StoreManagement.Shop
{
    public class GetAllShopHandler : IHandle<ResponseModel<List<ShopModel>>>
    {
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
                var storeList = Infrastructure.Data.Access.Tables.Shop.ShopAccess.Get();

                var response = new List<ShopModel>();

               response = storeList.Select(x => new ShopModel(x)).ToList();



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
            if (Infrastructure.Data.Access.Tables.Shop.ShopAccess.Get() == null)
                return ResponseModel<List<ShopModel>>.FailureResponse("Shops not found");

            return ResponseModel<List<ShopModel>>.SuccessResponse();
        }
    }
}
