using HttpAlerts.Models;
using RestSharp;
using System;
using System.Net;

namespace HttpAlerts.Alerts
{
    public class PushoverAlert
    {
        private static string PushoverAPI = "https://api.pushover.net/1/messages.json";

        private PushoverConfiguration PushoverConfiguration { get; set; }

        public PushoverAlert(PushoverConfiguration pushoverConfiguration)
        {
            this.PushoverConfiguration = pushoverConfiguration;
        }

        public void AlertMe(string title, string content, string logPrefix = "")
        {
            if (this.PushoverConfiguration == null)
            {
                return;
            }

            int? expire = null;
            int? retry = null;
            if (this.PushoverConfiguration.Priority == 2)
            {
                expire = this.PushoverConfiguration.Expire ?? 10800;
                retry = this.PushoverConfiguration.Retry ?? 60;
            }

            var restClient = new RestClient(PushoverAPI);
            RestRequest restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(new
            {
                token = this.PushoverConfiguration.Token,
                user = this.PushoverConfiguration.User,
                priority = (this.PushoverConfiguration.Priority ?? -1),
                expire,
                retry,
                title = title,
                message = content
            });

            IRestResponse restResponse = restClient.Execute(restRequest);

            if (restResponse.ErrorException != null || restResponse.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("{0}Pushover Notification Failed :(", logPrefix);
                Console.WriteLine("{0}ErrorException: {1}", logPrefix, restResponse.ErrorException?.Message);
                Console.WriteLine("{0}StatusCode: {1}", logPrefix, restResponse.StatusDescription);
            }
        }
    }
}
