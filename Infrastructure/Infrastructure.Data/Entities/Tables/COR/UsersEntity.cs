using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.COR
{
    public class UsersEntity
    {
		public DateTime? CreatedAt { get; set; }
		public string Email { get; set; }
		public long Id { get; set; }
		public int Id_Role { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? LastLogin { get; set; }
		public string Mobile { get; set; }
		public string PasswordHash { get; set; }
		public string SelectedLanguage { get; set; }
		public bool? SuperAdministrator { get; set; } = false;
		public string Username { get; set; }
		public bool? VerifiedSeller { get; set; }

        public UsersEntity() { }

        public UsersEntity(DataRow dataRow)
        {
			CreatedAt = dataRow["CreatedAt"] == DBNull.Value ? null : Convert.ToDateTime(dataRow["CreatedAt"]);
			Email = dataRow["Email"] == DBNull.Value ? "" : Convert.ToString(dataRow["Email"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			Id_Role = Convert.ToInt32(dataRow["Id_Role"]);
			IsActive = dataRow["IsActive"] == DBNull.Value ? null : Convert.ToBoolean(dataRow["IsActive"]);
			LastLogin = dataRow["LastLogin"] == DBNull.Value ? null : Convert.ToDateTime(dataRow["LastLogin"]);
			Mobile = Convert.ToString(dataRow["Mobile"]);
			PasswordHash = Convert.ToString(dataRow["PasswordHash"]);
			SelectedLanguage = dataRow["SelectedLanguage"] == DBNull.Value ? "" : Convert.ToString(dataRow["SelectedLanguage"]);
			SuperAdministrator = dataRow["SuperAdministrator"] == DBNull.Value ? null : Convert.ToBoolean(dataRow["SuperAdministrator"]);
			Username = Convert.ToString(dataRow["Username"]);
			VerifiedSeller = dataRow["VerifiedSeller"] == DBNull.Value ? null : Convert.ToBoolean(dataRow["VerifiedSeller"]);
        }
    }
}

