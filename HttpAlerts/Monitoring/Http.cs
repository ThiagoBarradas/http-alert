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

        public string GetResponse()
        {
            var restClient = new RestClient(this.HttpConfiguration.Url);

            if (!string.IsNullOrWhiteSpace(this.HttpConfiguration.User) ||
                !string.IsNullOrWhiteSpace(this.HttpConfiguration.Pass))
            {
                restClient.Authenticator = new HttpBasicAuthenticator(this.HttpConfiguration.User, this.HttpConfiguration.Pass);
            }

            var restRequest = new RestRequest(Method.GET);

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
                Console.WriteLine("### MONITORING ERROR!");
                Console.WriteLine("ErrorException: {0}", restResponse.ErrorException?.Message);
                Console.WriteLine("StatusCode: {0}", restResponse.StatusDescription);
                return null;
            }

            return restResponse.Content;
        }
    }
}
