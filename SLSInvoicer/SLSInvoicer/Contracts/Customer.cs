using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLSInvoicer.Contracts
{
    public class Customer
    {
        public string UserName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string BestelNummer { get; set; }
    }
}
