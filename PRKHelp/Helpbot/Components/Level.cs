namespace PRKHelper.Helpbot.Components
{
    public class LevelData
    {
        public short level { get; set; }
        public short teamMin { get; set; }
        public short teamMax { get; set; }
        public short pvpMin { get; set; }
        public short pvpMax { get; set; }
        public int xpToLevel { get; set; }
        public short tokens { get; set; }
        public required string missions { get; set; }
    }

    public class Level : Component
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

        public Level(DB _db) // Pass DB reference in from route if needed
        {
            DB = _db;
            LoadItems();

            // Base class will perform basic validation on params
            ParamSyntax = "/level 123";
            ParamTypes.Add(typeof(int));
        }

        // Use this function override to append additional param checks if needed
        public override (int, string) SpecificParamChecks(string[] _params)
        {
            int level = int.Parse(_params[0]);
            int statusCode = 1; // -1 for error 1 for success
            string statusMessage = "";
            if (level <= 0)
            {
                statusMessage = "Level must be greater than 0";
                statusCode = -1;
            }
            if (level > 220)
            {
                statusMessage = "Level must be less than 220";
                statusCode = -1;
            }

            return (statusCode, statusMessage);
        }

        // Perform function logic here
        public override int Process(string[] _params)
        {
            int statusCode = 1; // -1 for error 1 for success
            LevelData data = GetLevel(int.Parse(_params[0]));

            OutputStrings[0] = $"<a href=\"text://Level Ranges for {data.level}";
            OutputStrings[0] += $"{Indent} {HighlightColor}Team Ranges{EndColor}<br>{Indent}{Indent}Min: {ValueColor}{data.teamMin}{EndColor} Max: {ValueColor}{data.teamMax}{EndColor}<br><br>";
            OutputStrings[0] += $"{Indent} {RedColor}PvP Ranges{EndColor}<br>{Indent}{Indent}Min: {ValueColor}{data.pvpMin}{EndColor} Max: {ValueColor}{data.pvpMax}{EndColor}<br><br>";
            OutputStrings[0] += $"{Indent} Missions: {ValueColor}{data.missions}{EndColor}";
            OutputStrings[0] += $"\">Level Ranges for {data.level}</a>";


            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }
        static LevelData GetLevel(int _level)
        {
            string query = $"SELECT * FROM Levels ORDER BY column_name LIMIT 1 OFFSET {_level-1};";

            return DB.QueryLevel(query);
        }
            static void LoadItems()
        {
            DB.InsertSQLFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Helpbot\\SQL\\Levels.sql");
        }
        // Function you have access to in base class which quickly builds an itemref link - Do Not uncomment this, it is only for your reference, the function is inherited
        //public string BuildItemRef(int _minID, int _maxID, int _QL, string _name)
        //{
        //    return $"<a href=\'itemref://{_minID}/{_maxID}/{_QL}\'>{_name}</a>";
        //}
    }
}
