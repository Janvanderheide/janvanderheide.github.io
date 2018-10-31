using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SLSInvoicer.classes
{
    public class BaseconeResponse<T>
    {
        public HttpContentHeaders Headers { get; set; }
        public T Body { get; set; }
        public ErrorCodeObject Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess => Error == null;
    }

    public class ErrorCodeObject
    {
        public string Message { get; set; }
        public string Code { get; set; }

        //[JsonProperty("_metadata")]
        //public List<Metadata> Metadata { get; set; }

        [JsonProperty("_moreinfo")]
        public string MoreInfo { get; set; }
    }

    public static class HttpResponseMessageExtensions
    {
        public static async Task<BaseconeResponse<T>> ReadAsJson<T>(this HttpResponseMessage httpResponse)
        {
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            var response = new BaseconeResponse<T>
            {
                StatusCode = httpResponse.StatusCode
            };

            if (httpContent == "" || httpResponse.StatusCode == HttpStatusCode.NotFound ||
                httpResponse.StatusCode == HttpStatusCode.Forbidden)
            {
                // nothing to serialize.
            }
            else if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                var basecone400Response = JsonConvert.DeserializeObject<Basecone400Response>(httpContent);

                response.Error = new ErrorCodeObject
                {
                    Code = basecone400Response.Code,
                    Message = basecone400Response.Message,
                    MoreInfo = basecone400Response.MoreInfo,
                  //  Metadata = basecone400Response.Metadata?.ToList()
                };
            }
            //else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            //{
            //    var basecone400Response = JsonConvert.DeserializeObject<Basecone400Response>(httpContent);

            //    response.Error = new ErrorCodeObject
            //    {
            //        Code = basecone400Response.Code,
            //        Message = basecone400Response.Message,
            //        MoreInfo = basecone400Response.MoreInfo,
            //        Metadata = basecone400Response.Metadata?.ToList()
            //    };
            //}
            //else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError)
            //{
            //    var basecone500Response = JsonConvert.DeserializeObject<Basecone500Response>(httpContent);

            //    response.Error = new ErrorCodeObject
            //    {
            //        Message = basecone500Response.Message,
            //        Metadata = new List<Metadata> { basecone500Response.Metadata }
            //    };
            //}
            else if (httpResponse.IsSuccessStatusCode)
            {
                response.Body = JsonConvert.DeserializeObject<T>(httpContent);
            }
            else
            {
                throw new Exception($"Unknown status code: {httpResponse.StatusCode}");
            }
            response.Headers = httpResponse.Content.Headers;
            return response;
        }

        public static async Task<BaseconeResponse<Stream>> ReadAsStream(this HttpResponseMessage httpResponse)
        {
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            var response = new BaseconeResponse<Stream>
            {
                StatusCode = httpResponse.StatusCode
            };

            if (httpContent == "" || httpResponse.StatusCode == HttpStatusCode.NotFound ||
                httpResponse.StatusCode == HttpStatusCode.Forbidden)
            {
                // nothing to serialize.
            }
            //else if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
            //{
            //    var basecone400Response = JsonConvert.DeserializeObject<Basecone400Response>(httpContent);

            //    response.Error = new ErrorCodeObject
            //    {
            //        Code = basecone400Response.Code,
            //        Message = basecone400Response.Message,
            //        MoreInfo = basecone400Response.MoreInfo,
            //        Metadata = basecone400Response.Metadata.ToList()
            //    };
            //}
            //else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError)
            //{
            //    var basecone500Response = JsonConvert.DeserializeObject<Basecone500Response>(httpContent);

            //    response.Error = new ErrorCodeObject
            //    {
            //        Message = basecone500Response.Message,
            //        Metadata = new List<Metadata> { basecone500Response.Metadata }
            //    };
            //}
            //else if (httpResponse.IsSuccessStatusCode)
            //{
            //    response.Body = await httpResponse.Content.ReadAsStreamAsync();
            //}
            else
            {
                throw new Exception($"Unknown status code: {httpResponse.StatusCode}");
            }
            response.Headers = httpResponse.Content.Headers;
            return response;
        }
    }
}
