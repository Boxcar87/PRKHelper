using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace PRKHelper.Helpbot.Components
{
    public class Mission : Component
    {
        //string TextColor
        //string ValueColor
        //string HighlightColor
        //string RedColor
        //string EndColor
        //string Indent

        //List<Type> ParamTypes

        DB DB;
        //List<string> OutputStrings; // Inherited object retrieved for response by Route()
        public Mission(DB _db) // Pass DB reference in from route if needed
        {
            DB = _db;

            // Base class will perform basic validation on params
            ParamSyntax = "/mission 123";
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
                statusMessage = "Mission must be greater than 0";
                statusCode = -1;
            }
            if (level > 250)
            {
                statusMessage = "Mission must be less than 250";
                statusCode = -1;
            }

            return (statusCode, statusMessage);
        }

        // Perform function logic here
        public override int Process(string[] _params)
        {
            int statusCode = 1; // -1 for error 1 for success

            OutputStrings[0] = $"{TextColor}Mission level{EndColor} {_params[0]} {TextColor}can be rolled by levels |{EndColor}";
            foreach (short level in GetLevels(int.Parse(_params[0])))
            {
                OutputStrings[0] += $"{ValueColor}{level}{EndColor}{TextColor}|{EndColor}";
            }

            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        private List<short> GetLevels(int _mission)
        {
            string query = $"SELECT * FROM Levels WHERE missions LIKE '%,{_mission},%'";

            return DB.QueryMissions(query);
        }
    }
}
