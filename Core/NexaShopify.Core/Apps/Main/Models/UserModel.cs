using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Apps.Main.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }
        public string SelectedLanguage { get; set; }
        public Core.Identity.Models.UserRoleModel Role { get; set; }
    }
}
