namespace PRKHelp.Settings
{
    public class PathSettings
    {
        public string LogFilePath { get; set; } = "";
        public string LogCombatFilePath { get; set; } = "";
    }

    public class Settings
    {
        public Dictionary<string, PathSettings> Characters { get; set; } = new Dictionary<string, PathSettings>();
        public string ScriptsPath { get; set; } = "";
        public string LastSelectedCharacter { get; set; } = "";
    }
}

