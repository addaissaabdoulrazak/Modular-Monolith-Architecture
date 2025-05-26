using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Shop.Enums
{
    class StoreEnums
    {
        public enum ShopSubscriptionPlan : int
        {
            [Description("Basique")]
            Basic = 1,

            [Description("Professionnel")]
            Pro = 2,

            [Description("Entreprise")]
            Enterprise = 3
        }

        //public enum OrderStatus
        //{
        //    [Description("En attente")]
        //    Pending = 0,
        //    [Description("Confirmée")]
        //    Confirmed = 1,
        //    [Description("Livrée")]
        //    Delivered = 2
        //}

        //public enum DeliveryStatus
        //{
        //    [Description("En attente")]
        //    Pending = 0,
        //    [Description("Confirmée")]
        //    Confirmed = 1,
        //    [Description("Livrée")]
        //    Delivered = 2
        //}

        public enum OrderStatus
        {
            [Description("Pending")]
            Pending = 0,
            [Description("Confirmed")]
            Confirmed = 1,
            [Description("Shipping")] //"En cours de livraison"
            Shipping = 2,
            [Description("Delivered")]   // Livrée
            Delivered = 3,
            [Description("Cancelled")]  // Annulée
            Cancelled = 4
        }

        public enum PaymentMethod
        {
            [Description("OrangeMoney")] //Orange Money
            OrangeMoney = 0,

            [Description("MoovFlooz")] //Moov Flooz
            MoovFlooz = 1,

            [Description("CreditCard")] // Carte bancaire
            CreditCard = 2,

            [Description("CashOnDelivery")] // Terme officiel >> Paiement à la livraison
            CashOnDelivery = 3,
        }



        //public enum DeliveryStatus
        //{
        //    [Description("En préparation")]
        //    Preparing = 0,

        //    [Description("En transit")]
        //    InTransit = 1,

        //    [Description("Livré")]
        //    Delivered = 2,

        //    [Description("Retardé")]
        //    Delayed = 3
        //}



    }
}

#region Comment
//public enum ShopSubscriptionPlan : int
//{
//    [Description("Basique - 50 produits max")]
//    Basic = 1,

//    [Description("Professionnel - 500 produits + Analytics")]
//    Pro = 2,

//    [Description("Entreprise - Illimité + Support Premium")]
//    Enterprise = 3
//}

#endregion