using HttpAlerts.Alerts;
using HttpAlerts.Models;
using HttpAlerts.Monitoring;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpAlerts
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("================ HTTP Alerts Begin ====================");
            Console.WriteLine("Start! > {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                var ymlConfig = Configuration.FromYml();
                var channels = ymlConfig.Notifications;

                Parallel.ForEach(ymlConfig.HttpConfigs,
                    config => { Execute(config, channels); });
            }
            catch (Exception ex)
            {
                Console.WriteLine("### Ooops! Exception: {0}", ex.Message);
            }

            Console.WriteLine("Finish! > {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("================ HTTP Alerts End ====================");
            Console.WriteLine();
        }

        public static void Execute(HttpConfiguration httpConfig, List<NotificationChannel> channels)
        {
            var logPrefix = string.Format("[{0}] ", httpConfig.Code);

            Console.WriteLine("{0}================ HTTP Monitoring Begin ====================", logPrefix);
            Console.WriteLine("{0}Start! > {1}", logPrefix, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("{0}Url: {1}", logPrefix, httpConfig.Url);

            Http http = new Http(httpConfig);

            try
            {
                var response = http.GetResponse(logPrefix);
                JObject responseJObj = JObject.Parse(response);

                foreach (var rule in httpConfig.Rules)
                {
                    List<Alert> alerts = channels
                       .Where(channel => rule.AlertIn.Contains(channel.Name))
                       .Select(channel => new Alert(channel))
                       .ToList();

                    var ruleLogPrefix = string.Format("{0}[{1}] ", logPrefix, rule.Condition);

                    try
                    {
                        var isValid = rule.IsValid(responseJObj);

                        if (!isValid)
                        {
                            var title = rule.GetErroTitleValue(responseJObj);
                            var message = rule.GetErroMessageValue(responseJObj);
                            
                            alerts.AlertMe(title, message, ruleLogPrefix);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (rule.AlertWhenException)
                        {
                            var title = string.Format("{0} Rule Exception:", ruleLogPrefix);
                            alerts.AlertMe(title, ex.Message, ruleLogPrefix);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (httpConfig.AlertWhenException)
                {
                    List<Alert> alerts = channels
                       .Where(channel => httpConfig.AlertIn.Contains(channel.Name))
                       .Select(channel => new Alert(channel))
                       .ToList();

                    var title = string.Format("{0} Config Exception: ", logPrefix);
                    alerts.AlertMe(title, ex.Message, logPrefix);
                }

                Console.WriteLine("{0}### Ooops! Exception: {1}", logPrefix, ex.Message);
            }

            Console.WriteLine("{0}================ HTTP Monitoring End ====================", logPrefix);
        }

        public static void AlertMe(this List<Alert> alerts, string title, string content, string logPrefix)
        {
            Parallel.ForEach(alerts,
                alert => { alert.AlertMe(title, content, logPrefix); });
        }
    }
}
