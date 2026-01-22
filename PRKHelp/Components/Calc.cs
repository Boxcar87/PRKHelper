using System.Data;
using System.Text.RegularExpressions;

namespace PRKHelp.Components
{
    public class Calc : Component
    {
        public Calc() 
        {
            ParamTypes.Add(typeof(string));
            ParamSyntax = "/calc 1+1*(3/1)";
        }

        public override (int, string) SpecificParamChecks(string[] _params)
        {
            int maxInputLength = 200; //Estimated limit to keep response under 1kb //This is really just to throttle the size of response from script, may not be an issue.

            if (_params[0].Length > maxInputLength)
                return (-1, "/text Error: Input formula too large.");
            return (1, "true");
        }

        public override int Process(string[] _params)
        {
            string formula = _params[0];

            //Style input string         
            string pattern = @"[+\-*/%()]";

            OutputStrings[0] = $"/text Equation: {TextColor}";

            //Preliminary size check

            foreach (char character in formula)
            {
                string _tempString = character.ToString();
                if (Regex.Match(_tempString, pattern).Success)
                {
                    _tempString = $"{ValueColor}{_tempString}{EndColor}";
                }
                OutputStrings[0] += _tempString;
            }
            OutputStrings[0] += $"{EndColor}";

            //Perform some wizardy
            double? product = Evaluate(formula);

            if (product.HasValue)
                OutputStrings[0] += $"{ValueColor} = {EndColor}{HighlightColor}{product.ToString()}{EndColor}";
            else
                return -1;
            return 1;
        }

        public static double? Evaluate(string expression)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expression"]);
            }

            catch
            {
                return null;
            }
        }
    }
}
