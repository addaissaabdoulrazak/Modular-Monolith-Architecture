using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class OrderEntity
    {
		public DateTime? CreatedAt { get; set; }
		public long CustomerId { get; set; }
		public string DeliveryAddress { get; set; }
		public DateTime? OrderDate { get; set; }
		public long OrderId { get; set; }
		public string OrderNumber { get; set; }
		public string OrderStatus { get; set; }
		public string OrderStatus_Id { get; set; }
		public string PaymentMethod { get; set; }
		public string PaymentMethod_Id { get; set; }
		public string PaymentStatus { get; set; }
		public string PaymentStatus_Id { get; set; }
		public long ShippingAddressId { get; set; }
		public long StoreId { get; set; }		
		public decimal TotalAmount { get; set; }
		public string TrackingNumber { get; set; }
		public DateTime? UpdatedAt { get; set; }

        public OrderEntity() { }

        public OrderEntity(DataRow dataRow)
        {
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			CustomerId = Convert.ToInt64(dataRow["CustomerId"]);
			DeliveryAddress = (dataRow["DeliveryAddress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryAddress"]);
			OrderDate = (dataRow["OrderDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderDate"]);
			OrderId = Convert.ToInt64(dataRow["OrderId"]);
			OrderNumber = Convert.ToString(dataRow["OrderNumber"]);
			OrderStatus = Convert.ToString(dataRow["OrderStatus"]);
			OrderStatus_Id = Convert.ToString(dataRow["OrderStatus_Id"]);
			PaymentMethod = Convert.ToString(dataRow["PaymentMethod"]);
			PaymentMethod_Id = Convert.ToString(dataRow["PaymentMethod_Id"]);
			PaymentStatus = Convert.ToString(dataRow["PaymentStatus"]);
			PaymentStatus_Id = Convert.ToString(dataRow["PaymentStatus_Id"]);
			ShippingAddressId = Convert.ToInt64(dataRow["ShippingAddressId"]);
			StoreId = Convert.ToInt64(dataRow["StoreId"]);
			TotalAmount = Convert.ToDecimal(dataRow["TotalAmount"]);
			TrackingNumber = (dataRow["TrackingNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TrackingNumber"]);
			UpdatedAt = (dataRow["UpdatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdatedAt"]);
        }
    }
}

