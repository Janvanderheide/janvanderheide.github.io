using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLSInvoicer.Extractors
{
    public class CityExtractor
    {
        public static string GetCity(string text)
        {
            //string pattern = @"(?<=\d{4}[A-Z]{2} )\w+";
            string pattern = @"(?<=\d{4} ?[A-Z]{2})( ?([A-Za-z'-]+ ?){1,})";
            
            MatchCollection list = Regex.Matches(text, pattern);

            var result = "";
            foreach (Match item in list)
            {
                result = item.Value; // take firt element
                if(!result.Contains("Soest")) break;
            }
            return result.Trim();

        }
    }
}
