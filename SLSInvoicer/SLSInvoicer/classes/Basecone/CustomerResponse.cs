using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLSInvoicer.classes.Basecone
{
    public class GetCustomerResponse
    {
        public Guid CustomerId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }

        public CompanyReference Company { get; set; }
        public AddressReference Address { get; set; }
        //public VatCodeReference VatCode { get; set; }
        public string VatNumber { get; set; }
        public string Fax { get; set; }
        //public BankAccountReference[] BankAccounts { get; set; }
        //public GeneralLedgerReference SubstitutionGeneralLedger { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string ChamberOfCommerce { get; set; }
        public string Phone { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        //public PaymentConditionReference PaymentCondition { get; set; }
    }

    public class AddressReference
    {
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public CountryReference Country { get; set; }
    }
    public class CountryReference
    {
        public string Code { get; set; }
    }

    public class CompanyReference
    {
        public Guid CompanyId { get; set; }

        public string CompanyCode { get; set; }
    }
}
