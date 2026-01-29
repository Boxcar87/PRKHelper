namespace PRKHelper.Helpbot.Components
{
    internal class OE : Component
    {
        public OE()
        {
            ParamTypes.Add(typeof(int));
            ParamSyntax = "/oe 100";
        }

        public override int Process(string[] _params)
        {
            int numValue = int.Parse(_params[0]);
            double oe100 = Math.Floor(numValue / 0.8);
            double oe75 = Math.Floor(numValue / 0.6);
            double oe50 = Math.Floor(numValue / 0.4);
            double oe25 = Math.Floor(numValue / 0.2);
            double oe100min = Math.Floor(numValue * 0.8);
            double oe75min = Math.Floor(numValue * 0.6);
            double oe50min = Math.Floor(numValue * 0.4);
            double oe25min = Math.Floor(numValue * 0.2);

            OutputStrings[0] = $"<a href=\"text://{TextColor}OE data for {ValueColor}>{numValue}{EndColor} skill";
            OutputStrings[0] += $"<br><br>Item requires {ValueColor}{numValue}{EndColor} - mininum required:";
            OutputStrings[0] += $"<br>{Indent}{HighlightColor}>100%{EndColor}: {ValueColor}>{oe100min}{EndColor}";
            OutputStrings[0] += $"<br>{Indent}{Indent}{HighlightColor} 75%{EndColor}: {ValueColor}>{oe75min}{EndColor}";
            OutputStrings[0] += $"<br>{Indent}{Indent}{HighlightColor} 50%{EndColor}: {ValueColor}>{oe50min}{EndColor}";
            OutputStrings[0] += $"<br>{Indent}{Indent}{HighlightColor} 25%{EndColor}: {ValueColor}>{oe25min}{EndColor}";
            OutputStrings[0] += $"<br><br>Your skill is {ValueColor}{numValue}{EndColor} - effectiveness at requirement:";
            OutputStrings[0] += $"<br>{Indent}{HighlightColor}>100%{EndColor}: {ValueColor}>{oe100}{EndColor}";
            OutputStrings[0] += $"<br>{Indent}{Indent}{HighlightColor} 75%{EndColor}: {ValueColor}>{oe75}{EndColor}";
            OutputStrings[0] += $"<br>{Indent}{Indent}{HighlightColor} 50%{EndColor}: {ValueColor}>{oe50}{EndColor}";
            OutputStrings[0] += $"<br>{Indent}{Indent}{HighlightColor} 25%{EndColor}: {ValueColor}>{oe25}{EndColor}";
            OutputStrings[0] += $"{EndColor}\">{oe100min} - {numValue} - {oe100}</a>";

            return 1;
        }
    }
}
