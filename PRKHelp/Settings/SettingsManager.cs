
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text.Json;

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
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
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
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
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
                string newFileString = JsonSerializer.Serialize(new Settings(), new JsonSerializerOptions { WriteIndented = true });
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
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
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
                jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
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
            return JsonSerializer.Deserialize<Settings>(jsonString);
        }
    }
}

