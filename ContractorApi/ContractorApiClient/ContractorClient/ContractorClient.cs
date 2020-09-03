using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace ContractorApiClient
{
    public class ContractorClient : IContractorClient
    {
        private const string _apiUrl = "http://192.168.1.36:801/api/contractors";

        public ContractorClient()
        {}

        public async Task<IEnumerable<Contractor>> Get()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_apiUrl);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json";
            request.Accept = "application/json";

            var response = await request.GetResponseAsync();
            
            using (var responseStream = new StreamReader(response.GetResponseStream()))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    Contractor[] dadataResponse = JsonConvert.DeserializeObject<Contractor[]>(responseStream.ReadToEnd());
                    return dadataResponse;
                }
                else
                    throw new Exception(@$"Сервис {_apiUrl} вернул статус {httpResponse.StatusCode}");
            }
        }

        public async Task<Contractor> Get(int id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{_apiUrl}/{id}");
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json";
            request.Accept = "application/json";

            var response = await request.GetResponseAsync();

            using (var responseStream = new StreamReader(response.GetResponseStream()))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    Contractor dadataResponse = JsonConvert.DeserializeObject<Contractor>(responseStream.ReadToEnd());
                    return dadataResponse;
                }
                else
                    throw new Exception(@$"Сервис {_apiUrl} вернул статус {httpResponse.StatusCode}");
            }
        }

        public async Task<IActionResult> Create(Contractor c)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_apiUrl);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";

            try
            {
                using (var w = new StreamWriter(request.GetRequestStream()))
                using (JsonWriter writer = new JsonTextWriter(w))
                {
                    new JsonSerializer().Serialize(writer, c);
                }

                var response = await request.GetResponseAsync();

                using (var responseStream = new StreamReader(response.GetResponseStream()))
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse.StatusCode == HttpStatusCode.Created)
                        return new OkResult();
                    else
                        throw new Exception(@$"Сервис {_apiUrl} вернул статус {httpResponse.StatusCode}");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> Update(Contractor c)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_apiUrl);
            request.Method = WebRequestMethods.Http.Put;
            request.ContentType = "application/json";

            try
            {
                using (var w = new StreamWriter(request.GetRequestStream()))
                using (JsonWriter writer = new JsonTextWriter(w))
                {
                    new JsonSerializer().Serialize(writer, c);
                }

                var response = await request.GetResponseAsync();

                using (var responseStream = new StreamReader(response.GetResponseStream()))
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        return new OkResult();
                    else
                        throw new Exception(@$"Сервис {_apiUrl} вернул статус {httpResponse.StatusCode}");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{_apiUrl}/{id}");
            request.Method = "DELETE";

            var response = await request.GetResponseAsync();

            using (var responseStream = new StreamReader(response.GetResponseStream()))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                    return new NoContentResult();
                else
                    throw new Exception(@$"Сервис {_apiUrl} вернул статус {httpResponse.StatusCode}");
            }
        }
    }
}
