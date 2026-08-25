namespace PRKHelp.Settings
{
    public class PathSettings
    {
        public string LogFilePath { get; set; } = "";
        public string LogCombatFilePath { get; set; } = "";
        public (int, int, int)[] Gear { get; set; } = new (int, int, int)[3];
        public (int, int, int)[] Plan { get; set; } = new (int, int, int)[3];
        public int Init { get; set; } = 0;
        public int Crit { get; set; } = 3;
        public int AR { get; set; } = 0;
        public int Dmg { get; set; } = 0;
        public int Complit { get; set; } = 0;
        public string CharacterClass { get; set; } = "other";
    }

    public class Settings
    {
        public Dictionary<string, PathSettings> Characters { get; set; } = new Dictionary<string, PathSettings>();
        public string ScriptsPath { get; set; } = "";
        public string LastSelectedCharacter { get; set; } = "";
        public int WindowPositionTop { get; set; } = 0;
        public int WindowPositionLeft { get; set; } = 0;
        public string ShopMessage { get; set; } = "Peruse my ";
    }
}

