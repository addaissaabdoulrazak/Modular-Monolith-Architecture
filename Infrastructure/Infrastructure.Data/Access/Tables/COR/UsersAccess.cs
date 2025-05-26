using Infrastructure.Data.Entities.Tables.COR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Data.Access.Tables.COR
{

    public class UsersAccess
    {
        #region Default Methods
        public static UsersEntity Get(long id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Users] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.COR.UsersEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<UsersEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Users]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UsersEntity(x)).ToList();
            }
            else
            {
                return new List<UsersEntity>();
            }
        }
        public static List<UsersEntity> Get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<UsersEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<UsersEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<UsersEntity>();
        }
        private static List<UsersEntity> get(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                var dataTable = new DataTable();
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    
                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    sqlCommand.CommandText = $"SELECT * FROM [Users] WHERE [Id] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UsersEntity(x)).ToList();
                }
                else
                {
                    return new List<UsersEntity>();
                }
            }
            return new List<UsersEntity>();
        }

        public static long Insert(UsersEntity item)
        {
            long response = long.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Users] ([CreatedAt],[Email],[Id_Role],[IsActive],[LastLogin],[Mobile],[PasswordHash],[SelectedLanguage],[SuperAdministrator],[Username],[VerifiedSeller])  VALUES (@CreatedAt,@Email,@Id_Role,@IsActive,@LastLogin,@Mobile,@PasswordHash,@SelectedLanguage,@SuperAdministrator,@Username,@VerifiedSeller); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("Email",item.Email == null ? (object)DBNull.Value  : item.Email);
					sqlCommand.Parameters.AddWithValue("Id_Role",item.Id_Role);
					sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
					sqlCommand.Parameters.AddWithValue("LastLogin",item.LastLogin == null ? (object)DBNull.Value  : item.LastLogin);
					sqlCommand.Parameters.AddWithValue("Mobile",item.Mobile);
					sqlCommand.Parameters.AddWithValue("PasswordHash",item.PasswordHash);
					sqlCommand.Parameters.AddWithValue("SelectedLanguage",item.SelectedLanguage == null ? (object)DBNull.Value  : item.SelectedLanguage);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator",item.SuperAdministrator == null ? (object)DBNull.Value  : item.SuperAdministrator);
					sqlCommand.Parameters.AddWithValue("Username",item.Username);
					sqlCommand.Parameters.AddWithValue("VerifiedSeller",item.VerifiedSeller == null ? (object)DBNull.Value  : item.VerifiedSeller);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? long.MinValue:  long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<UsersEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
                int results=0;
                if(items.Count <= maxParamsNumber)
                {
                    results = insert(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }
                return results;
            }

            return -1;
        }
        private static int insert(List<UsersEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " INSERT INTO [Users] ([CreatedAt],[Email],[Id_Role],[IsActive],[LastLogin],[Mobile],[PasswordHash],[SelectedLanguage],[SuperAdministrator],[Username],[VerifiedSeller]) VALUES ( "

							+ "@CreatedAt"+ i +","
							+ "@Email"+ i +","
							+ "@Id_Role"+ i +","
							+ "@IsActive"+ i +","
							+ "@LastLogin"+ i +","
							+ "@Mobile"+ i +","
							+ "@PasswordHash"+ i +","
							+ "@SelectedLanguage"+ i +","
							+ "@SuperAdministrator"+ i +","
							+ "@Username"+ i +","
							+ "@VerifiedSeller"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value  : item.Email);
							sqlCommand.Parameters.AddWithValue("Id_Role" + i, item.Id_Role);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
							sqlCommand.Parameters.AddWithValue("LastLogin" + i, item.LastLogin == null ? (object)DBNull.Value  : item.LastLogin);
							sqlCommand.Parameters.AddWithValue("Mobile" + i, item.Mobile);
							sqlCommand.Parameters.AddWithValue("PasswordHash" + i, item.PasswordHash);
							sqlCommand.Parameters.AddWithValue("SelectedLanguage" + i, item.SelectedLanguage == null ? (object)DBNull.Value  : item.SelectedLanguage);
							sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator == null ? (object)DBNull.Value  : item.SuperAdministrator);
							sqlCommand.Parameters.AddWithValue("Username" + i, item.Username);
							sqlCommand.Parameters.AddWithValue("VerifiedSeller" + i, item.VerifiedSeller == null ? (object)DBNull.Value  : item.VerifiedSeller);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(UsersEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Users] SET [CreatedAt]=@CreatedAt, [Email]=@Email, [Id_Role]=@Id_Role, [IsActive]=@IsActive, [LastLogin]=@LastLogin, [Mobile]=@Mobile, [PasswordHash]=@PasswordHash, [SelectedLanguage]=@SelectedLanguage, [SuperAdministrator]=@SuperAdministrator, [Username]=@Username, [VerifiedSeller]=@VerifiedSeller WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreatedAt",item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("Email",item.Email == null ? (object)DBNull.Value  : item.Email);
				sqlCommand.Parameters.AddWithValue("Id_Role",item.Id_Role);
				sqlCommand.Parameters.AddWithValue("IsActive",item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
				sqlCommand.Parameters.AddWithValue("LastLogin",item.LastLogin == null ? (object)DBNull.Value  : item.LastLogin);
				sqlCommand.Parameters.AddWithValue("Mobile",item.Mobile);
				sqlCommand.Parameters.AddWithValue("PasswordHash",item.PasswordHash);
				sqlCommand.Parameters.AddWithValue("SelectedLanguage",item.SelectedLanguage == null ? (object)DBNull.Value  : item.SelectedLanguage);
				sqlCommand.Parameters.AddWithValue("SuperAdministrator",item.SuperAdministrator == null ? (object)DBNull.Value  : item.SuperAdministrator);
				sqlCommand.Parameters.AddWithValue("Username",item.Username);
				sqlCommand.Parameters.AddWithValue("VerifiedSeller",item.VerifiedSeller == null ? (object)DBNull.Value  : item.VerifiedSeller);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<UsersEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
                int results = 0;
                if(items.Count <= maxParamsNumber)
                {
                    results = update(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }

                return results;
            }

            return -1;
        }
        private static int update(List<UsersEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " UPDATE [Users] SET "

							+ "[CreatedAt]=@CreatedAt"+ i +","
							+ "[Email]=@Email"+ i +","
							+ "[Id_Role]=@Id_Role"+ i +","
							+ "[IsActive]=@IsActive"+ i +","
							+ "[LastLogin]=@LastLogin"+ i +","
							+ "[Mobile]=@Mobile"+ i +","
							+ "[PasswordHash]=@PasswordHash"+ i +","
							+ "[SelectedLanguage]=@SelectedLanguage"+ i +","
							+ "[SuperAdministrator]=@SuperAdministrator"+ i +","
							+ "[Username]=@Username"+ i +","
							+ "[VerifiedSeller]=@VerifiedSeller"+ i +" WHERE [Id]=@Id" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
							sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value  : item.CreatedAt);
							sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value  : item.Email);
							sqlCommand.Parameters.AddWithValue("Id_Role" + i, item.Id_Role);
							sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value  : item.IsActive);
							sqlCommand.Parameters.AddWithValue("LastLogin" + i, item.LastLogin == null ? (object)DBNull.Value  : item.LastLogin);
							sqlCommand.Parameters.AddWithValue("Mobile" + i, item.Mobile);
							sqlCommand.Parameters.AddWithValue("PasswordHash" + i, item.PasswordHash);
							sqlCommand.Parameters.AddWithValue("SelectedLanguage" + i, item.SelectedLanguage == null ? (object)DBNull.Value  : item.SelectedLanguage);
							sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator == null ? (object)DBNull.Value  : item.SuperAdministrator);
							sqlCommand.Parameters.AddWithValue("Username" + i, item.Username);
							sqlCommand.Parameters.AddWithValue("VerifiedSeller" + i, item.VerifiedSeller == null ? (object)DBNull.Value  : item.VerifiedSeller);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(long id)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Users] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static int Delete(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE; 
                int results=0;
                if(ids.Count <= maxParamsNumber)
                {
                    results = delete(ids);
                } else
                {
                    int batchNumber = ids.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
                }
            }
            return -1;
        }
        private static int delete(List<long> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;

                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    string query = "DELETE FROM [Users] WHERE [Id] IN ("+ queryIds +")";                    
                    sqlCommand.CommandText = query;
                        
                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }
            return -1;
        }
        #endregion

        #region Custom Methods

        public static UsersEntity GetByUsername(string name)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Users] WHERE [Username]=@name";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("name", name); //Username

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.COR.UsersEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }


        #endregion
    }
}
