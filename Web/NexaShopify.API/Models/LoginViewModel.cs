using System.ComponentModel.DataAnnotations;

namespace NexaShopify.API.Models.Authentication
{
    /** Adda -> : by default we have this NameSpace << NexaShopify.API.Models >> I added << Authentication >>
     * to the NameSpace in order to indicate that class refer the Authentication Controlleur **/
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
