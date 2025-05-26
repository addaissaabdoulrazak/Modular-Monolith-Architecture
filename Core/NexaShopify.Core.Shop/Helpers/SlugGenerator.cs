using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NexaShopify.Core.Shop.Helpers
{
    public static class SlugGenerator
    {
        public static string GenerateSlug(string input)
        {
            return Regex.Replace(
                input.ToLowerInvariant(),
                @"[^a-z0-9\s-]",    // Deletes special characters
                "")
                .Replace(" ", "-")  
                .Replace("--", "-"); 
        }
    }
}
