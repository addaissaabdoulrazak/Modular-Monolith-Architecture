using NexaShopify.Core.Common.Models;
using NexaShopify.Core.SharedKernel.Interfaces;

namespace NexaShopify.Core.Shop.Handlers.StoreManagement.Product
{
    public class CreateProductHandler : IHandle<ResponseModel<int>>
    {
        public ResponseModel<int> Handle()
        {


            try
            {
                var validationResponse = this.Validate();

                if (!validationResponse.Success)
                {
                    return validationResponse;
                }


                return  ResponseModel<int>.SuccessResponse(/*InsertedLand*/ 1);
            }
            catch (Exception ex)
            {
              Infrastructure.Services.Logging.Logger.Log( ex );
                
                throw;
            }

        }

        public ResponseModel<int> Validate()
        {
            // Logic here 

            return ResponseModel<int>.SuccessResponse();
        }
    }
}
