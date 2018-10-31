using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SLSInvoicer.classes;
using SLSInvoicer.classes.Basecone;

namespace SLSInvoicer.BaseconeServices
{
    public class BaseconeCustomerService
    {
        private readonly HttpClient _httpClient;

        public BaseconeCustomerService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<BaseconeResponse<CustomerResponse>> CreateCustomer(CustomerRequest request,
            bool skipCreationInAccountingSystem = false)
        {
            skipCreationInAccountingSystem = false;
            var response =
                await
                    _httpClient.PostAsync($"customers?skipCreationInAccountingSystem={skipCreationInAccountingSystem}",
                        new JsonContent(request));

            return await response.ReadAsJson<CustomerResponse>();
        }

        public async Task<BaseconeResponse<CustomersResponse>> GetAllCustomers(Guid companyId, string externalId = null,
            int? limit = null, int? offset = null)
        {
            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(externalId))
            {
                parameters.Add("ExternalId", externalId);
            }

            if (limit.HasValue)
            {
                parameters.Add("limit", limit.ToString());
            }
            if (offset.HasValue)
            {
                parameters.Add("offset", offset.ToString());
            }

            parameters.Add("companyId", companyId.ToString());

            var response = await _httpClient.GetAsync("customers" + QueryStringBuilder.Build(parameters));

            return await response.ReadAsJson<CustomersResponse>();
        }

        public async Task<BaseconeResponse<object>> UpdateCustomer(Guid customerId, CustomerRequest request,
            bool skipCreationInAccountingSystem = false)
        {
            var response =
                await
                    _httpClient.PutAsync(
                        $"customers/{customerId}?skipCreationInAccountingSystem={skipCreationInAccountingSystem}",
                        new JsonContent(request));

            return await response.ReadAsJson<object>();
        }

        public async Task<BaseconeResponse<GetCustomerResponse>> GetCustomer(Guid customerId)
        {
            var response = await _httpClient.GetAsync($"customers/" + customerId);

            return await response.ReadAsJson<GetCustomerResponse>();
        }

    }
}
