using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SLSInvoicer.classes
{
    public class QueryStringBuilder
    {
        public static string Build(Dictionary<string, string> queryStringParameters)
        {
            return Build(queryStringParameters?.ToDictionary(queryStringParameter => queryStringParameter.Key, queryStringParameter => new List<string>
            {
                queryStringParameter.Value
            }));
        }

        public static string Build(Dictionary<string, List<string>> queryStringParameters)
        {
            var queryString = string.Empty;

            if (queryStringParameters == null || queryStringParameters.Count == 0)
            {
                return queryString;
            }

            foreach (var parameter in queryStringParameters)
            {
                queryString += queryStringParameters.First().Key == parameter.Key
                    ? "?"
                    : "&";
                queryString += HttpUtility.UrlEncode(parameter.Key) + "=";
                queryString += parameter.Value.Count > 1 ?
                    JoinValues(parameter.Value) :
                    HttpUtility.UrlEncode(parameter.Value.First());
            }
            return queryString;
        }

        private static string JoinValues(List<string> queryStringValues)
        {
            return $"[{EscapeValues(queryStringValues[0])},{EscapeValues(queryStringValues[1])}]";
        }

        private static string EscapeValues(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "null" : value;
        }
    }
}
