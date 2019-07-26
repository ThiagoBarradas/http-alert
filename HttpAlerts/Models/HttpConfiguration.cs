using System.Collections.Generic;

namespace HttpAlerts.Models
{
    public class HttpConfiguration
    {
        public string Code { get; set; }

        public string Url { get; set; }

        public string User { get; set; }

        public string Pass { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public bool StopInFirstAlert { get; set; }
        
        public bool AlertWhenException { get; set; }

        public string[] AlertIn { get; set; }

        public int TimeoutSeconds { get; set; }

        public List<RuleConfiguration> Rules { get; set; }
    }
}
