using NexaShopify.Core.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Identity.Models
{
    #region>> UserModel comment
    //public class UserModel
    //{
    //    public int Id { get; set; }
    //    public string Username { get; set; }
    //    public string Email { get; set; }
    //    public string Name { get; set; }
    //    public DateTime CreationTime { get; set; }
    //    public string SelectedLanguage { get; set; } = "fr";
    //    public string Telephone { get; set; }

    //    // Gestion des rôles
    //    public UserRole Role { get; set; } = UserRole.Customer;

    //    // Intégration avec Shopify
    //    public string ShopifyCustomerId { get; set; }

    //    // Vérification du compte
    //    public bool IsVerified { get; set; } = false;

    //    // Adresse (utile pour la livraison des clients)
    //    public string Address { get; set; }

    //    // Accès et permissions
    //    public AccessProfileModel Access { get; set; }

    //    // Entreprise (utile pour les vendeurs B2B)
    //    //public int? CompanyId { get; set; }
    //    //public string CompanyName { get; set; }

    //    public UserModel() { }

    //    public UserModel(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
    //        Models.AccessProfileModel accessProfile)
    //    {

    //        Id = userEntity.Id;
    //        Username = userEntity.Username;
    //        CreationTime = userEntity.CreationTime;
    //        Name = userEntity.Name;
    //        SelectedLanguage = userEntity.SelectedLanguage ?? "fr";
    //        Email = userEntity.Email;
    //        Telephone = $"{userEntity.TelephoneHome} {userEntity.TelephoneMobile} {userEntity.TelephoneIP}".Trim();

    //        // Gestion des accès et de l'entreprise
    //        Access = accessProfile;
    //        // Ajout des nouveaux champs
    //        Role = userEntity.IsAdmin ? UserRole.Admin : UserRole.Customer;
    //        ShopifyCustomerId = userEntity.ShopifyCustomerId;
    //        IsVerified = userEntity.IsVerified;
    //        Address = userEntity.Address;
    //    }


    //}

    //// Enumération pour les rôles
    //public enum UserRole
    //{
    //    Admin,
    //    Vendor,
    //    Customer
    //}

    /// <summary>
    ///  attribut complementaire 
    /// </summary>
    //public string PasswordHash { get; set; }
    //public int ShopId { get; set; } = 0;
    //public string ShopName { get; set; } = string.Empty;
    //public int Id_Role { get; set; } = (int)Enums.BaseDataEnums.Roles.Customer;
    //public string RoleName { get; set; } = Enums.BaseDataEnums.Roles.Customer.GetDescription().ToLower().ToLower();
    #endregion



    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool? SuperAdministrator { get; set; } =false;
        
        public UserRoleModel Roles { get; set; }    
        public bool? VerifiedSeller { get; set; } = false;

        public DateTime CreatedAt  { get; set; } =DateTime.UtcNow;
        public DateTime? LastLogin { get; set; } =DateTime.UtcNow; 
        public bool? IsActive { get; set; } = true; // only for a seller


        public UserModel()
        {
                
        }

        public UserModel(Infrastructure.Data.Entities.Tables.COR.UsersEntity UserEntity, Models.UserRoleModel userRole)
        {
            this.Id = (int)UserEntity.Id;
            this.Username = UserEntity.Username;
            this.Email = UserEntity.Email;
            this.Mobile = UserEntity.Mobile;
            this.SuperAdministrator = UserEntity.SuperAdministrator;
            this.Roles = userRole;
            this.VerifiedSeller = UserEntity.VerifiedSeller;
           // this.CreatedAt = UserEntity.CreatedAt;  
            this.LastLogin = UserEntity.LastLogin;
            this.IsActive = UserEntity.IsActive;
        }


        public bool HasPermission(string permissionName)
        {
            return Roles?.HasPermission(permissionName) ?? false;
            // Ou pour multi-rôles:
            // return Roles.Any(r => r.HasPermission(permissionName));
        }

    }
}


