using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLSInvoicer.Extractors
{
    public class PostalCodeExtractor
    {
        public static string GetPostalCode(string text)
        {
            string pattern = @"\d{4} ?[A-Z]{2}";

            MatchCollection list = Regex.Matches(text, pattern);

            var result = "";
            foreach (Match item in list)
            {
                result = item.Value; // take firt element
                if(!result.StartsWith("3768")) break;
            }
            return result;
            
        }
    }
}
