using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Data.Access
{
    public class Settings
    {
        public const int MAX_BATCH_SIZE = 1000;
        public static string ServerIP { get; set; }
        private static bool DebugMode = true;

        public static string GetConnectionString()
        {
            return @"";
        }
        public static bool GetDebugMode()
        {
            return DebugMode;
        }
    }
}
