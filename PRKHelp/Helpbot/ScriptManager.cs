using System.Threading.Channels;

namespace PRKHelp
{
    internal class ScriptManager
    {
        static string ScriptOutputFile;
        public static string ScriptFolder;
        static int ExecutionDelay = 1500;

        public static void Init(string _scriptsFolderPath)
        {
            ScriptFolder = _scriptsFolderPath;
            // This exists to automatically clean up old versions of script generation pathing
            string PRKPath = Path.Combine(_scriptsFolderPath, "PRKHelp");
            if (File.Exists(PRKPath)) // This checks if a file exists at the path
            {
                FileAttributes attributes = File.GetAttributes(PRKPath);
                if (!attributes.HasFlag(FileAttributes.Directory))
                    File.Delete(Path.Combine(_scriptsFolderPath, "PRKHelp"));
            }

            Directory.CreateDirectory(Path.Combine(_scriptsFolderPath, "PRKHelp"));
            ScriptOutputFile = Path.Combine(_scriptsFolderPath, "PRKHelp/Output");
            // Generate script file if it doesnt exist
            using (FileStream scriptStream = new(ScriptOutputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) { }
            // Generate shop script file if it doesnt exist
            using (FileStream scriptStream = new(Path.Combine(_scriptsFolderPath, "PRKHelp/Shop"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) { }
            // Generate chat item link file
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "PRKHelp/Itemlink"), $"<a href=\"itemref://%1/%2/%3\">%4</a>");

            //using (FileStream itemLinkStream = new(Path.Combine(_scriptsFolderPath, "PRKHelp/Itemlink"), FileMode.Create, FileAccess.Write, FileShare.Write))
            //{
            //    using(StreamWriter linkWriter = new(itemLinkStream))
            //    {
            //        linkWriter.Write($"<a href=\"itemref://%1/%2/%3\">%4</a>");
            //    }
            //}
            // Generate chat pb link file
            //using (FileStream itemLinkStream = new(Path.Combine(_scriptsFolderPath, "PRKHelp/Itemlink"), FileMode.Create, FileAccess.Write, FileShare.Write))
            //{
            //    using (StreamWriter linkWriter = new(itemLinkStream))
            //    {
            //        linkWriter.Write($"<a href=\"itemref://%1/%2/%3\">%4</a>");
            //    }
            //}
            string statsText = $"/text <a href=\"text://<header2>::: Offense / Defense :::</header2><br><a href=skillid://276>Offense (Addall-Off)</a><br><a href=skillid://277>Defense (Addall-Def)</a><br><a href=skillid://51>Aggdef-Slider</a><br><a href=skillid://4>Attack Speed</a><br><br>" +
                        $"<header2>::: Critical Strike :::</header2><br><a href=skillid://379>Crit increase</a><br><a href=skillid://391>Crit decrease</a><br><br>" +
                        $"<header2>::: Heal :::</header2><br><a href=skillid://342>Heal delta (interval)</a> (tick in secs)<br><a href=skillid://343>Heal delta (amount)</a><br><a href=skillid://535>Heal modifier</a><br><a href=skillid://689>Heal reactivity</a><br><br>" +
                        $"<header2>::: Nano :::</header2><br><a href=skillid://363>Nano delta (interval)</a> (tick in secs)<br><a href=skillid://364>Nano delta (amount)</a><br><a href=skillid://318>Nano execution cost</a><br><a href=skillid://536>Nano modifier</a><br><a href=skillid://383>Interrupt modifier</a><br><a href=skillid://381>Range Increase Nanoformula</a><br><br>" +
                        $"<header2>::: Add Damage (Amount) :::</header2><br><a href=skillid://279>+Damage - Melee</a><br><a href=skillid://280>+Damage - Energy</a><br><a href=skillid://281>+Damage - Chemical</a><br><a href=skillid://282>+Damage - Radiation</a><br><a href=skillid://278>+Damage - Projectile</a><br><a href=skillid://311>+Damage - Cold</a><br><a href=skillid://315>+Damage - Nano</a><br><a href=skillid://316>+Damage - Fire</a><br><a href=skillid://317>+Damage - Poison</a><br><br>" +
                        $"<header2>::: Reflect Shield (Percentage) :::</header2><br><a href=skillid://205>ReflectProjectileAC</a><br><a href=skillid://206>ReflectMeleeAC</a><br><a href=skillid://207>ReflectEnergyAC</a><br><a href=skillid://208>ReflectChemicalAC</a><br><a href=skillid://216>ReflectRadiationAC</a><br><a href=skillid://217>ReflectColdAC</a><br><a href=skillid://218>ReflectNanoAC</a><br><a href=skillid://219>ReflectFireAC</a><br><a href=skillid://225>ReflectPoisonAC</a><br><br>" +
                        $"<header2>::: Reflect Shield (Amount) :::</header2><br><a href=skillid://475>MaxReflectedProjectileDmg</a><br><a href=skillid://476>MaxReflectedMeleeDmg</a><br><a href=skillid://477>MaxReflectedEnergyDmg</a><br><a href=skillid://478>MaxReflectedChemicalDmg</a><br><a href=skillid://479>MaxReflectedRadiationDmg</a><br><a href=skillid://480>MaxReflectedColdDmg</a><br><a href=skillid://481>MaxReflectedNanoDmg</a><br><a href=skillid://482>MaxReflectedFireDmg</a><br><a href=skillid://483>MaxReflectedPoisonDmg</a><br><br>" +
                        $"<header2>::: Damage Shield (Amount) :::</header2><br><a href=skillid://226>ShieldProjectileAC</a><br><a href=skillid://227>ShieldMeleeAC</a><br><a href=skillid://228>ShieldEnergyAC</a><br><a href=skillid://229>ShieldChemicalAC</a><br><a href=skillid://230>ShieldRadiationAC</a><br><a href=skillid://231>ShieldColdAC</a><br><a href=skillid://232>ShieldNanoAC</a><br><a href=skillid://233>ShieldFireAC</a><br><a href=skillid://234>ShieldPoisonAC</a><br><br>" +
                        $"<header2>::: Damage Absorb (Amount) :::</header2><br><a href=skillid://238>AbsorbProjectileAC</a><br><a href=skillid://239>AbsorbMeleeAC</a><br><a href=skillid://240>AbsorbEnergyAC</a><br><a href=skillid://241>AbsorbChemicalAC</a><br><a href=skillid://242>AbsorbRadiationAC</a><br><a href=skillid://243>AbsorbColdAC</a><br><a href=skillid://244>AbsorbFireAC</a><br><a href=skillid://245>AbsorbPoisonAC</a><br><a href=skillid://246>AbsorbNanoAC</a><br><br>" +
                        $"<header2>::: Misc :::</header2><br><a href=skillid://592>Unsaved XP</a><br><a href=skillid://382>SkillLockModifier</a><br><a href=skillid://380>Weapon Range Increase</a><br><a href=skillid://517>Special Attack Blockers</a><br><a href=skillid://199>Reset Points</a><br><a href=skillid://360>Scale</a><br><a href=skillid://676>Profession Duel Kills</a><br><a href=skillid://677>Profession Duel Deaths</a><br><a href=skillid://679>Solo Deaths</a><br><a href=skillid://681>Team Deaths</a><br><a href=skillid://410>Number of fighting opponents</a>\">Hidden Stats</a>";
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "PRKHelp/Stats"), statsText);
           
            //using (FileStream statsStream = new(Path.Combine(_scriptsFolderPath, "PRKHelp/Stats"), FileMode.Create, FileAccess.Write, FileShare.Write))
            //{
            //    using (StreamWriter statsWriter = new(statsStream))
            //    {
            //        statsWriter.Write($"/text <a href=\"text://<header2>::: Offense / Defense :::</header2><br><a href=skillid://276>Offense (Addall-Off)</a><br><a href=skillid://277>Defense (Addall-Def)</a><br><a href=skillid://51>Aggdef-Slider</a><br><a href=skillid://4>Attack Speed</a><br><br>" +
            //            $"<header2>::: Critical Strike :::</header2><br><a href=skillid://379>Crit increase</a><br><a href=skillid://391>Crit decrease</a><br><br>" +
            //            $"<header2>::: Heal :::</header2><br><a href=skillid://342>Heal delta (interval)</a> (tick in secs)<br><a href=skillid://343>Heal delta (amount)</a><br><a href=skillid://535>Heal modifier</a><br><a href=skillid://689>Heal reactivity</a><br><br>" +
            //            $"<header2>::: Nano :::</header2><br><a href=skillid://363>Nano delta (interval)</a> (tick in secs)<br><a href=skillid://364>Nano delta (amount)</a><br><a href=skillid://318>Nano execution cost</a><br><a href=skillid://536>Nano modifier</a><br><a href=skillid://383>Interrupt modifier</a><br><a href=skillid://381>Range Increase Nanoformula</a><br><br>" +
            //            $"<header2>::: Add Damage (Amount) :::</header2><br><a href=skillid://279>+Damage - Melee</a><br><a href=skillid://280>+Damage - Energy</a><br><a href=skillid://281>+Damage - Chemical</a><br><a href=skillid://282>+Damage - Radiation</a><br><a href=skillid://278>+Damage - Projectile</a><br><a href=skillid://311>+Damage - Cold</a><br><a href=skillid://315>+Damage - Nano</a><br><a href=skillid://316>+Damage - Fire</a><br><a href=skillid://317>+Damage - Poison</a><br><br>" +
            //            $"<header2>::: Reflect Shield (Percentage) :::</header2><br><a href=skillid://205>ReflectProjectileAC</a><br><a href=skillid://206>ReflectMeleeAC</a><br><a href=skillid://207>ReflectEnergyAC</a><br><a href=skillid://208>ReflectChemicalAC</a><br><a href=skillid://216>ReflectRadiationAC</a><br><a href=skillid://217>ReflectColdAC</a><br><a href=skillid://218>ReflectNanoAC</a><br><a href=skillid://219>ReflectFireAC</a><br><a href=skillid://225>ReflectPoisonAC</a><br><br>" +
            //            $"<header2>::: Reflect Shield (Amount) :::</header2><br><a href=skillid://475>MaxReflectedProjectileDmg</a><br><a href=skillid://476>MaxReflectedMeleeDmg</a><br><a href=skillid://477>MaxReflectedEnergyDmg</a><br><a href=skillid://478>MaxReflectedChemicalDmg</a><br><a href=skillid://479>MaxReflectedRadiationDmg</a><br><a href=skillid://480>MaxReflectedColdDmg</a><br><a href=skillid://481>MaxReflectedNanoDmg</a><br><a href=skillid://482>MaxReflectedFireDmg</a><br><a href=skillid://483>MaxReflectedPoisonDmg</a><br><br>" +
            //            $"<header2>::: Damage Shield (Amount) :::</header2><br><a href=skillid://226>ShieldProjectileAC</a><br><a href=skillid://227>ShieldMeleeAC</a><br><a href=skillid://228>ShieldEnergyAC</a><br><a href=skillid://229>ShieldChemicalAC</a><br><a href=skillid://230>ShieldRadiationAC</a><br><a href=skillid://231>ShieldColdAC</a><br><a href=skillid://232>ShieldNanoAC</a><br><a href=skillid://233>ShieldFireAC</a><br><a href=skillid://234>ShieldPoisonAC</a><br><br>" +
            //            $"<header2>::: Damage Absorb (Amount) :::</header2><br><a href=skillid://238>AbsorbProjectileAC</a><br><a href=skillid://239>AbsorbMeleeAC</a><br><a href=skillid://240>AbsorbEnergyAC</a><br><a href=skillid://241>AbsorbChemicalAC</a><br><a href=skillid://242>AbsorbRadiationAC</a><br><a href=skillid://243>AbsorbColdAC</a><br><a href=skillid://244>AbsorbFireAC</a><br><a href=skillid://245>AbsorbPoisonAC</a><br><a href=skillid://246>AbsorbNanoAC</a><br><br>" +
            //            $"<header2>::: Misc :::</header2><br><a href=skillid://592>Unsaved XP</a><br><a href=skillid://382>SkillLockModifier</a><br><a href=skillid://380>Weapon Range Increase</a><br><a href=skillid://517>Special Attack Blockers</a><br><a href=skillid://199>Reset Points</a><br><a href=skillid://360>Scale</a><br><a href=skillid://676>Profession Duel Kills</a><br><a href=skillid://677>Profession Duel Deaths</a><br><a href=skillid://679>Solo Deaths</a><br><a href=skillid://681>Team Deaths</a><br><a href=skillid://410>Number of fighting opponents</a>\">Hidden Stats</a>");
            //    }
            //}

            GenerateInterfaceScripts(_scriptsFolderPath);
        }

        // Supports paginated output.
        // Each element in _output should be a new page.
        public static void WriteOutput(List<string> _output, string _channel="/text ")
        {
            for (var i = 0; i < _output.Count; i++)
            {
                string _outputIndexString = ScriptOutputFile.ToString();
                if (i > 0)
                    _outputIndexString += i.ToString();

                string text = _channel + _output[i];
                if (_output.Count > 1 && _output.Count > i + 1)
                    text += ($"\n/PRKHelp/Output{i + 1}");
                File.WriteAllText(_outputIndexString, $"{_channel}{_output[i]}");

                //using (FileStream scriptStream = new(_outputIndexString, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                //{
                //    using (StreamWriter scriptWriter = new(scriptStream))
                //    {
                //        scriptWriter.Write($"{_channel}{_output[i]}");
                        
                //        // Create new page references as needed
                //        if(_output.Count > 1 && _output.Count > i + 1)
                //        {
                //            scriptWriter.Write($"\n/PRKHelp/Output{i + 1}");
                //        }
                //    }
                //}                
            }
        }

        public static void UpdateShop(string _shopText)
        {
            File.WriteAllText(Path.Combine(ScriptFolder, "PRKHelp/Shop"), _shopText);

            //using (FileStream scriptStream = new(Path.Combine(ScriptFolder, "PRKHelp/Shop"), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            //{
            //    using (StreamWriter scriptWriter = new(scriptStream))
            //    {
            //        scriptWriter.Write($"{_shopText}");

            //        // Create new page references as needed
            //        //if (_output.Count > 1 && _output.Count > i + 1)
            //        //{
            //        //    scriptWriter.Write($"\n/PRKHelp/Output{i + 1}");
            //        //}
            //    }
            //}
        }

        // Add new script here so player can call the function
        // Append appropriate amount of parameter inputs via add %1 %2 etc
        private static void GenerateInterfaceScripts(string _scriptsFolderPath)
        {

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "ma"), $"/w !mafist %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is number
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "mafist"), $"/w !mafist %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is number

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "calc"), $"/w !calc %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is number
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "oe"), $"/w !oe %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is number
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "level"), $"/w !level %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is number
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "mission"), $"/w !mission %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is number
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "timer"), $"/w !timer %1 %2\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // First input is string second is number
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "timers"), $"/w !timers\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // No inputs

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "dps"), $"/w !dps %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is a string
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "dpm"), $"/w !dps %1\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is a string

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "whois"), $"/w !whois %1 %2 %3 %4 %5\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is a string
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "alts"), $"/w !whois %1 %2 %3 %4 %5\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Input is a string

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "itemfind"), $"/w !itemfind %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs, each input is a word of the item name, first input can be ql
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "trickle"), $"/w !trickle %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 8 inputs, 9th input is used for error handling

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "symb"), $"/w !symbiant %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs, each input is a word of the item name
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "symbiant"), $"/w !symbiant %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs, each input is a word of the item name

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "pb"), $"/w !pocketboss %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs, input could be an inserted pattern
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "pocketboss"), $"/w !pocketboss %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs, input could be an inserted pattern

            File.WriteAllText(Path.Combine(_scriptsFolderPath, "editshop"), $"/w !editshop %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs 2rd input could be start of item 
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "character"), $"/w !character %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs 3rd input could be start of item
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "vendor"), $"/w !vendor %1 %2 %3 %4 %5 %6 %7 %8 %9\n/delay {ExecutionDelay}\n/PRKHelp/Output"); // Allows 9 inputs 1st input should be start of item
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "postshop"), $"/PRKHelp/Shop"); // directly executes shop macro
            File.WriteAllText(Path.Combine(_scriptsFolderPath, "stats"), $"/PRKHelp/Stats"); // directly executes shop macro
        }
    }
}
