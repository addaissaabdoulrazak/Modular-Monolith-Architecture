using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core
{

    #region > Adda - Configuration in progress ... 

    #endregion
    public class Program
    {
        public static string Version { get; set; } = "1.1.0-rc";
        public static Infrastructure.Services.Logging.Logger Logger { get; set; }

        // Initailize static method
        public static void Initiate()
        {

        }


        private static void initLogger()
        {
            Logger = new Infrastructure.Services.Logging.Logger();
        }

        private static void setDatabaseDefaultConnection(string connectionString)
        {
            Infrastructure.Data.Access.Settings.SetDefaultConnectionString(connectionString);
        }
        private static void SetConnectionStringNexaCorp(string connectionString)
        {
           // Infrastructure.Data.Access.Settings.SetConnectionStringNexaCorp(connectionString);
        }
        private static void setDatabaseConnectionStringTest(string connectionString)
        {
           // Infrastructure.Data.Access.Settings.setDatabaseConnectionStringTest(connectionString);
        }
    }
}
