using System.Data;
using System.Diagnostics;
using PRKHelp.Settings;
using static System.Windows.Forms.AxHost;

namespace PRKHelper.Helpbot.Components
{
    public class Character : Component
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
        public Character(DB _db) // Pass DB reference in from route if needed
        {
            // Base class will perform basic validation on params
            DB = _db;
            ParamSyntax = "/character string string (insert item)";
        }

        // If you have variable inputs you can override ValidateParams, otherwise this performs basic param validation
        public override (int, string) ValidateParams(string[] _params)
        {
            if (_params.Length == 0)
            {
                return (1, $"Accepted");
            }
            if (_params[0].ToLower() == "init" || _params[0].ToLower() == "crit" || _params[0].ToLower() == "ar" || _params[0].ToLower() == "dmg" || _params[0].ToLower() == "complit" || _params[0].ToLower() == "burst" || _params[0].ToLower() == "fullauto" || _params[0].ToLower() == "flingshot" || _params[0].ToLower() == "fastattack" || _params[0].ToLower() == "brawl")
            {
                if (_params.Length != 2)
                {
                    return (-1, $"Please input skill amount (eg: init 350)");
                }
                if (int.TryParse(_params[1], out _))
                {
                    return (1, "Accepted");
                }
                else
                    return (-1, $"Please input skil amount (eg: init 350)");
            }
            else if (_params[0].ToLower() == "class")
            {
                if (_params.Length != 2)
                {
                    return (-1, $"Please input class as one word (eg: class martialartist)");
                }
                else
                    return (1, "Accepted");
            }
            else if (_params[0].ToLower() == "gear" || _params[0].ToLower() == "plan")
            {
                if (_params.Length < 4)
                {
                    if (_params.Length < 3)
                        return (-1, $"Provide slot and item for that slot(eg: mainhand (insert item))");

                    if (_params[1].ToLower() == "ma")
                    {
                        if (_params[2].ToLower() == "clear")
                        {
                            return (1, "Accepted");
                        }
                        else if (int.TryParse(_params[2], out _))
                        {
                            return (1, "Accepted");
                        }
                        else
                            return (-1, $"Please input MA skill amount (eg: ma 650)");
                    }
                    if (_params[1].ToLower() == "mainhand" || _params[1].ToLower() == "offhand")
                    {
                        if (_params[2] == "clear")
                            return (1, "Accepted");
                        else
                            return (-1, "Provide slot and item for that slot(eg: mainhand (insert item) or mainhand clear)");
                    }
                    else
                        return (-1, $"Provide slot and item for that slot(eg: mainhand (insert item))");
                }
                else if (_params[1].ToLower() == "mainhand" || _params[1].ToLower() == "offhand")
                {
                    if (_params[2] == "<a" || _params[2] == "raw")
                    {
                        return (1, "Accepted");
                    }
                    else
                        return (-1, $"Provide slot and item for that slot(eg: mainhand (insert item) or mainhand clear)");
                }
                else
                    return (-1, $"Please provide a weapon slot (eg: gear mainhand)");
            }
            else
                return (-1, $"Invalid Params");
        }

        // Perform function logic here
        public override int Process(string[] _params)
        {
            int statusCode = 1; // -1 for error 1 for success
            if(_params.Length == 0)
            {
                OutputStrings[0] = DrawCharacter();
            }
            else
            {
                _params[0] = _params[0].ToLower();
                switch (_params[0])
                {
                    case "class":
                        UpdateClass(_params[1]);
                        OutputStrings[0] = $"{TextColor}Updated class to {_params[1]}. Please add martial arts skill again (only relevant for martial arts).";
                        break;
                    case "init":
                    case "crit":
                    case "ar":
                    case "dmg":
                    case "complit":
                    case "flingshot":
                    case "burst":
                    case "fullauto":
                    case "brawl":
                    case "fastattack":
                        int value = int.Parse(_params[1]);
                        // Giving 3% base crit to equation;
                        if (_params[0] == "crit")
                            value += 3;
                        UpdateStat(_params[0], value);
                        OutputStrings[0] = $"{TextColor}Updated {_params[0]} to {_params[1]}";
                        break;
                    case "gear":
                        if (_params[2].ToLower() == "clear")
                        {
                            RemoveWeaponFromGear(_params[1].ToLower());
                            OutputStrings[0] = $"{TextColor}Removed {_params[1]} weapon from gear";
                        }
                        else if (_params[1].ToLower() == "ma")
                        {
                            // Get MA item
                            (int, int, int)[] maItems = MA.GetMAItems(int.Parse(_params[2]));
                            Debug.WriteLine(maItems[1]);
                            string characterClass = SettingsManager.GetClass();
                            switch (characterClass)
                            {
                                case "ma":
                                    AddWeaponToGear("ma", maItems[0]);
                                    break;
                                case "shade":
                                    AddWeaponToGear("ma", maItems[1]);
                                    break;
                                case "other":
                                    AddWeaponToGear("ma", maItems[2]);
                                    break;
                            }
                            OutputStrings[0] = $"{TextColor}Updated martial arts for gear";
                        }
                        else
                        {
                            string slot = _params[1];
                            _params = _params[2..];
                            string itemString = string.Join(" ", _params);
                            itemString = itemString.Replace("\"", "\'");
                            int start = itemString.IndexOf("//") + 2;
                            string clipped = itemString[start..^4];
                            string numberString = clipped[..clipped.IndexOf("\'")];
                            string[] numbers = numberString.Split('/');

                            (int, int, int) weapon = (int.Parse(numbers[0]), int.Parse(numbers[1]), int.Parse(numbers[2]));
                            bool success = AddWeaponToGear(slot.ToLower(), weapon);
                            if (success)
                                OutputStrings[0] = $"{TextColor}Updated {slot} weapon for gear";
                            else
                                OutputStrings[0] = $"{HighlightColor}Invalid weapon given for {slot}. Please check inputs.";
                        }
                        break;
                    case "plan":
                        if (_params[2].ToLower() == "clear")
                        {
                            RemoveWeaponFromPlan(_params[1].ToLower());
                            OutputStrings[0] = $"{TextColor}Removed {_params[1]} weapon from plan";
                        }
                        else if (_params[1].ToLower() == "ma")
                        {
                            // Get MA item
                            (int, int, int)[] maItems = MA.GetMAItems(int.Parse(_params[2]));
                            string characterClass = SettingsManager.GetClass();
                            switch (characterClass)
                            {
                                case "ma":
                                    AddWeaponToPlan("ma", maItems[0]);
                                    break;
                                case "shade":
                                    AddWeaponToPlan("ma", maItems[1]);
                                    break;
                                case "other":
                                    AddWeaponToPlan("ma", maItems[2]);
                                    break;
                            }
                            OutputStrings[0] = $"{TextColor}Updated martial arts for plan";
                        }
                        else
                        {
                            string[] numbers;
                            string slot = _params[1];
                            if (_params[2] == "raw")
                            {
                                numbers = [_params[3], _params[4], _params[5]];
                            }
                            else
                            {
                                _params = _params[2..];
                                string itemString = string.Join(" ", _params);
                                itemString = itemString.Replace("\"", "\'");
                                int start = itemString.IndexOf("//") + 2;
                                string clipped = itemString[start..^4];
                                string numberString = clipped[..clipped.IndexOf("\'")];
                                numbers = numberString.Split('/');
                            }

                            (int, int, int) weapon = (int.Parse(numbers[0]), int.Parse(numbers[1]), int.Parse(numbers[2]));
                            bool success = AddWeaponToPlan(slot.ToLower(), weapon);
                            if (success)
                                OutputStrings[0] = $"{TextColor}Updated {slot} weapon for plan";
                            else
                                OutputStrings[0] = $"{HighlightColor}Invalid weapon given for {slot}. Please check inputs.";
                        }
                        break;
                }
            }
            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        private string DrawCharacter()
        {
            string hrefString = $"<a href=\"text://{HighlightColor}Character{EndColor}<br><br>Stats:<br>";

            Dictionary<string, int> stats = new Dictionary<string, int>() {

                ["AR"] = SettingsManager.GetStat("AR"),
                ["Init"] = SettingsManager.GetStat("Init"),
                ["Crit"] = SettingsManager.GetStat("Crit"),
                ["AddDmg"] = SettingsManager.GetStat("Dmg"),
                ["FlingShot"] = SettingsManager.GetStat("Flingshot"),
                ["Burst"] = SettingsManager.GetStat("Burst"),
                ["FullAuto"] = SettingsManager.GetStat("Fullauto"),
                ["FastAttack"] = SettingsManager.GetStat("Fastattack"),
                ["Brawl"] = SettingsManager.GetStat("Brawl"),
                ["CompLit"] = SettingsManager.GetStat("Complit")
            };

            (int, int, int)[] gearData = SettingsManager.GetGear();
            (int, int, int)[] planData = SettingsManager.GetPlan();
            
            foreach (KeyValuePair<string,int> stat in stats)
            {
                hrefString += $"{Indent}{stat.Key} - {ValueColor}{stat.Value}{EndColor} | {RedColor}[<a href='chatcmd:///character {stat.Key.ToLower()} 0'>X</a>]{EndColor}<br>";
            }

            hrefString += $"<br>Gear Weapons:<br>";
            for (int g=0; g<gearData.Length; g++)
            {
                string slot = "Mainhand";
                switch (g)
                {
                    case 1:
                        slot = "Offhand";
                        break;
                    case 2:
                        slot = "MA";
                        break;
                }
                if (gearData[g].Item1 > 0)
                {
                    hrefString += $"{Indent}{slot} - {GetWeaponAsHref(gearData[g].Item1, gearData[g].Item3)} | {RedColor}[<a href='chatcmd:///character gear {slot.ToLower()} clear'>X</a>]{EndColor}<br>";
                }
            }
            hrefString += $"<br>Plan Weapons:<br>";
            for (int p = 0; p < planData.Length; p++)
            {
                string slot = "Mainhand";
                switch (p)
                {
                    case 1:
                        slot = "Offhand";
                        break;
                    case 2:
                        slot = "MA";
                        break;
                }
                if (planData[p].Item1 > 0)
                {
                    hrefString += $"{Indent}{slot} - {GetWeaponAsHref(planData[p].Item1, planData[p].Item3)} | {RedColor}[<a href='chatcmd:///character plan {slot.ToLower()} clear'>X</a>]{EndColor}<br>";
                }
            }
            hrefString += "\">View Character</a>";
            return hrefString;
        }

        private bool AddWeaponToGear(string _slot, (int, int, int) _weapon)
        {
            if(!CheckIfValidWeapon(_weapon.Item1))
                return false;
            (int, int, int)[] weapons = SettingsManager.GetGear();
            switch (_slot)
            {
                case "mainhand":
                    weapons[0] = _weapon;
                    break;
                case "offhand":
                    weapons[1] = _weapon;
                    break;
                case "ma":
                    weapons[2] = _weapon;
                    break;
            }
            SettingsManager.UpdateGear(weapons);
            return true;
        }

        private void RemoveWeaponFromGear(string _slot)
        {
            (int, int, int)[] weapons = SettingsManager.GetGear();
            switch (_slot)
            {
                case "mainhand":
                    weapons[0] = (0, 0, 0);
                    break;
                case "offhand":
                    weapons[1] = (0, 0, 0);
                    break;
                case "ma":
                    weapons[2] = (0, 0, 0);
                    break;
            }
            SettingsManager.UpdateGear(weapons);
        }

        private bool AddWeaponToPlan(string _slot, (int, int, int) _weapon)
        {
            if (!CheckIfValidWeapon(_weapon.Item1))
                return false;
            (int, int, int)[] weapons = SettingsManager.GetPlan();
            switch (_slot)
            {
                case "mainhand":
                    weapons[0] = _weapon;
                    break;
                case "offhand":
                    weapons[1] = _weapon;
                    break;
                case "ma":
                    weapons[2] = _weapon;
                    break;
            }
            SettingsManager.UpdatePlan(weapons);
            return true;
        }

        private void RemoveWeaponFromPlan(string _slot)
        {
            (int, int, int)[] weapons = SettingsManager.GetPlan();
            switch (_slot)
            {
                case "mainhand":
                    weapons[0] = (0, 0, 0);
                    break;
                case "offhand":
                    weapons[1] = (0, 0, 0);
                    break;
                case "ma":
                    weapons[2] = (0, 0, 0);
                    break;
            }
            SettingsManager.UpdatePlan(weapons);
        }

        private void UpdateStat(string _stat, int _value)
        {
            SettingsManager.UpdateStat(_stat, _value);
        }
        private void UpdateClass(string _class)
        {
            switch (_class)
            {
                case "martialartist":
                case "ma":
                    _class = "ma";
                    break;
                case "shade":
                    _class = "shade";
                    break;
                default:
                    _class = "other";
                    break;
            }
            SettingsManager.UpdateClass(_class);
            RemoveWeaponFromGear("ma");
            RemoveWeaponFromPlan("ma");
        }

        private bool CheckIfValidWeapon(int _lowid)
        {
            string query = $"Select * FROM WeaponStats WHERE {_lowid} == lowid";
            return DB.QueryIfWeaponExists(query);
        }

        private string GetWeaponAsHref(int _lowid, int _ql)
        {
            string itemQuery = "";
            itemQuery = $"SELECT * FROM Items WHERE lowid == {_lowid} AND {_ql} BETWEEN lowql AND highql";
            AOItem item = DB.QueryItemByIDs(itemQuery);
            return BuildItemRef(item.lowid, item.highid, _ql, item.name);
        }
    }
}
