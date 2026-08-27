using System.Diagnostics;
using PRKHelp.Settings;

namespace PRKHelper.Helpbot.Components
{
    public class VendorValue : Component
    {
        //string TextColor
        //string ValueColor
        //string HighlightColor
        //string RedColor
        //string EndColor
        //string Indent

        //List<Type> ParamTypes

        //List<string> OutputStrings; // Inherited object retrieved for response by Route()
        DB DB;
        public VendorValue(DB _db) // Pass DB reference in from route if needed
        {
            // Base class will perform basic validation on params
            DB = _db;
            ParamSyntax = "/vendor (insert item)";
        }

        // If you have variable inputs you can override ValidateParams, otherwise this performs basic param validation
        public override (int, string) ValidateParams(string[] _params)
        {
            if (_params.Length == 0)
                return (-1, "Please provide an item");
            if (_params[0] != "<a")
                return (-1, "Please insert an item");
            else
                return (1, "Accepted");
        }

        // Perform function logic here
        public override int Process(string[] _params)
        {
            int statusCode = 1; // -1 for error 1 for success

            // Sell value ratio from item value varies a bit to much to be succinctly predicatble. Sell value seems to be around 4% of actual value, potentially going as low as 3.5% or maybe even lower.
            // Comp lit increases sell value by 1% for every 40 CL. 40CL thresholds must be crossed to gain the additional sell value

            string itemString = string.Join(" ", _params);
            itemString = itemString.Replace("\"", "\'");
            int start = itemString.IndexOf("//") + 2;
            string clipped = itemString[start..^4];
            string numberString = clipped[..clipped.IndexOf("\'")];
            string[] numbers = numberString.Split('/');

            int value = GetItemValue(int.Parse(numbers[0]), int.Parse(numbers[2]));
            AOItem item = GetItem(int.Parse(numbers[0]), int.Parse(numbers[2]));
            int compLit = SettingsManager.GetStat("complit");
            int bonus = compLit / 40;
            double multiplier = 1 + (bonus / 100.00);
            int price = (int)(value * 0.037 * multiplier);

            OutputStrings[0] = $"{TextColor}{item.name} is worth approximately - Base {ValueColor}{price}{EndColor} | NPCs {ValueColor}{(int)price*1.25}{EndColor} | Omni {ValueColor}{(int)price *1.5}{EndColor} | Trader {ValueColor}{(int)price *1.75}{EndColor} | Omni Trader {ValueColor}{(int)price *2}{EndColor}";

            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        private int GetItemValue(int _lowid, int _ql)
        {
            string interpolationProgress = $"(({_ql}*1.000) - lowql) / (highql - lowql)";
            string query = "SELECT ";
            query += $"(lowvalue + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (highvalue - lowvalue))) AS value ";
            query += $"FROM Items WHERE lowid == {_lowid} AND {_ql} BETWEEN lowql AND highql";
            return DB.QueryItemValue(query);
        }

        private AOItem GetItem(int _lowid, int _ql)
        {
            string query = "";
            query = $"SELECT * FROM Items WHERE lowid == {_lowid} AND {_ql} BETWEEN lowql AND highql";

            return DB.QueryItemByIDs(query);
        }
    }
}
