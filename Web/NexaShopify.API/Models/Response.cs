namespace NexaShopify.API.Models
{
    public class Response<T>
    {
        public Boolean Success { get; set; }
        public T ResponseBody { get; set; }
        public List<String> Errors { get; set; }

        public Response() { }
        public Response(Boolean success, T responseBody, List<string> error = null)
        {
            this.Success = success;
            this.ResponseBody = responseBody;
            this.Errors = error;
        }
    }
}
