namespace HttpAlerts.Models
{
    public class PushoverConfiguration
    {
        public string Token { get; set; }

        public string User { get; set; }

        public int? Priority { get; set; }

        public int? Expire { get; set; }

        public int? Retry { get; set; }
    }
}
