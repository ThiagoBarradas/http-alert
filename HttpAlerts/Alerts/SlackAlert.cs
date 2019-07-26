using HttpAlerts.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace HttpAlerts.Alerts
{
    public class SlackAlert
    {
        private SlackConfiguration SlackConfiguration { get; set; }

        public SlackAlert(SlackConfiguration slackConfiguration)
        {
            this.SlackConfiguration = slackConfiguration;
        }

        public void AlertMe(string title, string content, string logPrefix = "")
        {
            if (this.SlackConfiguration == null)
            {
                return;
            }

            var message = new SlackMessage(title, content);
            var restClient = new RestClient(this.SlackConfiguration.Url);
            RestRequest restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(message);

            IRestResponse restResponse = restClient.Execute(restRequest);

            if (restResponse.ErrorException != null || restResponse.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("{0}Slack Notification Failed :(", logPrefix);
                Console.WriteLine("{0}ErrorException: {1}", logPrefix, restResponse.ErrorException?.Message);
                Console.WriteLine("{0}StatusCode: {1}", logPrefix, restResponse.StatusDescription);
            }
        }
    }

    public class SlackAttachment
    {
        public string text { get; set; }

        public string color { get; set; }
        
        public string title { get; set; }

        public List<string> mrkdwn_in { get; set; }
    }
    
    public class SlackMessage
    {
        public SlackMessage(string title, string content)
        {
            this.username = "HttpAlert";
            this.icon_url = new Uri("https://i.imgur.com/aIeGdDs.png");
            this.mrkdwn = true;

            this.attachments = new List<SlackAttachment>();
            this.attachments.Add(new SlackAttachment()
            {
                color = "danger",
                title = title,
                text = content
            });
        }

        public string text { get; set; }

        public string channel { get; set; }

        public string username { get; set; }

        public Uri icon_url { get; set; }

        public bool mrkdwn { get; set; }

        public List<SlackAttachment> attachments { get; set; }
    }
}
