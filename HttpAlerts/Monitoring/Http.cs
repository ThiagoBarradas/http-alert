using HttpAlerts.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Linq;
using System.Net;

namespace HttpAlerts.Monitoring
{
    public class Http 
    {
        private RestClient RestClient { get; set; }

        private HttpConfiguration HttpConfiguration { get; set; }

        public Http(HttpConfiguration httpConfiguration)
        {
            this.HttpConfiguration = httpConfiguration;
        }

        public string GetResponse(string logPrefix)
        {
            var restClient = new RestClient(this.HttpConfiguration.Url);

            if (!string.IsNullOrWhiteSpace(this.HttpConfiguration.User) ||
                !string.IsNullOrWhiteSpace(this.HttpConfiguration.Pass))
            {
                restClient.Authenticator = new HttpBasicAuthenticator(this.HttpConfiguration.User, this.HttpConfiguration.Pass);
            }

            var restRequest = new RestRequest(Method.GET);

            if (this.HttpConfiguration.TimeoutSeconds > 0)
            {
                restRequest.Timeout = this.HttpConfiguration.TimeoutSeconds * 1000;
            }
            
            if (this.HttpConfiguration.Headers?.Any() == true)
            {
                foreach(var header in this.HttpConfiguration.Headers)
                {
                    restRequest.AddHeader(header.Key, header.Value);
                }
            }

            var restResponse = restClient.Execute(restRequest);

            if (restResponse.ErrorException != null || restResponse.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("{0}### MONITORING ERROR!", logPrefix);
                Console.WriteLine("{0}ErrorException: {1}", logPrefix, restResponse.ErrorException?.Message);
                Console.WriteLine("{0} HTTP.GetResponse() - StatusCode: {1}", logPrefix, restResponse.StatusDescription);
                return null;
            }

            return restResponse.Content;
        }
    }
}
