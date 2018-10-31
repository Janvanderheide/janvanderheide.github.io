using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SLSInvoicer.classes;
using SLSInvoicer.classes.Woo;

namespace SLSInvoicer.WooServices
{
    public class WooOrderService
    {
        private HttpClient _httpClient;
        public WooOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Order>> GetAllOrders(DateTime fromDate)
        {
            var page = 0;
            var orders = new List<Order>();
            List<Order> result;
            do
            {
                result = GetOrders(100, page * 100, fromDate).Result.Body;
                page++;
                orders.AddRange(result);
            } while (result.Count > 99);

            return orders; //.Where(x => x);
        }
        public async Task<BaseconeResponse<List<Order>>> GetOrders(int limt, int offset, DateTime afterDate)
        {

            var response = await _httpClient.GetAsync($"wp-json/wc/v2/orders?per_page={limt}&offset={offset}&after={afterDate:o}");
            //var response = await _httpClient.GetAsync($"wp-json/wc/v2/orders?per_page={limt}&offset={offset}");
            return await response.ReadAsJson<List<Order>>();
        }

    }
}
