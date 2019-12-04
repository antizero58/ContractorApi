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
    public class DadataClient
    {
        private const string _dadataUrl = "https://suggestions.dadata.ru/suggestions/api/4_1/rs/suggest/party";
        private string _apiKey = "0d5a072ea1b0378bd05b7dedcb59dfc526ba54fa";

        public DadataClient()
        {}

        private class DadataRequest
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

            public string Serialize()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

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

        public DadataResponse GetSuggestions(string inn, string kpp, ContractorType type)
        {
            DadataResponse response = null;

            // при создании контрагента проверять его наличие в ЕГРЮЛ по полям inn kpp для Юр.лица и по inn для ИП
            if (type == ContractorType.Legal)
            {
                response = GetSuggestionsInternal(inn, type);

                if (response == null)
                    response = GetSuggestionsInternal(kpp, type);
            }
            else if (type == ContractorType.Individual)
            {
                response = GetSuggestionsInternal(inn, type);
            }

            return response;
        }

        private DadataResponse GetSuggestionsInternal(string query, ContractorType type)
        {
            var request = (HttpWebRequest)WebRequest.Create(_dadataUrl);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", $"Token {_apiKey}");

            var dadataRequest = new DadataRequest(query, type).Serialize();

            HttpWebResponse response = null;

            try
            {
                request.ContentLength = dadataRequest.Length;
                using (Stream stream = request.GetRequestStream())
                    stream.Write(Encoding.UTF8.GetBytes(dadataRequest), 0, dadataRequest.Length);

                response = (HttpWebResponse)request.GetResponse();

                string responseStr;
                using (var sr = new StreamReader(response.GetResponseStream()))
                    responseStr = sr.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK && !responseStr.IsNullOrEmpty())
                {
                    DadataResponse dadataResponse = JsonConvert.DeserializeObject<DadataResponse>(responseStr);
                    if (dadataResponse.Suggestions.Length == 0)
                        return null;

                    return dadataResponse;
                }
            }
            catch (WebException ex)
            {
                int b = 0;
            }
            catch (Exception ex)
            {
                int b = 0;
            }
            finally
            {
                response?.Close();
            }
            
            return null;
        }
    }
}
