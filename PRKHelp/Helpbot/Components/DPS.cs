using System.Diagnostics;
using PRKHelp.Settings;
namespace PRKHelper.Helpbot.Components
{
    public class Weapon
    {
        internal int min { get; set; }
        internal int max  { get; set; }
        internal int crit { get; set; }
        internal int attack { get; set; }
        internal int recharge { get; set; }
    }   

    public class DPS : Component
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
        public DPS(DB _db) // Pass DB reference in from route if needed
        {
            // Base class will perform basic validation on params
            DB = _db;
            ParamSyntax = "/dps";
            ParamTypes.Add(typeof(string));
            LoadWeaponStats();
        }

        // If you have variable inputs you can override ValidateParams, otherwise this performs basic param validation
        public override (int, string) ValidateParams(string[] _params)
        {
            return SpecificParamChecks(_params);
        }

        // Use this function override to append additional param checks if needed
        public override (int, string) SpecificParamChecks(string[] _params)
        {
            int statusCode = -1; // -1 for error 1 for success
            string statusMessage = "Valid params are (gear, plan, compare)";

            if (_params[0].ToLower() == "gear" ||  _params[0].ToLower() == "plan" || _params[0].ToLower() == "compare")
            {
                statusCode = 1;
                statusMessage = "Accepted";
            }

            return (statusCode, statusMessage);
        }

        // Perform function logic here
        public override int Process(string[] _params)
        {
            int statusCode = 1; // -1 for error 1 for success

            int ar = SettingsManager.GetStat("AR");
            int init = SettingsManager.GetStat("Init");
            int critRate = SettingsManager.GetStat("Crit");
            int addDmg = SettingsManager.GetStat("Dmg");

            _params[0] = _params[0].ToLower();

            string dpmGear = "";
            string dpmGearCapped = "";
            string dpmPlan = "";
            string dpmPlanCapped = "";
            (List<Weapon> gearWeapons, List<string> gearhRef) = GetGear();
            (List<Weapon> planWeapons, List<string> planhRef) = GetPlan();


            switch (_params[0])
            {
                case "gear":
                    (dpmGear, dpmGearCapped) = GetDPM(gearWeapons, ar, init, critRate, addDmg);
                    OutputStrings[0] = $"{TextColor}Gear DPM - {dpmGear} | DPM(AC Capped) - {dpmGearCapped}";
                    break;
                case "plan":
                    (dpmPlan, dpmPlanCapped) = GetDPM(planWeapons, ar, init, critRate, addDmg);
                    OutputStrings[0] = $"{TextColor}Plan DPM - {dpmPlan} | DPM(AC Capped) - {dpmPlanCapped}";
                    break;
                case "compare":
                    (dpmGear, dpmGearCapped) = GetDPM(gearWeapons, ar, init, critRate, addDmg);
                    OutputStrings[0] = $"{TextColor}Gear DPM - {dpmGear} | DPM (AC Capped) - {dpmGearCapped}";
                    (dpmPlan, dpmPlanCapped) = GetDPM(planWeapons, ar, init, critRate, addDmg);
                    OutputStrings.Add($"{TextColor}Plan DPM - {dpmPlan} | DPM (AC Capped) - {dpmPlanCapped}");
                    break;
            }
            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        private (List<Weapon>, List<string>) GetGear()
        {
            (int,int,int)[] gearData = SettingsManager.GetGear();
            List<Weapon> weapons = new();
            List<string> hrefWeapons = new();
            foreach ((int, int, int) weapon in gearData)
            {
                if (weapon.Item1 > 0)
                {
                    // Use min and max values for item to calculate interpolation based on QL
                    string interpolationProgress = $"(({weapon.Item3}*1.00) - lowql) / (highql - lowql)";
                    string query = "SELECT ";
                    query += $"(minlow + ROUND({interpolationProgress} * (minhigh - minlow))) AS min, ";
                    query += $"(maxlow + ROUND({interpolationProgress} * (maxhigh - maxlow))) AS max, ";
                    query += $"(critlow + ROUND({interpolationProgress} * (crithigh - critlow))) AS crit, ";
                    query += $"(attacklow + ROUND({interpolationProgress} * (attackhigh - attacklow))) AS attack, ";
                    query += $"(rechargelow + ROUND({interpolationProgress} * (rechargehigh - rechargelow))) AS recharge ";
                    query += $"FROM WeaponStats WHERE lowid == {weapon.Item1} AND {weapon.Item3} BETWEEN lowql AND highql";
                    Weapon weaponStats = DB.QueryWeaponStats(query);
                    weapons.Add(weaponStats);

                    // Get the item name for nice looking stat readout
                    string itemQuery = "";
                    itemQuery = $"SELECT * FROM Items WHERE lowid == {weapon.Item1} AND {weapon.Item3} BETWEEN lowql AND highql";
                    AOItem item = DB.QueryItemByIDs(itemQuery);
                    hrefWeapons.Add(BuildItemRef(item.lowid, item.highid, weapon.Item3, item.name));
                }
                else
                {
                    //weapons.Add(new Weapon());
                    //hrefWeapons.Add("");
                }
            }
            return (weapons, hrefWeapons);
        }

        private (List<Weapon>, List<string>) GetPlan()
        {
            (int, int, int)[] gearData = SettingsManager.GetPlan();
            List<Weapon> weapons = new();
            List<string> hrefWeapons = new();
            foreach ((int, int, int) weapon in gearData)
            {
                if (weapon.Item1 > 0)
                {
                    // Use min and max values for item to calculate interpolation based on QL
                    string interpolationProgress = $"(({weapon.Item3}*1.0) - lowql) / (highql - lowql)";
                    string query = "SELECT ";
                    query += $"(minlow + ROUND({interpolationProgress} * (minhigh - minlow))) AS min, ";
                    query += $"(maxlow + ROUND({interpolationProgress} * (maxhigh - maxlow))) AS max, ";
                    query += $"(critlow + ROUND({interpolationProgress} * (crithigh - critlow))) AS crit, ";
                    query += $"(attacklow + ROUND({interpolationProgress} * (attackhigh - attacklow))) AS attack, ";
                    query += $"(rechargelow + ROUND({interpolationProgress} * (rechargehigh - rechargelow))) AS recharge ";
                    query += $"FROM WeaponStats WHERE lowid == {weapon.Item1} AND {weapon.Item3} BETWEEN lowql AND highql";
                    Weapon weaponStats = DB.QueryWeaponStats(query);
                    weapons.Add(weaponStats);

                    // Get the item name for nice looking stat readout
                    string itemQuery = "";
                    itemQuery = $"SELECT * FROM Items WHERE lowid == {weapon.Item1} AND {weapon.Item3} BETWEEN lowql AND highql";
                    AOItem item = DB.QueryItemByIDs(itemQuery);
                    hrefWeapons.Add(BuildItemRef(item.lowid, item.highid, weapon.Item3, item.name));
                }
                else
                {
                    //weapons.Add(new Weapon());
                    //hrefWeapons.Add("");
                }
            }
            return (weapons, hrefWeapons);
        }

        private (string, string) GetDPM(List<Weapon> _weapons, int _ar, int _init, int _crit, int _addDmg)
        {
            int dpm = 0;
            int dpmCapped = 0;
            int hitsPerWeapon = 30;
            if (_weapons.Count == 2)
                hitsPerWeapon = 20;
            if (_weapons.Count == 3)
                hitsPerWeapon = 15;

            foreach(Weapon weapon in _weapons)
            {
                double nonCritDmg = ((weapon.min * (1 + _ar / 400)) + (weapon.max * (1 + _ar / 400)) / 2) + _addDmg;
                double nonCritCapped = weapon.min * (1 + _ar / 400) + _addDmg;
                double nonCritHitRate = (double)(100 - _crit) / 100;
                double nonCritHits = hitsPerWeapon * nonCritHitRate;
                double critDmg = ((weapon.max + weapon.crit) * (1 + _ar / 400)) + _addDmg;
                double critCapped = (weapon.min + weapon.crit) * (1 + _ar / 400) + _addDmg;
                double critHitRate = (double)_crit / 100;
                double critHits = hitsPerWeapon * critHitRate;
                dpm += (int)((nonCritDmg * nonCritHits) + (critDmg * critHits));
                dpmCapped += (int)((nonCritCapped * nonCritHits) + (critCapped * critHits));
            }
            return (dpm.ToString(), dpmCapped.ToString());
        }

        static void LoadWeaponStats()
        {
            DB.InsertSQLFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Helpbot\\SQL\\WeaponStats.sql");
        }
    }
}


