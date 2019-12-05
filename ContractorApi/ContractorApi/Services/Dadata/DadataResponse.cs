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
    public class DadataResponse
    {
        [JsonProperty(PropertyName = "suggestions")]
        public DadataResponseRecord[] Suggestions { get; set; }
    }

    public class DadataResponseRecord
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "data")]
        public DadataResponseRecordData Data { get; set; }
    }

    public class DadataResponseRecordData
    {
        [JsonProperty(PropertyName = "inn")]
        public string Inn { get; set; }

        [JsonProperty(PropertyName = "kpp")]
        public string Kpp { get; set; }

        [JsonProperty(PropertyName = "ogrn")]
        public string Ogrn { get; set; }

        [JsonProperty(PropertyName = "name")]
        public DadataResponseRecordDataName Name { get; set; }
    }

    public class DadataResponseRecordDataName
    {
        [JsonProperty(PropertyName = "full_with_opf")]
        public string FullWithOpf { get; set; }

        [JsonProperty(PropertyName = "short_with_opf")]
        public string ShortWithOpf { get; set; }

        [JsonProperty(PropertyName = "latin")]
        public string Latin { get; set; }

        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "short")]
        public string Short { get; set; }
    }
}
