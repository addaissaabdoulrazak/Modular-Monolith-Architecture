using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class BrandEntity
    {
		public int BrandId { get; set; }
		public string BrandName { get; set; }
		public bool? IsAPartner { get; set; }
		public string WebsiteLink { get; set; }

        public BrandEntity() { }

        public BrandEntity(DataRow dataRow)
        {
			BrandId = Convert.ToInt32(dataRow["BrandId"]);
			BrandName = Convert.ToString(dataRow["BrandName"]);
			IsAPartner = (dataRow["IsAPartner"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsAPartner"]);
			WebsiteLink = (dataRow["WebsiteLink"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WebsiteLink"]);
        }
    }
}

