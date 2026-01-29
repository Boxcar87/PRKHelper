namespace PRKHelper.Helpbot.Components
{
    public class MA : Component
    {
        public MA()
        {
            ParamTypes.Add(typeof(int));
            ParamSyntax = "/mafist 100";
        }
        public override int Process(string[] _params)
        {
            int numValue = int.Parse(_params[0]);
            if (numValue > 3000)
                numValue = 3000;

            double qualityLevel = 0;
            switch (numValue)
            {
                case < 1001:
                    qualityLevel = Math.Floor(numValue / 2.0);
                    break;
                case < 2001:
                    qualityLevel = Math.Floor((numValue - 1000) / 2.0);
                    break;
                case < 3001:
                    qualityLevel = Math.Floor((numValue - 2000) / 2.0);
                    break;
            }

            int[] thresholds = [200, 1000, 2000];
            (int, int)[] martialArtistIDs = [(211352, 211353), (211353, 211354), (211357, 211358), (211363, 211364)];
            (int, int)[] shadeIDs = [(211349, 211350), (211350, 211351), (211359, 211360), (211365, 211366)];
            (int, int)[] othersIDs = [(43712, 144745), (144745, 43713), (211355, 211356), (211361, 211362)];

            int range = 0;
            if (numValue < 2001)
            {
                for (int i = 0; i < thresholds.Length; i++)
                {
                    range = i;
                    if (numValue < thresholds[range])
                        break;
                }
            }
            else
                range = 3;

            OutputStrings[0] = $"{TextColor}{HighlightColor}{numValue}{EndColor} MA{EndColor} - {EndColor}{HighlightColor}";
            OutputStrings[0] += $"{BuildItemRef(martialArtistIDs[range].Item1, martialArtistIDs[range].Item2, (int)qualityLevel, "Martial Artist")} | ";
            OutputStrings[0] += $"{BuildItemRef(shadeIDs[range].Item1, shadeIDs[range].Item2, (int)qualityLevel, "Shade")} | ";
            OutputStrings[0] += $"{BuildItemRef(othersIDs[range].Item1, othersIDs[range].Item2, (int)qualityLevel, "Others")}{EndColor}";

            return 1;
        }
    }
}
