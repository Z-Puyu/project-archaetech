using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectArchaetech.util
{
    public static class StringFormatter {
        public static string Format(string inputString, Dictionary<string, string> dictionary) {
            Regex regex = new Regex("{(.*?)}");
            MatchCollection matches = regex.Matches(inputString);
            foreach (Match match in matches.Cast<Match>()) {
                var valueWithoutBrackets = match.Groups[1].Value;
                var valueWithBrackets = match.Value;

                if(dictionary.TryGetValue(valueWithoutBrackets, out string value)) {
                    inputString = inputString.Replace(valueWithBrackets, value);
                }
            }

            return inputString;
        }
    }
}