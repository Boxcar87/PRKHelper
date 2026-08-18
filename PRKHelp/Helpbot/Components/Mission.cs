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

            OutputStrings[0] = $"Result of function";
            OutputStrings.Add($"Additional scripts can be generated as new lines"); // Useful when text output would exceed 4096 characters (/text limit)

            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        //mission_coefficients = [0.7001, 0.75, 0.8, 0.85, 0.9, 1.0, 1.1, 1.2, 1.3, 1.5, 1.7913]
        //mission_levels = set()
        //for i in mission_coefficients:
        //    val = math.floor(level* i)
        //    if val< 1:
        //        val = 1
        //    elif val > 250:
        //        val = 250

        //    # I couldn't get 4 values to match with 1.3?
        //    if i == 1.3 and(level == 90 or level == 170 or level == 180 or level == 190) :
        //        val = val - 1
    }
}
