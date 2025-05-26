using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.Store
{
    public class StoreAccess 
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.Store.ShopEntity Get(int id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Store] WHERE [id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.Store.ShopEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> Get()
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Store]";
                var sqlCommand = new SqlCommand(query, sqlConnection);

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Store.ShopEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.Store.ShopEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> Get(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE;
                List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> results = null;
                if (ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }
                else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.Store.ShopEntity>();
                    for (int i = 0; i < batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.Store.ShopEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> get(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                var dataTable = new DataTable();
                using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;

                    string queryIds = string.Empty;
                    for (int i = 0; i < ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    sqlCommand.CommandText = $"SELECT * FROM [Store] WHERE [id] IN ({queryIds})";
                    new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Store.ShopEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.Store.ShopEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.Store.ShopEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.Store.ShopEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Store] ([address],[banner],[created_at],[custom_url],[description],[email],[logo],[name],[phone],[updated_at])  VALUES (@address,@banner,@created_at,@custom_url,@description,@email,@logo,@name,@phone,@updated_at); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                {

                    sqlCommand.Parameters.AddWithValue("address", item.address == null ? (object)DBNull.Value : item.address);
                    sqlCommand.Parameters.AddWithValue("banner", item.banner == null ? (object)DBNull.Value : item.banner);
                    sqlCommand.Parameters.AddWithValue("created_at", item.created_at == null ? (object)DBNull.Value : item.created_at);
                    sqlCommand.Parameters.AddWithValue("custom_url", item.custom_url == null ? (object)DBNull.Value : item.custom_url);
                    sqlCommand.Parameters.AddWithValue("description", item.description == null ? (object)DBNull.Value : item.description);
                    sqlCommand.Parameters.AddWithValue("email", item.email);
                    sqlCommand.Parameters.AddWithValue("logo", item.logo == null ? (object)DBNull.Value : item.logo);
                    sqlCommand.Parameters.AddWithValue("name", item.name);
                    sqlCommand.Parameters.AddWithValue("phone", item.phone == null ? (object)DBNull.Value : item.phone);
                    sqlCommand.Parameters.AddWithValue("updated_at", item.updated_at == null ? (object)DBNull.Value : item.updated_at);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
                int results = 0;
                if (items.Count <= maxParamsNumber)
                {
                    results = insert(items);
                }
                else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for (int i = 0; i < batchNumber; i++)
                    {
                        results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }
                return results;
            }

            return -1;
        }
        private static int insert(List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " INSERT INTO [Store] ([address],[banner],[created_at],[custom_url],[description],[email],[logo],[name],[phone],[updated_at]) VALUES ( "

                            + "@address" + i + ","
                            + "@banner" + i + ","
                            + "@created_at" + i + ","
                            + "@custom_url" + i + ","
                            + "@description" + i + ","
                            + "@email" + i + ","
                            + "@logo" + i + ","
                            + "@name" + i + ","
                            + "@phone" + i + ","
                            + "@updated_at" + i
                            + "); ";


                        sqlCommand.Parameters.AddWithValue("address" + i, item.address == null ? (object)DBNull.Value : item.address);
                        sqlCommand.Parameters.AddWithValue("banner" + i, item.banner == null ? (object)DBNull.Value : item.banner);
                        sqlCommand.Parameters.AddWithValue("created_at" + i, item.created_at == null ? (object)DBNull.Value : item.created_at);
                        sqlCommand.Parameters.AddWithValue("custom_url" + i, item.custom_url == null ? (object)DBNull.Value : item.custom_url);
                        sqlCommand.Parameters.AddWithValue("description" + i, item.description == null ? (object)DBNull.Value : item.description);
                        sqlCommand.Parameters.AddWithValue("email" + i, item.email);
                        sqlCommand.Parameters.AddWithValue("logo" + i, item.logo == null ? (object)DBNull.Value : item.logo);
                        sqlCommand.Parameters.AddWithValue("name" + i, item.name);
                        sqlCommand.Parameters.AddWithValue("phone" + i, item.phone == null ? (object)DBNull.Value : item.phone);
                        sqlCommand.Parameters.AddWithValue("updated_at" + i, item.updated_at == null ? (object)DBNull.Value : item.updated_at);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.Store.ShopEntity item)
        {
            int results = -1;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [Store] SET [address]=@address, [banner]=@banner, [created_at]=@created_at, [custom_url]=@custom_url, [description]=@description, [email]=@email, [logo]=@logo, [name]=@name, [phone]=@phone, [updated_at]=@updated_at WHERE [id]=@id";
                var sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("id", item.id);
                sqlCommand.Parameters.AddWithValue("address", item.address == null ? (object)DBNull.Value : item.address);
                sqlCommand.Parameters.AddWithValue("banner", item.banner == null ? (object)DBNull.Value : item.banner);
                sqlCommand.Parameters.AddWithValue("created_at", item.created_at == null ? (object)DBNull.Value : item.created_at);
                sqlCommand.Parameters.AddWithValue("custom_url", item.custom_url == null ? (object)DBNull.Value : item.custom_url);
                sqlCommand.Parameters.AddWithValue("description", item.description == null ? (object)DBNull.Value : item.description);
                sqlCommand.Parameters.AddWithValue("email", item.email);
                sqlCommand.Parameters.AddWithValue("logo", item.logo == null ? (object)DBNull.Value : item.logo);
                sqlCommand.Parameters.AddWithValue("name", item.name);
                sqlCommand.Parameters.AddWithValue("phone", item.phone == null ? (object)DBNull.Value : item.phone);
                sqlCommand.Parameters.AddWithValue("updated_at", item.updated_at == null ? (object)DBNull.Value : item.updated_at);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
                int results = 0;
                if (items.Count <= maxParamsNumber)
                {
                    results = update(items);
                }
                else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for (int i = 0; i < batchNumber; i++)
                    {
                        results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }

                return results;
            }

            return -1;
        }
        private static int update(List<Infrastructure.Data.Entities.Tables.Store.ShopEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " UPDATE [Store] SET "

                            + "[address]=@address" + i + ","
                            + "[banner]=@banner" + i + ","
                            + "[created_at]=@created_at" + i + ","
                            + "[custom_url]=@custom_url" + i + ","
                            + "[description]=@description" + i + ","
                            + "[email]=@email" + i + ","
                            + "[logo]=@logo" + i + ","
                            + "[name]=@name" + i + ","
                            + "[phone]=@phone" + i + ","
                            + "[updated_at]=@updated_at" + i + " WHERE [id]=@id" + i
                            + "; ";

                        sqlCommand.Parameters.AddWithValue("id" + i, item.id);
                        sqlCommand.Parameters.AddWithValue("address" + i, item.address == null ? (object)DBNull.Value : item.address);
                        sqlCommand.Parameters.AddWithValue("banner" + i, item.banner == null ? (object)DBNull.Value : item.banner);
                        sqlCommand.Parameters.AddWithValue("created_at" + i, item.created_at == null ? (object)DBNull.Value : item.created_at);
                        sqlCommand.Parameters.AddWithValue("custom_url" + i, item.custom_url == null ? (object)DBNull.Value : item.custom_url);
                        sqlCommand.Parameters.AddWithValue("description" + i, item.description == null ? (object)DBNull.Value : item.description);
                        sqlCommand.Parameters.AddWithValue("email" + i, item.email);
                        sqlCommand.Parameters.AddWithValue("logo" + i, item.logo == null ? (object)DBNull.Value : item.logo);
                        sqlCommand.Parameters.AddWithValue("name" + i, item.name);
                        sqlCommand.Parameters.AddWithValue("phone" + i, item.phone == null ? (object)DBNull.Value : item.phone);
                        sqlCommand.Parameters.AddWithValue("updated_at" + i, item.updated_at == null ? (object)DBNull.Value : item.updated_at);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(int id)
        {
            int results = -1;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [Store] WHERE [id]=@id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("id", id);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public  int Delete(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE;
                int results = 0;
                if (ids.Count <= maxParamsNumber)
                {
                    results = delete(ids);
                }
                else
                {
                    int batchNumber = ids.Count / maxParamsNumber;
                    for (int i = 0; i < batchNumber; i++)
                    {
                        results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
                }
            }
            return -1;
        }
        private static int delete(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                int results = -1;
                using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;

                    string queryIds = string.Empty;
                    for (int i = 0; i < ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    string query = "DELETE FROM [Store] WHERE [id] IN (" + queryIds + ")";
                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }
            return -1;
        }
        #endregion



        #region Custom Methods

        // if you want to add or custom your method

        #endregion
    }
}
