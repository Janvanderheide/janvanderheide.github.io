using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using SLSInvoicer.BaseconeServices;
using SLSInvoicer.classes;
using SLSInvoicer.classes.Basecone;
using SLSInvoicer.classes.Woo;
using SLSInvoicer.WooServices;

namespace SLSInvoicerTest.WooCommerce
{

    public class TestConnectionWoocommerce
    {
        [Test]
        public void Test()
        {
            // from date will fetch everything from woocommerce from this date:
            var fromDate = new DateTime(2018, 01, 03);

            // nalini woo commerce getting only
            string wooClientIdentifier = "ck_ba27fec8e6066a2805d47445dd1f75532de7cd7e";
            string wooSecret = "cs_84c39b40d369af4d9f7a7a4c22a6f6cb3a24f066";
            string wooUrl = "https://nalinikraampakket.nl";

            // uat
            //string BaseconeClientIdentifier = "d9547eb7-208a-4fbd-8943-eb0aca41c9ea";
            //string BaseconeSecret = "08261ba3-60fe-48ca-ab42-938714f74247";
            //string BaseconeUrl = "https://uat.basecone.com/Baseconeapi/";
            //Guid CompanyId = new Guid("bd284725-bc8b-4642-b1ab-660d294b276b");

            //live nalini:
            string BaseconeClientIdentifier = "53de13e3-e83e-4bc7-8704-175c71d73be5";
            string BaseconeSecret = "9293c380-1972-4c77-90bd-c73815a15fab";
            string BaseconeUrl = "https://api.basecone.com/v1/";
            Guid CompanyId = new Guid("84c21c30-f91c-47f1-bcb5-81102519af2c");
            //--nalini

            //http://www.example.com/wp-json/wc/v2/
            var wooCommerceClient = HttpClientFactory.GetBasicHttpClient(wooUrl, wooClientIdentifier, wooSecret);

            var baseconeClient = HttpClientFactory.GetBasicHttpClient(BaseconeUrl, BaseconeClientIdentifier, BaseconeSecret);


            // Get orders:
            var orderservice = new WooOrderService(wooCommerceClient);
            var result = orderservice.GetAllOrders(fromDate);
            result.Wait();
            var orders = result.Result;
            // filter all manual orders and only sync those. (te rest van de boekingen wordt in 1 keer gedaan omdat die niet afgeletterd hoeven te worden)
            orders = orders.Where(x => x.payment_method_title != "iDEAL").ToList();


            var baseconeCustomerService = new BaseconeCustomerService(baseconeClient);
            var bCustomers = baseconeCustomerService.GetAllCustomers(CompanyId, null, 10000, 0);
            bCustomers.Wait();
            var code = bCustomers.Result.Body.Customers.Max(x => x.Code);
            var customerListFromBasecone = bCustomers.Result.Body.Customers;

            foreach (var order in orders)
            {
                var billingAddres = order.billing;
                code++;
                var customer = new CustomerRequest()
                {
                    Code = code.ToString(),
                    Company = new CompanyRequest()
                    {
                      CompanyId  = CompanyId
                    },
                    Name = order.billing.company == string.Empty ? billingAddres.first_name + " "+ billingAddres.last_name : order.billing.company,
                    Address = new AddressRequest()
                    {
                        PostalCode = billingAddres.postcode,
                        City = billingAddres.city,
                        StreetName = billingAddres.address_1 + " " + billingAddres.address_2,
                        Country = new CountryRequest()
                        {
                            Code = "NL"
                        }
                        
                    },
                    Phone = billingAddres.phone,
                    Email = billingAddres.email
                };

                if (customer.Name.Trim() == string.Empty)
                {
                    Console.WriteLine("Empty don't add " + customer.Name + " " + order.number + ";");
                }

                if (!DoesCustomerExists(customer.Name, customer.Address.PostalCode, customerListFromBasecone))
                {
                    Console.WriteLine("NOT EXISTS      " + customer.Name + " " + customer.Address.City + ";");

                    // Create customer in basecone when not exists.
                    var createResult = baseconeCustomerService.CreateCustomer(customer, false);
                    createResult.Wait();
                    // Add the newly added customers postalcode and name to the list so it won't be created double.
                    bCustomers.Result.Body.Customers.Add(new SLSInvoicer.classes.Basecone.Customer() {Name = customer.Name, Address = new AddressReference() {PostalCode = customer.Address.PostalCode} });

                }
                else
                {
                    Console.WriteLine("Already exists! " + customer.Name + " " + customer.Address.City + ";");
                }
                
            }
        }

        public bool DoesCustomerExists(string name, string postalCode, List<SLSInvoicer.classes.Basecone.Customer> customers)
        {
            return customers.Any(x => x.Name.Contains(name)) || customers.Any(x => x.Address?.PostalCode.Trim().Replace(" ", "") == postalCode.Trim().Replace(" ", ""));
        }

    }

    public static class HttpClientFactory
    {
        public static HttpClient GetBasicHttpClient(string baseUrl, string clientIdentifier, string apiKey)
        {
            var client = new HttpClient { BaseAddress = new Uri(baseUrl) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientIdentifier}:{apiKey}")));

            return client;
        }
    }



   

    
}
