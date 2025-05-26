using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class PermissionsEntity
    {
		public string Name { get; set; }
		public int PermissionId { get; set; }

        public PermissionsEntity() { }

        public PermissionsEntity(DataRow dataRow)
        {
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			PermissionId = Convert.ToInt32(dataRow["PermissionId"]);
        }
    }
}

