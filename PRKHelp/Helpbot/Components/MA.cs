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
            (int, int, int)[] maItems = GetMAItems(numValue);

            OutputStrings[0] = $"{TextColor}{HighlightColor}{numValue}{EndColor} MA{EndColor} - {EndColor}{HighlightColor}";
            OutputStrings[0] += $"{BuildItemRef(maItems[0].Item1, maItems[0].Item2, maItems[0].Item3, "Martial Artist")} | ";
            OutputStrings[0] += $"{BuildItemRef(maItems[1].Item1, maItems[1].Item2, maItems[1].Item3, "Shade")} | ";
            OutputStrings[0] += $"{BuildItemRef(maItems[2].Item1, maItems[2].Item2, maItems[2].Item3, "Others")}{EndColor}";

            return 1;
        }

        public static (int, int, int)[] GetMAItems(int _skill)
        {
            (int, int, int)[] maItems = new (int, int, int)[3];
            if (_skill > 3000)
                _skill = 3000;

            double qualityLevel = 0;
            switch (_skill)
            {
                case < 1001:
                    qualityLevel = Math.Floor(_skill / 2.0);
                    break;
                case < 2001:
                    qualityLevel = Math.Floor((_skill - 1000) / 2.0);
                    break;
                case < 3001:
                    qualityLevel = Math.Floor((_skill - 2000) / 2.0);
                    break;
            }

            int[] thresholds = [201, 1001, 2001];
            (int, int)[] martialArtistIDs = [(211352, 211353), (211353, 211354), (211357, 211358), (211363, 211364)];
            (int, int)[] shadeIDs = [(211349, 211350), (211350, 211351), (211359, 211360), (211365, 211366)];
            (int, int)[] othersIDs = [(43712, 144745), (144745, 43713), (211355, 211356), (211361, 211362)];

            int range = 0;
            if (_skill <= 2001)
            {
                for (int i = 0; i < thresholds.Length; i++)
                {
                    range = i;
                    if (_skill < thresholds[range])
                        break;
                }
            }
            else
                range = 3;

            maItems[0] = (martialArtistIDs[range].Item1, martialArtistIDs[range].Item2, (int)qualityLevel);
            maItems[1] = (shadeIDs[range].Item1, shadeIDs[range].Item2, (int)qualityLevel);
            maItems[2] = (othersIDs[range].Item1, othersIDs[range].Item2, (int)qualityLevel);
            return maItems;
        }

        public static (int, int, int) GetBrawlItem(int _skill)
        {
            int lowid = 0;
            int highid = 0;
            double qualityLevel = 0;


            if (_skill > 3000)
                _skill = 3000;

            switch (_skill)
            {
                case < 1001:
                    qualityLevel = Math.Floor(_skill / 2.0);
                    break;
                case < 2001:
                    qualityLevel = Math.Floor((_skill - 1000) / 2.0);
                    break;
                case < 3001:
                    qualityLevel = Math.Floor((_skill - 2000) / 2.0);
                    break;
            }

            int[] thresholds = [1001, 2001];
            (int, int)[] brawlIDs = [(70292, 70293), (211401, 211402), (211403, 211404)];

            int range = 0;
            if (_skill <= 2001)
            {
                for (int i = 0; i < thresholds.Length; i++)
                {
                    range = i;
                    if (_skill < thresholds[range])
                        break;
                }
            }
            else
                range = 2;

            return (lowid, highid, (int)qualityLevel);
        }
    }
}
