using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using ContractorApi.Models;
using System.Text.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ContractorApi
{
    public class DadataRequest
    {
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public DadataRequest(string query, ContractorType contractorType)
        {
            Query = query;
            Type = contractorType == ContractorType.Legal ? "LEGAL" : "INDIVIDUAL";
        }
    }
}
