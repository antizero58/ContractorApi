using System;
using System.Net;
using ContractorApi.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace ContractorApi.WebServices.Dadata
{
    public class DadataClient : IDadataClient
    {
        private readonly string _dadataUrl;
        private readonly string _apiKey;

        public DadataClient(IOptions<DadataOptions> options)
        {
            _dadataUrl = options.Value.Url;
            _apiKey = options.Value.ApiKey;
        }

        public DadataResponse GetSuggestions(string inn, string kpp, ContractorType type)
        {
            DadataResponse response = null;

            // При создании контрагента проверять его наличие в ЕГРЮЛ по полям inn kpp для Юр.лица и по inn для ИП
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

            var requestJson = JsonConvert.SerializeObject(new DadataRequest(query, type));

            HttpWebResponse response = null;

            try
            {
                request.ContentLength = requestJson.Length;
                using (Stream stream = request.GetRequestStream())
                    stream.Write(Encoding.UTF8.GetBytes(requestJson), 0, requestJson.Length);

                response = (HttpWebResponse)request.GetResponse();

                string responseJson;
                using (var sr = new StreamReader(response.GetResponseStream()))
                    responseJson = sr.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK && !responseJson.IsNullOrEmpty())
                {
                    DadataResponse dadataResponse = JsonConvert.DeserializeObject<DadataResponse>(responseJson);
                    if (dadataResponse.Suggestions.Length == 0)
                        return null;

                    return dadataResponse;
                }
            }
            catch (WebException ex)
            {
                // TODO
            }
            catch (Exception ex)
            {
                // TODO
            }
            finally
            {
                response?.Close();
            }
            
            return null;
        }
    }
}
