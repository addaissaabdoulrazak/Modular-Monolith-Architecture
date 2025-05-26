using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Identity.Models
{
    /// <summary>
    /// Représente un rôle utilisateur avec ses permissions
    /// </summary>
    public class UserRoleModel
    {
        public int Id { get; set; }

        // Identifiant technique (ex: "seller", "admin")
        public string Name { get; set; }

        // Nom affichable (ex: "Vendeur", "Administrateur")
        public string DisplayName { get; set; }

        // Description du rôle
        public string Description { get; set; }

        // Niveau hiérarchique (optionnel)
        public int Level { get; set; } = 0;

        // === Permissions Spécifiques ===
        public bool CanManageProducts { get; set; }
        public bool CanManageOrders { get; set; }
        public bool CanManageShopSettings { get; set; }
        public bool CanAccessDashboard { get; set; }
        public bool CanInviteStaff { get; set; }

        // === Méthodes Utiles === //

        /// <summary>
        /// Vérifie si le rôle a une permission spécifique
        /// </summary>
        public bool HasPermission(string permissionName)
        {
            return GetType().GetProperty(permissionName)?.GetValue(this) is true;
        }

        /// <summary>
        /// Liste toutes les permissions activées
        /// </summary>
        public List<string> GetActivePermissions()
        {
            return GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(bool) &&
                           p.Name.StartsWith("Can") &&
                           (bool)p.GetValue(this))
                .Select(p => p.Name)
                .ToList();
        }
    }

    /// <summary>
    /// Rôles prédéfinis pour l'application
    /// </summary>
    public static class StandardRoles
    {
        public static UserRoleModel Customer => new UserRoleModel
        {
            Id = 1,
            Name = "customer",
            DisplayName = "Client",
            Description = "Utilisateur standard pouvant effectuer des achats",
            CanAccessDashboard = false
        };

        public static UserRoleModel Seller => new UserRoleModel
        {
            Id = 2,
            Name = "seller",
            DisplayName = "Vendeur",
            Description = "Gère une boutique et ses produits",
            Level = 10,
            CanManageProducts = true,
            CanManageOrders = true,
            CanAccessDashboard = true
        };

        public static UserRoleModel VerifiedSeller => new UserRoleModel
        {
            Id = 3,
            Name = "verified_seller",
            DisplayName = "Vendeur Vérifié",
            Description = "Vendeur avec compte certifié",
            Level = 20,
            CanManageProducts = true,
            CanManageOrders = true,
            CanManageShopSettings = true,
            CanAccessDashboard = true,
            CanInviteStaff = true
        };

        public static UserRoleModel Admin => new UserRoleModel
        {
            Id = 4,
            Name = "admin",
            DisplayName = "Administrateur",
            Description = "Accès complet à la plateforme",
            Level = 100,
            CanManageProducts = true,
            CanManageOrders = true,
            CanManageShopSettings = true,
            CanAccessDashboard = true,
            CanInviteStaff = true
        };

        /// <summary>
        /// Récupère un rôle standard par son nom
        /// </summary>
        public static UserRoleModel GetByName(string name)
        {
            return AllRoles.FirstOrDefault(r => r.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }

        public static List<UserRoleModel> AllRoles => new List<UserRoleModel>
        {
         Customer, Seller, VerifiedSeller, Admin
        };
    }
}

#region>> Comment section

//public class UserRoleModel
//{
//    public int Id { get; set; }

//    // Identifiant technique (ex: "seller", "admin")
//    public string Name { get; set; }

//    // Nom affichable (ex: "Vendeur", "Administrateur")
//    public string DisplayName { get; set; }

//    // Description du rôle
//    public string Description { get; set; }

//    // Niveau hiérarchique (optionnel)
//    public int Level { get; set; } = 0;

//    // === Permissions Spécifiques ===
//    public bool CanManageProducts { get; set; }
//    public bool CanManageOrders { get; set; }
//    public bool CanManageShopSettings { get; set; }
//    public bool CanAccessDashboard { get; set; }
//    public bool CanInviteStaff { get; set; }

//}

#endregion


