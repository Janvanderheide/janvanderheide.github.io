using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLSInvoicer.Extractors
{
    public class BestelnummerExtractor
    {
        public static string GetBestelnummer(string pdfText)
        {
            string pattern = @"(Bestellingsnummer|Bestelnummer|Bestellingsnummer|Ordernummer):( | )*\d+";
            string input = pdfText;

            MatchCollection list = Regex.Matches(input, pattern);

            var result = "";
            foreach (Match item in list)
            {
                result = item.Value; // take firt element
                result = result.Replace(" ", "");
                result = result.Replace(" ", "");
                result = result.Replace(" ", "");
                result = result.Replace(" ", "");
                result = result.Replace("Bestellingsnummer:", "");
                result = result.Replace("Bestelnummer:", "");
                result = result.Replace("Bestellingsnummer:", "");
                result = result.Replace("Ordernummer:", "");
          
                break;
            }
            return result;
        }
    }
}
