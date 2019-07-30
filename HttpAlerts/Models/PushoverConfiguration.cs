namespace HttpAlerts.Models
{
    public class PushoverConfiguration
    {
        public string Token { get; set; }

        public string User { get; set; }

        public int? Priority { get; set; }
    }
}
