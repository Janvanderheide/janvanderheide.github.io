using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLSInvoicer.Extractors
{
    public class CustomerExtractor
    {
        public static string GetCustomerName(string pdfText)
        {
            string pattern = @"(?<=Nalini Natuurlijk Kraampakket|Verzend naar:)[\w ]*";
            string input = pdfText;

            MatchCollection list = Regex.Matches(input, pattern);

            var result = list.Cast<Match>().Select(m => m.Value)
                .Where(x => x.Trim() != "").ToList().Last();
            

            return result.Trim();
        }
    }
}
