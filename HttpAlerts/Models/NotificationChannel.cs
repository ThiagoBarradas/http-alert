namespace HttpAlerts.Models
{
    public class NotificationChannel
    {
        public string Name { get; set; }

        public PushoverConfiguration Pushover { get; set; }

        public SlackConfiguration Slack { get; set; }
    }
}
