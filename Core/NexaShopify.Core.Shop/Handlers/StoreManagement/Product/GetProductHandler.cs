using NexaShopify.Core.Common.Models;
using NexaShopify.Core.SharedKernel.Interfaces;
using NexaShopify.Core.Shop.Models.Product;


namespace NexaShopify.Core.Shop.Handlers.StoreManagement.Product
{
    public class GetProductHandler : IHandle<ResponseModel<ProductModel>>
    {

        private int _id { get; set; }

        public GetProductHandler(int id)
        {
            this._id = id;  
        }
        public ResponseModel<ProductModel> Handle()
        {

            try
            {
                var validationResponse = this.Validate();

                if (!validationResponse.Success)
                {
                    return validationResponse;
                }

                return  ResponseModel<ProductModel>.SuccessResponse(/*InsertedLand*/ );


            }
            catch (Exception ex)
            {
              Infrastructure.Services.Logging.Logger.Log(ex);
              throw;
            }
           

        }

        public ResponseModel<ProductModel> Validate()
        {
            // 1. Logic here For exampl check if user is connected before to get Product 
            //if (this.user == null) { return ResponseModel<ProductModel>.AccessDeniedResponse()}

            // 2 -try to get item by using Id   ---------------- Check here

            /*if (Infrastructure.Data.Access.Tables.Shop.Products.get(this._id))
                return ResponseModel<ProductModel>.FailureResponse("Product not found");*/

            return ResponseModel<ProductModel>.SuccessResponse();
        }
    }
}

