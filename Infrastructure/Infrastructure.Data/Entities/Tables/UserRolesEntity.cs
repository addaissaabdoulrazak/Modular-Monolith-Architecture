using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class UserRolesEntity
    {
		public DateTime CreationTime { get; set; }
		public string Description { get; set; }
		public string DisplayName { get; set; }
		public int Id { get; set; }
		public bool IsActive { get; set; }
		public int Level { get; set; }
		public string Name { get; set; }

        public UserRolesEntity() { }

        public UserRolesEntity(DataRow dataRow)
        {
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			DisplayName = Convert.ToString(dataRow["DisplayName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsActive = Convert.ToBoolean(dataRow["IsActive"]);
			Level = Convert.ToInt32(dataRow["Level"]);
			Name = Convert.ToString(dataRow["Name"]);
        }
    }
}

