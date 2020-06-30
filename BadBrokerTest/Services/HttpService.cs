using BadBrokerTest.Interfaces;
using System.IO;
using System.Net;
using System.Text;

namespace BadBrokerTest.Services
{
    public class HttpService : IHttpService
    {
        public string GetResponse(string url) {
            var result = "";
            var request = WebRequest.Create(url);
            try {
                var response = (HttpWebResponse)request.GetResponse();

                using (var responseStream = response.GetResponseStream()) {
                    var responseEncoding = Encoding.Default;
                    if (response.CharacterSet == "ISO-8859-1") {
                        responseEncoding = Encoding.GetEncoding("windows-1251");
                    } else if (!string.IsNullOrEmpty(response.CharacterSet)) {
                        responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                    }

                    using (var reader = new StreamReader(responseStream, responseEncoding)) {
                        result = reader.ReadToEnd();
                    }
                }
                response.Close();

            } catch (WebException) {
                throw;
            }

            return result;
        }
    }
}