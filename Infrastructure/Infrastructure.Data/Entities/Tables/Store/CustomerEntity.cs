using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class CustomerEntity
    {
		public DateTime? CreatedAt { get; set; }
		public long CustomerId { get; set; }
		public string Email { get; set; }
		public string FullName { get; set; }
		public string PhoneNumber { get; set; }

        public CustomerEntity() { }

        public CustomerEntity(DataRow dataRow)
        {
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			CustomerId = Convert.ToInt64(dataRow["CustomerId"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			FullName = Convert.ToString(dataRow["FullName"]);
			PhoneNumber = Convert.ToString(dataRow["PhoneNumber"]);
        }
    }
}

