using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SLSInvoicer.classes.Basecone
{
    public class CustomersResponse
    {
        public List<Customer> Customers { get; set; }

       
        //[JsonProperty("_links")]
        //public Links[] Links { get; set; }

        //[JsonProperty("_metadata")]
        //public Metadata Metadata { get; set; }
    }

    public class Customer
    {
        public Guid CustomerId { get; set; }
        public Int64 Code { get; set; }

        public string Name { get; set; }
        public string ExternalId { get; set; }

        public CompanyReference Company { get; set; }
        public AddressReference Address { get; set; }
    }
    public class CustomerRequest
    {
        public CompanyRequest Company { get; set; }
        public string Code { get; set; }
        public string ExternalId { get; set; }
        public AddressRequest Address { get; set; }
        //public PayCollectTypeRequest PayCollectType { get; set; }
        //public VatCodeRequest VatCode { get; set; }
        public string Name { get; set; }
        public string VatNumber { get; set; }
        public string ChamberOfCommerce { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        //public BankAccountRequest[] BankAccounts { get; set; }
        //public GeneralLedgerRequest SubstitutionGeneralLedger { get; set; }
        public string Number { get; set; }
        //public PaymentConditionRequest PaymentCondition { get; set; }
        public string Email { get; set; }
    }

    public class CustomerResponse
    {
        public Guid CustomerId { get; set; }
    }

    

    public class CompanyRequest
    {
        public Guid CompanyId { get; set; }
    }
    public class AddressRequest
    {
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public CountryRequest Country { get; set; }
    }

    public class CountryRequest
    {
        public string Code { get; set; }
    }
}
