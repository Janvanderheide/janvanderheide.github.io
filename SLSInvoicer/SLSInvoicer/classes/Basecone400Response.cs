using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLSInvoicer.classes
{
    public class Basecone400Response
    {
        public string Message { get; set; }

        public string Code { get; set; }

        //[JsonProperty("_metadata")]
        //public Metadata[] Metadata { get; set; }

        //[JsonProperty("_moreinfo")]
        public string MoreInfo { get; set; }
    }
}
