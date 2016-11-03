using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Joney.BasePro.Controllers
{
    public class UtilController : ApiController
    {
        public async Task<string> _PostHttpInfo(string url,string param)
        {
            var result = string.Empty;
            try
            {
                Uri req = new Uri(url);
                HttpContent content = new StringContent(param);
                content.Headers.Add("Access-Control-Allow-Origin", "*");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                HttpClient hc = new HttpClient(handler);
                HttpResponseMessage response = hc.PostAsync(req,content).Result;
                if (response.StatusCode==HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsStringAsync();
                    if (response.ReasonPhrase.ToUpper()=="OK")
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message;
            }
            return result;
        }

        public async Task<string> _GetHttpInfo(string url, string param)
        {
            var result = string.Empty;
            try
            {
                Uri req = new Uri(url);
                HttpContent content = new StringContent(param);
                content.Headers.Add("Access-Control-Allow-Origin", "*");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                HttpClient hc = new HttpClient(handler);
                HttpResponseMessage response = hc.GetAsync(req).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = await response.Content.ReadAsStringAsync();
                    if (response.ReasonPhrase.ToUpper() == "OK")
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message;
            }
            return result;
        }
    }
}
