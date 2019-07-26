using Flee.PublicTypes;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;

namespace HttpAlerts.Models
{
    public class RuleConfiguration
    {
        public bool AlertWhenException { get; set; }

        public string Condition { get; set; }

        public string ErrorTitle { get; set; }

        public string ErrorMessage { get; set; }

        public string[] AlertIn { get; set; }

        public bool IsValid(JObject obj)
        {
            var expression = this.ReplaceParameters(obj, this.Condition);

            ExpressionContext context = new ExpressionContext();

            IGenericExpression<bool> e = context.CompileGeneric<bool>(expression);
            bool conditionFailed = e.Evaluate();

            return (conditionFailed == false);
        }

        private string ReplaceParameters(JObject obj, string text)
        {
            var parameters = GetStringParameters(text);
            foreach (var parameter in parameters)
            {
                var newValue = GetValue(obj, parameter);
                text = text.Replace(parameter, newValue);
            }

            return text;
        }

        private string[] GetStringParameters(string text)
        {
            var regex = new Regex(@"(?:(\{){1}[\w\.\-\[\]]+(\}){1})");
            var parameters = regex.Matches(text);

            return parameters.Select(r => r.Value).ToArray();
        }

        private string GetValue(JObject obj, string path)
        {
            path = path.TrimStart('{').TrimEnd('}');
            var token = obj.SelectToken(path);
            return token.ToString();
        }

        public string GetErroTitleValue(JObject obj)
        {
            return this.ReplaceParameters(obj, this.ErrorTitle);
        }

        public string GetErroMessageValue(JObject obj)
        {
            return this.ReplaceParameters(obj, this.ErrorMessage);
        }
    }
}
