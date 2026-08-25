
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;
using PRKHelper.Helpbot.Components;
using PRKHelper.Properties;

namespace PRKHelp.Settings
{
    public partial class SettingsManager
    {
        readonly static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        readonly static string Settings = Path.Combine(AppDataPath, "PRKHelp", "Settings");
        readonly static string SettingsFile = "PRKHelpSettings.json";

        public static string GetLastSelectedCharacter()
        {
            Settings settings = GetSettings();
            return (string)settings.GetType().GetProperty("LastSelectedCharacter").GetValue(settings);
        }

        public static void UpdateLastSelectedCharacter(string _character)
        {
            Settings settings = GetSettings();
            settings.LastSelectedCharacter = _character;

            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static List<string> GetAllCharacters()
        {
            Settings settings = GetSettings();
            Dictionary<string, PathSettings> CharacterPaths = settings.Characters;
            List<string> characters = [];
            foreach (KeyValuePair<string, PathSettings> pair in CharacterPaths)
            {
                characters.Add(pair.Key);
            }
            return characters;
        }

        public static void RemoveCharacter(string _character)
        {
            Settings settings = GetSettings();
            settings.Characters.Remove(_character);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static string GetFilePath(string _pathType, string _character)
        {
            string path;
            Directory.CreateDirectory(Settings);

            if (!File.Exists(Path.Combine(Settings, SettingsFile)))
            {
                string newFileString = JsonSerializer.Serialize(new Settings(), new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), newFileString);
                return "";
            }

            try
            {
                Settings settings = GetSettings();
                if (settings.Characters.ContainsKey(_character))
                    path = (string)settings.Characters[_character].GetType().GetProperty(_pathType).GetValue(settings.Characters[_character]);
                else
                    return "";
            }
            catch (JsonException _ex)
            {

                // Currently returning an object where all values are null in the event it cannot parse the file
                // In this situation the UIWindowController will reset/repopulate the default values as the windows are opened for the first time
                path = "";
            }

            return path;
        }

        public static void UpdateFilePath(string _path, string _pathType, string _character)
        {
            Settings settings = GetSettings();
            if (!settings.Characters.ContainsKey(_character))
                settings.Characters.Add(_character, new PathSettings());

            settings.Characters[_character].GetType().GetProperty(_pathType).SetValue(settings.Characters[_character], _path, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static string GetScriptsPath()
        {
            Settings settings = GetSettings();
            return (string)settings.GetType().GetProperty("ScriptsPath").GetValue(settings);
        }

        public static void UpdateScriptsPath(string _path)
        {
            Settings settings = GetSettings();
            settings.GetType().GetProperty("ScriptsPath").SetValue(settings, _path, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static (int, int) GetWindowPosition()
        {
            Settings settings = GetSettings();
            int top = (int)settings.GetType().GetProperty("WindowPositionTop").GetValue(settings);
            int left = (int)settings.GetType().GetProperty("WindowPositionLeft").GetValue(settings);
            return (top, left);
        }

        public static void UpdateWindowPosition(int _top, int _left)
        {
            Settings settings = GetSettings();
            settings.GetType().GetProperty("WindowPositionTop").SetValue(settings, _top, null);
            settings.GetType().GetProperty("WindowPositionLeft").SetValue(settings, _left, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static void UpdateShopMessage(string _message)
        {
            Settings settings = GetSettings();
            settings.GetType().GetProperty("ShopMessage").SetValue(settings, _message, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static string GetShopMessage()
        {
            Settings settings = GetSettings();
            return (string)settings.GetType().GetProperty("ShopMessage").GetValue(settings);
        }

        public static (int, int, int)[] GetGear()
        {
            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            if (settings.Characters.ContainsKey(character))
                return ((int,int,int)[])settings.Characters[character].GetType().GetProperty("Gear").GetValue(settings.Characters[character]);
            else
                return new (int, int, int)[3];
        }

        public static void UpdateGear((int, int, int)[] _gear)
        {
            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            settings.Characters[character].GetType().GetProperty("Gear").SetValue(settings.Characters[character], _gear, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static (int, int, int)[] GetPlan()
        {
            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            if (settings.Characters.ContainsKey(character))
                return ((int, int, int)[])settings.Characters[character].GetType().GetProperty("Plan").GetValue(settings.Characters[character]);
            else
                return new (int, int, int)[3];
        }

        public static void UpdatePlan((int, int, int)[] _plan)
        {
            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            settings.Characters[character].GetType().GetProperty("Plan").SetValue(settings.Characters[character], _plan, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static int GetStat(string _stat)
        {
            _stat = _stat.ToLower();
            if (_stat == "ar")
                _stat = _stat.ToUpper();
            else
                _stat = char.ToUpper(_stat[0]) + _stat.Substring(1);

            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            if (settings.Characters.ContainsKey(character))
                return (int)settings.Characters[character].GetType().GetProperty(_stat).GetValue(settings.Characters[character]);
            else
                return 0;
        }

        public static void UpdateStat(string _stat, int _value)
        {
            _stat = _stat.ToLower();
            if (_stat == "ar")
                _stat = _stat.ToUpper();
            else
                _stat = char.ToUpper(_stat[0]) + _stat.Substring(1);

            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            settings.Characters[character].GetType().GetProperty(_stat).SetValue(settings.Characters[character], _value, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        public static string GetClass()
        {
            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            if (settings.Characters.ContainsKey(character))
                return (string)settings.Characters[character].GetType().GetProperty("CharacterClass").GetValue(settings.Characters[character]);
            else
                return "other";
        }

        public static void UpdateClass(string _class)
        {
            Settings settings = GetSettings();
            string character = settings.LastSelectedCharacter;
            settings.Characters[character].GetType().GetProperty("CharacterClass").SetValue(settings.Characters[character], _class, null);
            string jsonString = "";
            try
            {
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                File.WriteAllText(Path.Combine(Settings, SettingsFile), jsonString);
            }
            catch (JsonException _ex)
            {
            }
        }

        static Settings GetSettings()
        {
            if (!File.Exists(Path.Combine(Settings, SettingsFile)))
            {
                return new Settings();
            }
            string jsonString = File.ReadAllText(Path.Combine(Settings, SettingsFile));
            return JsonSerializer.Deserialize<Settings>(jsonString, new JsonSerializerOptions { IncludeFields = true });
        }
    }
}

