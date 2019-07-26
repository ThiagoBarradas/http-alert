using HttpAlerts.Models;
using System;

namespace HttpAlerts.Alerts
{
    public class Alert
    {
        private SlackAlert SlackAlert { get; set; }

        private PushoverAlert PushoverAlert { get; set; }

        private NotificationChannel Channel { get; set; }

        public Alert(NotificationChannel channel)
        {
            this.Channel = channel;

            this.SlackAlert = new SlackAlert(channel.Slack);
            this.PushoverAlert = new PushoverAlert(channel.Pushover);
        }

        public void AlertMe(string title, string content, string logPrefix = "")
        {
            logPrefix = string.Format("{0}[{1}] ", logPrefix, this.Channel.Name);

            Console.WriteLine("{0}Channel Alert! | F****CK!!!!", logPrefix);
            Console.WriteLine("{0}Channel Alert! | - {1}", logPrefix, title);
            Console.WriteLine("{0}Channel Alert! | - {1}", logPrefix, content);

            if (this.Channel.Slack != null)
            {
                this.SlackAlert.AlertMe(title, content, logPrefix);
            }

            if (this.Channel.Pushover != null)
            {
                this.PushoverAlert.AlertMe(title, content, logPrefix);
            }
        }
    }
}
