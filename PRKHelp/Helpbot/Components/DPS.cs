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
        internal int flingShot { get; set; }
        internal int burst { get; set; }
        internal int fullAuto { get; set; }
        internal int fastAttack { get; set; }
        internal int brawl { get; set; }
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
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(gearWeapons, gearhRef);
                    OutputStrings[0] = $"{TextColor}Gear DPM - {ValueColor}{dpm}{EndColor} | (AC Capped) - {ValueColor}{dpmCapped}{EndColor} @ {HighlightColor}{lowestAgg}{EndColor}% Agg - {hrefString}";
                    break;
                case "plan":
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(planWeapons, planhRef);
                    OutputStrings[0] = $"{TextColor}Plan DPM - {ValueColor}{dpm}{EndColor} | (AC Capped) - {ValueColor}{dpmCapped}{EndColor} @ {HighlightColor}{lowestAgg}{EndColor}% Agg - {hrefString}";
                    break;
                case "compare":
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(gearWeapons, gearhRef);
                    OutputStrings[0] = $"{TextColor}Gear DPM - {ValueColor}{dpm}{EndColor} | (AC Capped) - {ValueColor}{dpmCapped}{EndColor} @ {HighlightColor}{lowestAgg}{EndColor}% Agg - {hrefString}";
                    (dpm, dpmCapped, hrefString, lowestAgg) = GetDPM(planWeapons, planhRef);
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
                    Weapon weaponStats = GetWeaponStats(weapon.Item1, weapon.Item3);
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
                    Weapon weaponStats = GetWeaponStats(weapon.Item1, weapon.Item3);
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

        private (string, string, string, int) GetDPM(List<Weapon> _weapons, List<string> _hrefWeapons)
        {
            int ar = SettingsManager.GetStat("AR");
            int init = SettingsManager.GetStat("Init");
            int crit = SettingsManager.GetStat("Crit");
            int addDmg = SettingsManager.GetStat("Dmg");
            int flingShot = SettingsManager.GetStat("Flingshot");
            int burst = SettingsManager.GetStat("Burst");
            int fullAuto = SettingsManager.GetStat("Fullauto");
            int fastAttack = SettingsManager.GetStat("Fastattack");
            int brawl = SettingsManager.GetStat("Brawl");

            int dpm = 0;
            int dpmCapped = 0;
            int specialsDpm = 0;
            int specialsDpmCapped = 0;

            bool flingShotNotAdded = true;
            bool burstNotAdded = true;
            bool fullAutoNotAdded = true;
            bool fastAttackNotAdded = true;
            bool brawlNotAdded = true;

            int hitsPerWeapon = 30;
            if (_weapons.Count == 2)
                hitsPerWeapon = 20;
            if (_weapons.Count == 3)
                hitsPerWeapon = 15;

            int lowestAllAgg = 0;
            int fullDefDpm = 0;
            int fullDefDpmCapped = 0;
            int threeQuarterDefDpm = 0;
            int threeQuarterDefDpmCapped = 0;
            int halfDefDpm = 0;
            int halfDefDpmCapped = 0;
            int neutralDefDpm = 0;
            int neutralDefDpmCapped = 0;

            string hrefString = $"<a href=\"text://{HighlightColor}DPM Breakdown{EndColor}<br><br>";
            for (int i = 0; i < _weapons.Count; i++)
            {
                Weapon weapon = _weapons[i];

                if (ar > weapon.arCap)
                    ar = weapon.arCap;
                double arBonus = ar > 1000 ? 1000 + (int)((ar - 1000) * 0.30) : ar;
                arBonus = 1 + arBonus / 400;
                int minDamage = (int)(weapon.min * arBonus);
                int maxDamage = (int)(weapon.max * arBonus);
                int critDamage = (int)(weapon.crit * arBonus);
                hrefString += $"{Indent}{_hrefWeapons[i]} {ValueColor}{minDamage + addDmg}{EndColor} - {ValueColor}{maxDamage + addDmg}{EndColor} ({ValueColor}{critDamage}{EndColor})<br>";

                int nonCritDmg = ((minDamage + maxDamage) / 2) + addDmg;
                int nonCritCapped = minDamage + addDmg;
                double nonCritHitRate = (double)(100 - crit) / 100;

                int critDmg = maxDamage + critDamage + addDmg;
                int critCapped = minDamage + critDamage + addDmg;
                double critHitRate = (double)crit / 100;

                // Get various attack and recharge rates
                (int attackRate, int rechargeRate, int lowestAgg) = GetWeaponSpeed(init, weapon.attack, weapon.recharge);
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


                // Get damage of specials 
                if(weapon.flingShot == 1 && flingShot > 0 && flingShotNotAdded)
                {
                    int flingRecharge = GetFlingRecharge(flingShot, weapon.attack);
                    double nonCritFlingHits = (60.00 / flingRecharge) * nonCritHitRate;
                    double critFlingHits = (60.00 / flingRecharge) * critHitRate;
                    int flingDamage = (int)((nonCritDmg * nonCritFlingHits) + (critDamage * critFlingHits));
                    int flingDamageCapped = (int)((nonCritCapped * nonCritFlingHits) + (critCapped * critFlingHits));

                    dpm += flingDamage;
                    dpmCapped += flingDamageCapped;
                    specialsDpm += flingDamage;
                    specialsDpmCapped += flingDamageCapped;
                    hrefString += $"{Indent}{Indent}{Indent}Fling shot recharge {ValueColor}{flingRecharge}{EndColor}s<br>";
                    flingShotNotAdded = false;
                }
                if (weapon.burst > 0 && burst > 0 && burstNotAdded)
                {
                    int burstRecharge = GetBurstRecharge(burst, weapon.burst, weapon.attack, weapon.recharge);
                    double burstHits = (60.00 / burstRecharge) * 3;
                    int burstDamage = (int)(nonCritDmg * burstHits);
                    int burstDamageCapped = (int)(nonCritCapped * burstHits);

                    dpm += burstDamage;
                    dpmCapped += burstDamageCapped;
                    specialsDpm += burstDamage;
                    specialsDpmCapped += burstDamageCapped;
                    hrefString += $"{Indent}{Indent}{Indent}Burst recharge {ValueColor}{burstRecharge}{EndColor}s<br>";
                    burstNotAdded = false;
                }
                if (weapon.fullAuto > 0 && fullAuto > 0 && fullAutoNotAdded)
                {
                    int faRecharge = GetFullAutoRecharge(fullAuto, weapon.fullAuto, weapon.attack, weapon.recharge);
                    int rawHits = 5 + (fullAuto / 100);
                    double faNonCritHits = (60.00 / faRecharge) * rawHits * nonCritHitRate;
                    double faCritHits = (60.00 / faRecharge) * rawHits * critHitRate;
                    int faDamage = (int)((nonCritDmg * faNonCritHits) + (critDamage * faCritHits));
                    int faDamageCapped = (int)((nonCritCapped * faNonCritHits) + (critCapped * faCritHits));

                    if (faDamage > 10000)
                    {
                        faDamage = 10000 + (faDamage - 10000) / 2;
                        if (faDamage > 11500)
                            faDamage = 11500 + (faDamage - 11500) / 2;
                        if (faDamage > 13000)
                            faDamage = 13000 + (faDamage - 13000) / 2;
                        if (faDamage > 14500)
                            faDamage = 14500 + (faDamage - 14500) / 2;
                        if (faDamage > 15000)
                            faDamage = 15000;
                    }
                    if (faDamageCapped > 10000)
                    {
                        faDamageCapped = 10000 + (faDamageCapped - 10000) / 2;
                        if (faDamageCapped > 11500)
                            faDamageCapped = 11500 + (faDamageCapped - 11500) / 2;
                        if (faDamageCapped > 13000)
                            faDamageCapped = 13000 + (faDamageCapped - 13000) / 2;
                        if (faDamageCapped > 14500)
                            faDamageCapped = 14500 + (faDamageCapped - 14500) / 2;
                        if (faDamageCapped > 15000)
                            faDamageCapped = 15000;
                    }

                    dpm += faDamage;
                    dpmCapped += faDamageCapped;
                    specialsDpm += faDamage;
                    specialsDpmCapped += faDamageCapped;
                    hrefString += $"{Indent}{Indent}{Indent}Full auto recharge {ValueColor}{faRecharge}{EndColor}s<br>";
                    fullAutoNotAdded = false;
                }
                if (weapon.fastAttack == 1 && fastAttack > 0 && fastAttackNotAdded)
                {
                    int fastRecharge = GetFastRecharge(fastAttack, weapon.attack);
                    double nonCritFastHits = (60.00 / fastRecharge) * nonCritHitRate;
                    double critFastHits = (60.00 / fastRecharge) * critHitRate;
                    int fastDamage = (int)((nonCritDmg * nonCritFastHits) + (critDamage * critFastHits));
                    int fastDamageCapped = (int)((nonCritCapped * nonCritFastHits) + (critCapped * critFastHits));

                    dpm += fastDamage;
                    dpmCapped += fastDamageCapped;
                    specialsDpm += fastDamage;
                    specialsDpmCapped += fastDamageCapped;
                    hrefString += $"{Indent}{Indent}{Indent}Fast attack recharge {ValueColor}{fastRecharge}{EndColor}s<br>";
                    fastAttackNotAdded = false;
                }
                if (weapon.brawl == 1 && brawl > 0 && brawlNotAdded)
                {
                    Weapon brawlWeapon = GetBrawlDamage(brawl);
                    int minBrawlDamage = (int)(brawlWeapon.min * arBonus);
                    int maxBrawlDamage = (int)(brawlWeapon.max * arBonus);
                    int critBrawlDamage = (int)(brawlWeapon.crit * arBonus);
                    int nonCritBrawlDmg = ((minBrawlDamage + maxBrawlDamage) / 2) + addDmg;
                    int nonCritBrawlCapped = minBrawlDamage + addDmg;
                    int critBrawlCapped = minBrawlDamage + critBrawlDamage + addDmg;
                    int critBrawlDmg = maxBrawlDamage + critBrawlDamage + addDmg;
                    double nonCritBrawHits = 4 * nonCritHitRate;
                    double critBrawlHits = 4 * critHitRate;
                    int brawlDamage = (int)((nonCritBrawlDmg * nonCritBrawHits) + (critBrawlDamage * critBrawlHits));
                    int brawlDamageCapped = (int)((nonCritBrawlCapped * nonCritBrawHits) + (critBrawlCapped * critBrawlHits));
                    // Calculate 100 brawl hits to get average crit spread. Divide by 25 to mimic 4 hits (15s cd) per minute
                    dpm += brawlDamage;
                    dpmCapped += brawlDamageCapped;
                    specialsDpm += brawlDamage;
                    specialsDpmCapped += brawlDamageCapped;
                    hrefString += $"{Indent}{Indent}{Indent}Brawl damage {ValueColor}{minBrawlDamage + addDmg}{EndColor} - {ValueColor}{maxBrawlDamage + addDmg}{EndColor} ({ValueColor}{critBrawlDamage}{EndColor})<br>";
                    brawlNotAdded = false;
                }


                // Differnt DPM per agg/def settings
                // Fastest hit rate possible
                double nonCritHits = nonCritHitsBase / ((fastestAttackRate + fastestRechargeRate) / 200.00);
                double critHits = critHitsBase / ((fastestAttackRate + fastestRechargeRate) / 200.00);
                dpm += (int)((nonCritDmg * nonCritHits) + (critDmg * critHits));
                dpmCapped += (int)((nonCritCapped * nonCritHits) + (critCapped * critHits));

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
                if (lowestAgg < 0)
                    lowestAgg = 0;
                if (lowestAgg > 100)
                    lowestAgg = 100;
                if (lowestAgg > lowestAllAgg)
                    lowestAllAgg = lowestAgg;

                // Draw agg/def hit rates for weapon (hide surpassed rates)
                hrefString += $"<br>{Indent}{Indent}{HighlightColor}Lowest{EndColor} 1/1 AggDef - {ValueColor}{lowestAgg}{EndColor}%{EndColor}<br>";
                hrefString += $"{Indent}{Indent}{Indent}0% {ValueColor}{fullDefAttack / 100.00}{EndColor}/{ValueColor}{fullDefRecharge / 100.00}{EndColor} ";
                if (lowestAgg > 25)
                    hrefString += $"| 25% {ValueColor}{threeQuarterDefAttack / 100.00}{EndColor}/{ValueColor}{threeQuarterDefRecharge / 100.00}{EndColor} ";                
                if (lowestAgg > 50)
                    hrefString += $"| 50% {ValueColor}{halfDefAttack / 100.00}{EndColor}/{ValueColor}{halfDefRechage / 100.00}{EndColor} ";                
                if (lowestAgg > 88)
                    hrefString += $"<br>{Indent}{Indent}{Indent}{Indent}87.5% {ValueColor}{neutralAttack / 100.00}{EndColor}/{ValueColor}{neutralRechage / 100.00}{EndColor} ";                
                if (lowestAgg == 100)
                    hrefString += $"| 100% {ValueColor}{fullAggAttack / 100.00}{EndColor}/{ValueColor}{fullAggRechage / 100.00}{EndColor} ";
                
                hrefString += $"<br><br>";
            }

            // Draw dpm rates for each agg/def position (hide surpassed rates)
            hrefString += $"    0% DPM - {ValueColor}{fullDefDpm+specialsDpm}{EndColor} | (Capped) {ValueColor}{fullDefDpmCapped+specialsDpmCapped}{EndColor}<br>";
            if (lowestAllAgg > 25)
                hrefString += $"  25% DPM - {ValueColor}{threeQuarterDefDpm+specialsDpm}{EndColor} | (Capped) {ValueColor}{threeQuarterDefDpmCapped+specialsDpmCapped}{EndColor}<br>";
            if (lowestAllAgg > 50)
                hrefString += $"  50% DPM - {ValueColor}{halfDefDpm+specialsDpm}{EndColor} | (Capped) {ValueColor}{halfDefDpmCapped+specialsDpmCapped}{EndColor}<br>";
            if (lowestAllAgg > 88)
                hrefString += $"87.5% DPM - {ValueColor}{neutralDefDpm+specialsDpm}{EndColor} | (Capped) {ValueColor}{neutralDefDpmCapped+specialsDpmCapped}{EndColor}<br>";
            if (lowestAllAgg == 100)
                hrefString += $" 100% DPM - {ValueColor}{dpm+specialsDpm}{EndColor} | (Capped) {ValueColor}{dpmCapped+specialsDpmCapped}{EndColor}<br>";
            hrefString += $"<br>{RedColor}Lowest{EndColor} 1/1 Agg = {ValueColor}{lowestAllAgg}{EndColor}% ({ValueColor}{(int)(-100 + (200) * (lowestAllAgg/100.00))}{EndColor})\">Breakdown</a>";

            return (dpm.ToString(), dpmCapped.ToString(), hrefString, lowestAllAgg);
        }

        private (int, int, int) GetWeaponSpeed(int _init, int _attack, int _recharge)
        {
            _init = _init > 1200 ? _init + ((_init - 1200) / 3) : _init; 
            int attackRate = (int)(_attack - (_init / 600.00) * 100);
            int rechargeRate = (int)(_recharge - (_init / 300.00) * 100);
            double attackAgg = ((((_attack/100.00) - (_init / 600.00)) - 1) / 0.02) + 87.5;
            double rechargeAgg = ((((_attack/100.00) - (_init / 300.00)) - 1) / 0.02) + 87.5;
            double lowestAgg = attackAgg > rechargeAgg ? attackAgg : rechargeAgg;

            return (attackRate, rechargeRate, (int)lowestAgg);
        }

        // Brawl recharge is 15s flat
        private Weapon GetBrawlDamage(int _brawlSkill)
        {
            (int, int, int) brawlItem = MA.GetBrawlItem(_brawlSkill);
            return GetWeaponStats(brawlItem.Item1, brawlItem.Item3);
        }

        private int GetFastRecharge(int _fastSkill, int _weaponAttack)
        {
            double attackNormalized = _weaponAttack / 100.00;
            double recharge = (attackNormalized * 15) - (_fastSkill / 100);
            if (recharge < 6 + attackNormalized)
                recharge = 6 + attackNormalized;

            return (int)recharge;
        }

        private int GetFlingRecharge(int _flingSkill, int _weaponAttack)
        {
            double attackNormalized = _weaponAttack / 100.00;
            double recharge = (attackNormalized * 15) - (_flingSkill / 100);
            if (recharge < 6 + attackNormalized)
                recharge = 6 + attackNormalized;

            return (int)recharge;
        }

        private int GetBurstRecharge(int _burstSkill, int _weaponBurstCycle, int _weaponAttack, int _weaponRecharge) 
        {
            //Recharge Time: (Recharge x 20) + Burst Cycle / 100 - Burst Skill / 25
            //Minimum Recharge Time: 8 Seconds + Attack

            double rechargeNormalized = _weaponRecharge / 100.00;
            double attackNormalized = _weaponAttack / 100.00;
            double recharge = (rechargeNormalized * 20) + (_weaponBurstCycle / 100) - (_burstSkill / 25);
            if (recharge < 8 + attackNormalized)
                recharge = 8 + attackNormalized;

            return (int)recharge;
        }

        private int GetFullAutoRecharge(int _faSkill, int _weaponFullAutoCycle, int _weaponAttack, int _weaponRecharge)
        {
            double rechargeNormalized = _weaponRecharge / 100.00;
            double attackNormalized = _weaponAttack / 100.00;
            double recharge = (rechargeNormalized * 40) + (_weaponFullAutoCycle / 100) - (_faSkill / 25);
            if (recharge < 10 + attackNormalized)
                recharge = 10 + attackNormalized;

            return (int)recharge;
        }

        private Weapon GetWeaponStats(int _lowid, int _ql)
        {
            string interpolationProgress = $"(({_ql}*1.0) - lowql) / (highql - lowql)";
            string query = "SELECT ";
            query += $"(minlow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (minhigh - minlow))) AS min, ";
            query += $"(maxlow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (maxhigh - maxlow))) AS max, ";
            query += $"(critlow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (crithigh - critlow))) AS crit, ";
            query += $"(attacklow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (attackhigh - attacklow))) AS attack, ";
            query += $"(rechargelow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (rechargehigh - rechargelow))) AS recharge, ";
            query += $"(arcaplow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (arcaphigh - arcaplow))) AS arcap, ";
            query += $"(burstlow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (bursthigh - burstlow))) AS burst, ";
            query += $"(fullautolow + ROUND(CASE WHEN highql = lowql THEN 0 ELSE {interpolationProgress} END * (fullautohigh - fullautolow))) AS fullauto, ";
            query += $"flingshot, fastattack, brawl ";
            query += $"FROM WeaponStats WHERE lowid == {_lowid} AND {_ql} BETWEEN lowql AND highql";
            return DB.QueryWeaponStats(query);
        }

        static void LoadWeaponStats()
        {
            DB.InsertSQLFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Helpbot\\SQL\\WeaponStats.sql");
        }
    }
}


