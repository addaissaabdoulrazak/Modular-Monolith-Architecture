using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class AddressEntity
    {
		public long AddressId { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public long CustomerId { get; set; }
		public string PhoneNumber { get; set; }
		public string PostalCode { get; set; }
		public string RecipientName { get; set; }
		public string StateProvince { get; set; }

        public AddressEntity() { }

        public AddressEntity(DataRow dataRow)
        {
			AddressId = Convert.ToInt64(dataRow["AddressId"]);
			AddressLine1 = Convert.ToString(dataRow["AddressLine1"]);
			AddressLine2 = (dataRow["AddressLine2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AddressLine2"]);
			City = Convert.ToString(dataRow["City"]);
			Country = Convert.ToString(dataRow["Country"]);
			CustomerId = Convert.ToInt64(dataRow["CustomerId"]);
			PhoneNumber = (dataRow["PhoneNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PhoneNumber"]);
			PostalCode = Convert.ToString(dataRow["PostalCode"]);
			RecipientName = Convert.ToString(dataRow["RecipientName"]);
			StateProvince = (dataRow["StateProvince"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StateProvince"]);
        }
    }
}

