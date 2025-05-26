using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class RolePermissionsEntity
    {
		public int? PermissionId { get; set; }
		public int? RoleId { get; set; }

        public RolePermissionsEntity() { }

        public RolePermissionsEntity(DataRow dataRow)
        {
			PermissionId = (dataRow["PermissionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PermissionId"]);
			RoleId = (dataRow["RoleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RoleId"]);
        }
    }
}

