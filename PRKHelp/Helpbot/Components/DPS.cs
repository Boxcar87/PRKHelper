using System.Diagnostics;
using System.Drawing;
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
        internal int arCap { get; set; }
        internal int burst { get; set; }
        internal int fullAuto { get; set; }
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

            string dpm = "";
            string dpmCapped = "";
            string hrefString = "";
            int lowestAgg = 100;
            (List<Weapon> gearWeapons, List<string> gearhRef) = GetGear();
            (List<Weapon> planWeapons, List<string> planhRef) = GetPlan();


            switch (_params[0])
            {
                case "gear":
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(gearWeapons, gearhRef, ar, init, critRate, addDmg);
                    OutputStrings[0] = $"{TextColor}Gear DPM - {ValueColor}{dpm}{EndColor} | (AC Capped) - {ValueColor}{dpmCapped}{EndColor} @ {HighlightColor}{lowestAgg}{EndColor}% Agg - {hrefString}";
                    break;
                case "plan":
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(planWeapons, planhRef, ar, init, critRate, addDmg);
                    OutputStrings[0] = $"{TextColor}Plan DPM - {ValueColor}{dpm}{EndColor} | (AC Capped) - {ValueColor}{dpmCapped}{EndColor} @ {HighlightColor}{lowestAgg}{EndColor}% Agg - {hrefString}";
                    break;
                case "compare":
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(gearWeapons, gearhRef, ar, init, critRate, addDmg);
                    OutputStrings[0] = $"{TextColor}Gear DPM - {ValueColor}{dpm}{EndColor} | (AC Capped) - {ValueColor}{dpmCapped}{EndColor} @ {HighlightColor}{lowestAgg}{EndColor}% Agg - {hrefString}";
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(planWeapons, planhRef, ar, init, critRate, addDmg);
                    OutputStrings.Add($"{TextColor}Plan DPM - {ValueColor}{dpm}{EndColor} | (AC Capped) - {ValueColor}{dpmCapped}{EndColor} @ {HighlightColor}{lowestAgg}{EndColor}% Agg - {hrefString}");
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
            int index = 0;
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
                    query += $"(rechargelow + ROUND({interpolationProgress} * (rechargehigh - rechargelow))) AS recharge, ";
                    query += $"(arcaplow + ROUND({interpolationProgress} * (arcaphigh - arcaplow))) AS arcap, ";
                    query += $"(burstlow + ROUND({interpolationProgress} * (bursthigh - burstlow))) AS burst, ";
                    query += $"(fullautolow + ROUND({interpolationProgress} * (fullautohigh - fullautolow))) AS fullauto ";
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
                index++;
            }
            return (weapons, hrefWeapons);
        }

        private (List<Weapon>, List<string>) GetPlan()
        {
            (int, int, int)[] gearData = SettingsManager.GetPlan();
            List<Weapon> weapons = new();
            List<string> hrefWeapons = new();
            int index = 0;
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
                    query += $"(rechargelow + ROUND({interpolationProgress} * (rechargehigh - rechargelow))) AS recharge, ";
                    query += $"(arcaplow + ROUND({interpolationProgress} * (arcaphigh - arcaplow))) AS arcap, ";
                    query += $"(burstlow + ROUND({interpolationProgress} * (bursthigh - burstlow))) AS burst, ";
                    query += $"(fullautolow + ROUND({interpolationProgress} * (fullautohigh - fullautolow))) AS fullauto ";
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

        private (string, string, string, int) GetDPM(List<Weapon> _weapons, List<string> _hrefWeapons, int _ar, int _init, int _crit, int _addDmg)
        {
            int dpm = 0;
            int dpmCapped = 0;
            int hitsPerWeapon = 30;
            if (_weapons.Count == 2)
                hitsPerWeapon = 20;
            if (_weapons.Count == 3)
                hitsPerWeapon = 15;

            string hrefString = $"<a href=\"text://{HighlightColor}DPM Breakdown{EndColor}<br><br>";
            int lowestAllAgg = 0;
            int fullDefDpm = 0;
            int fullDefDpmCapped = 0;
            int threeQuarterDefDpm = 0;
            int threeQuarterDefDpmCapped = 0;
            int halfDefDpm = 0;
            int halfDefDpmCapped = 0;
            int neutralDefDpm = 0;
            int neutralDefDpmCapped = 0;
            for (int i = 0; i < _weapons.Count; i++)
            {
                Weapon weapon = _weapons[i];

                if (_ar > weapon.arCap)
                    _ar = weapon.arCap;
                double arBonus = _ar > 1000 ? 1000 + (int)((_ar - 1000) * 0.30) : _ar;
                arBonus = 1 + arBonus / 400;
                int minDamage = (int)(weapon.min * arBonus);
                int maxDamage = (int)(weapon.max * arBonus);
                int critDamage = (int)(weapon.crit * arBonus);

                int nonCritDmg = ((minDamage + maxDamage) / 2) + _addDmg;
                int nonCritCapped = minDamage + _addDmg;
                double nonCritHitRate = (double)(100 - _crit) / 100;

                int critDmg = maxDamage + critDamage + _addDmg;
                int critCapped = minDamage + critDamage + _addDmg;
                double critHitRate = (double)_crit / 100;

                // Get various attack and recharge rates
                (int attackRate, int rechargeRate) = GetWeaponSpeed(_init, weapon.attack, weapon.recharge);
                int highestRate = attackRate > rechargeRate ? attackRate : rechargeRate;
                int fullDefAttack = (attackRate + 175) < 100 ? 100 : attackRate + 175;
                int fullDefRecharge = (rechargeRate + 175) < 100 ? 100 : rechargeRate + 175;
                int threeQuarterDefAttack = (attackRate + 125) < 100 ? 100 : attackRate + 125;
                int threeQuarterDefRecharge = (rechargeRate + 125) < 100 ? 100 : rechargeRate + 125;
                int halfDefAttack = (attackRate + 75) < 100 ? 100 : attackRate + 75;
                int halfDefRechage = (rechargeRate + 75) < 100 ? 100 : rechargeRate + 75;
                int neutralAttack = attackRate < 100 ? 100 : attackRate;
                int neutralRechage = rechargeRate < 100 ? 100 : rechargeRate;
                int fullAggAttack = (attackRate - 25) < 100 ? 100 : attackRate - 25;
                int fullAggRechage = (rechargeRate - 25) < 100 ? 100 : rechargeRate - 25;

                // Check if full agg is faster than neutral
                attackRate = attackRate < 100 ? 100 : attackRate;
                rechargeRate = rechargeRate < 100 ? 100 : rechargeRate;
                int fastestAttackRate = fullAggAttack < attackRate ? fullAggAttack : attackRate;
                int fastestRechargeRate = fullAggRechage < rechargeRate ? fullAggAttack : rechargeRate;

                // Assign base hits/min that all agg/def rates can factor against
                double nonCritHitsBase = hitsPerWeapon * nonCritHitRate;
                double critHitsBase = hitsPerWeapon * critHitRate;

                // Fastest hit rate possible
                double nonCritHits = nonCritHitsBase / ((fastestAttackRate + fastestRechargeRate) / 200.00);
                double critHits = critHitsBase / ((fastestAttackRate + fastestRechargeRate) / 200.00);
                dpm += (int)((nonCritDmg * nonCritHits) + (critDmg * critHits));
                dpmCapped += (int)((nonCritCapped * nonCritHits) + (critCapped * critHits));
                hrefString += $"{Indent}{_hrefWeapons[i]} {ValueColor}{minDamage + _addDmg}{EndColor} - {ValueColor}{maxDamage + _addDmg}{EndColor} ({ValueColor}{critDamage}{EndColor})<br>";

                // Full def dpm
                nonCritHits = nonCritHitsBase / ((fullDefAttack + fullDefRecharge) / 200.00);
                critHits = critHitsBase / ((fullDefAttack + fullDefRecharge) / 200.00);
                fullDefDpm += (int)((nonCritDmg * nonCritHits) + (critDmg * critHits));
                fullDefDpmCapped += (int)((nonCritCapped * nonCritHits) + (critCapped * critHits));

                // 75% def dpm
                nonCritHits = nonCritHitsBase / ((threeQuarterDefAttack + threeQuarterDefRecharge) / 200.00);
                critHits = critHitsBase / ((threeQuarterDefAttack + threeQuarterDefRecharge) / 200.00);
                threeQuarterDefDpm += (int)((nonCritDmg * nonCritHits) + (critDmg * critHits));
                threeQuarterDefDpmCapped += (int)((nonCritCapped * nonCritHits) + (critCapped * critHits));

                // 50% def dpm
                nonCritHits = nonCritHitsBase / ((halfDefAttack + halfDefRechage) / 200.00);
                critHits = critHitsBase / ((halfDefAttack + halfDefRechage) / 200.00);
                halfDefDpm += (int)((nonCritDmg * nonCritHits) + (critDmg * critHits));
                halfDefDpmCapped += (int)((nonCritCapped * nonCritHits) + (critCapped * critHits));

                // Neutral def dpm
                nonCritHits = nonCritHitsBase / ((neutralAttack + neutralRechage) / 200.00);
                critHits = critHitsBase / ((neutralAttack + neutralRechage) / 200.00);
                neutralDefDpm += (int)((nonCritDmg * nonCritHits) + (critDmg * critHits));
                neutralDefDpmCapped += (int)((nonCritCapped * nonCritHits) + (critCapped * critHits));

                // Get lowest we can set our agg/def for all equipped weapons to maintain 1/1
                int lowestAgg = (int)Math.Round((((highestRate - 125) + 175) / 175.00) * 100);
                if (lowestAgg < 0)
                    lowestAgg = 0;
                if (lowestAgg > 100)
                    lowestAgg = 100;
                if (lowestAgg > lowestAllAgg)
                    lowestAllAgg = lowestAgg;

                // Draw agg/def hit rates for weapon (hide surpassed rates)
                hrefString += $"{Indent}{HighlightColor}Lowest{EndColor} 1/1 AggDef - {ValueColor}{lowestAgg}{EndColor}%{EndColor}<br>{Indent}{Indent}0% ";
                hrefString += $" {ValueColor}{fullDefAttack / 100.00}{EndColor}/{ValueColor}{fullDefRecharge / 100.00}{EndColor} ";
                if (lowestAgg > 25)
                    hrefString += $"| 25% {ValueColor}{threeQuarterDefAttack / 100.00}{EndColor}/{ValueColor}{threeQuarterDefRecharge / 100.00}{EndColor} ";                
                if (lowestAgg > 50)
                    hrefString += $"| 50% {ValueColor}{halfDefAttack / 100.00}{EndColor}/{ValueColor}{halfDefRechage / 100.00}{EndColor} ";                
                if (lowestAgg > 88)
                    hrefString += $"<br>{Indent}{Indent}{Indent}87.5% {ValueColor}{neutralAttack / 100.00}{EndColor}/{ValueColor}{neutralRechage / 100.00}{EndColor} ";                
                if (lowestAgg == 100)
                    hrefString += $"| 100% {ValueColor}{fullAggAttack / 100.00}{EndColor}/{ValueColor}{fullAggRechage / 100.00}{EndColor} ";
                
                hrefString += $"<br><br>";
            }
            // Draw dpm rates for each agg/def position (hide surpassed rates)
            hrefString += $"    0% DPM - {ValueColor}{fullDefDpm}{EndColor} | (Capped) {ValueColor}{fullDefDpmCapped}{EndColor}<br>";
            if (lowestAllAgg > 25)
                hrefString += $"  75% DPM - {ValueColor}{threeQuarterDefDpm}{EndColor} | (Capped) {ValueColor}{threeQuarterDefDpmCapped}{EndColor}<br>";
            if (lowestAllAgg > 50)
                hrefString += $"  50% DPM - {ValueColor}{halfDefDpm}{EndColor} | (Capped) {ValueColor}{halfDefDpmCapped}{EndColor}<br>";
            if (lowestAllAgg > 88)
                hrefString += $"87.5% DPM - {ValueColor}{neutralDefDpm}{EndColor} | (Capped) {ValueColor}{neutralDefDpmCapped}{EndColor}<br>";
            if (lowestAllAgg == 100)
                hrefString += $" 100% DPM - {ValueColor}{dpm}{EndColor} | (Capped) {ValueColor}{dpmCapped}{EndColor}<br>";
            hrefString += $"<br>{RedColor}Lowest{EndColor} 1/1 Agg = {ValueColor}{lowestAllAgg}{EndColor}% ({ValueColor}{(int)(-100 + (200) * (lowestAllAgg/100.00))}{EndColor})\">Breakdown</a>";
            return (dpm.ToString(), dpmCapped.ToString(), hrefString, lowestAllAgg);
        }

        private (int, int) GetWeaponSpeed(int _init, int _attack, int _recharge)
        {
            _init = _init > 1200 ? _init + ((_init - 1200) / 3) : _init; 
            int attackRate = (int)(_attack - (_init / 600.00) * 100);
            int rechargeRate = (int)(_recharge - (_init / 300.00) * 100);

            return (attackRate, rechargeRate);
        }

        // Brawl recharge is 15s flat

        private int GetFastRecharge(int _fastSkill, int _weaponAttack)
        {
            return 0;
        }

        private int GetFlingRecharge(int _flingSkill, int _weaponAttack)
        {
            //Recharge Time: (Attack x 15) -Fling skill / 100
            //Minimum Recharge Time: 6 Seconds + Attack

            return 0;
        }

        private int GetBurstRecharge(int _burstSkill, int _weaponBurstCycle) 
        {
            //Recharge Time: (Recharge x 20) +Burst Cycle / 100 - Burst Skill / 25
            //Minimum Recharge Time: 8 Seconds + Attack

            return 0;
        }

        private int GetFullAutoRecharge(int _faSkill, int _weaponFullAutoCycle)
        {
            //Recharge Time: (Recharge x 40) +Full Auto Delay/ 100 - FA skill / 25
            //Minimum Recharge Time: 10 seconds + Attack
            //Full auto can hit for 5 bullets + 1 bullet for every 100th FA skill you have
            //If a full auto does over 10.000 damage, all the damage after that will be halved, and halved once again after 11.500, 13.000 and 14.500 making a capping 15k FA

            return 0;
        }

        static void LoadWeaponStats()
        {
            DB.InsertSQLFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Helpbot\\SQL\\WeaponStats.sql");
        }
    }
}


