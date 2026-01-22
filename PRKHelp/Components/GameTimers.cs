using System.Text.RegularExpressions;

namespace PRKHelp.Components
{
    public class GameTimers : Component
    {
        static Dictionary<string, (long, System.Timers.Timer)> CustomTimers = new Dictionary<string, (long, System.Timers.Timer)>();
        //static string TimerEndedWriteFile;

        public override (int, string) ValidateParams(string[] _params)
        {
            if (_params.Length == 0)
                return (1, "true");
            
            string name = _params[0];
            if (CustomTimers.ContainsKey(name))
                return (-1, $"/text Timer already exists for {name}!");

            string duration = _params[1];
            long durationLimit = 2 * 3600000; // Do you want a limit?
            duration = duration.Trim(' ');
            long totalDuration = 0;
            List<string> uniqueDenotationsUsed = [];

            // Split the string into sections based on hour minute and second.
            string[] durationSections = Regex.Split(duration, @"(?<=[hm])");
            foreach (string section in durationSections)
            {
                if (section == string.Empty) // Clean up hanging empty elements in array
                    continue;

                string denotation = section[^1].ToString();
                string durationOnly = section[..^1];

                // Ensure the format doesnt have duplicates like 1h1h1s
                if (!uniqueDenotationsUsed.Contains(denotation))
                    uniqueDenotationsUsed.Add(denotation);
                else
                    return (-1, "/text Duplicate denotation used. Please used format 1h10m30s");

                bool validDuration = long.TryParse(durationOnly, out long durationTime);
                if (!validDuration)
                    return (-1, "/text Invalid duration. Please use format 1h10m30s");
                if (durationTime < 0)
                    return (-1, "/text Duration must be greater than 0. Please use format 1h10m30s");

                totalDuration += ParseDenotation(denotation.ToLower(), durationTime);
            }
           
            if (totalDuration > durationLimit)
                return (-1, "/text No No No, I want to be alive when this timer ends. 1 Hour maximum");

            return (1, "true");
        }
        public override int Process(string[] _params)
        {
            if (_params.Length == 0)
                return GetTimers();
            else
                return CreateTimer(_params[0], _params[1]);
        }
        public int CreateTimer(string name, string duration)
        {
            duration = duration.Trim(' ');
            long totalDuration = 0;

            // Split the string into sections based on hour minute and second.
            string[] durationSections = Regex.Split(duration, @"(?<=[hm])");
            foreach (string section in durationSections)
            {
                if (section == string.Empty) // Clean up hanging empty elements in array
                    continue;

                string denotation = section[^1].ToString();
                string durationOnly = section[..^1];

                long durationTime = long.Parse(durationOnly);

                totalDuration += ParseDenotation(denotation.ToLower(), durationTime);
            }

            long timerEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds() + totalDuration;

            // Create new timer and pass name into callback for proper termination once timer ends
            System.Timers.Timer newTimer = new(totalDuration);
            newTimer.Elapsed += (sender, e) => ExpireTimer(name);
            newTimer.AutoReset = false;
            newTimer.Start();

            CustomTimers.Add(name, (timerEnd, newTimer));
            OutputStrings[0] = $"/text Created timer for {name} Successfully";
            return 1;
        }

        public int GetTimers()
        {
            OutputStrings[0] = "/text <a href=\"text://Active Timers:<br><br>";
            foreach (KeyValuePair<string, (long, System.Timers.Timer)> timer in CustomTimers)
            {
                string secondsColor = TextColor;
                long remaining = timer.Value.Item1 - DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (remaining <= 60000)
                    secondsColor = RedColor;

                OutputStrings[0] += $"{Indent}{HighlightColor}{timer.Key}{EndColor}: {secondsColor}{ParseTimeRemaining(timer.Value.Item1 - DateTimeOffset.Now.ToUnixTimeMilliseconds())}{EndColor} remaining<br><br>";
            }
            OutputStrings[0] += "\">Current Timers</a>";
            return 1;
        }
        static void ExpireTimer(string name)
        {
            CustomTimers[name].Item2.Dispose();
            //Console.WriteLine($"{name} timer has been disposed"); // Left in for testing functionality
            CustomTimers.Remove(name);
            //File.WriteAllText(TimerEndedWriteFile, $"!TimerEnd Timer: {name} has just expired!");
        }

        // Convert time into readable string
        static string ParseTimeRemaining(long remaining)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(remaining);

            remaining /= 1000;
            switch (remaining)
            {
                case >= 3600: // Hours
                    return $"{t.Hours}h {t.Minutes}m {t.Seconds}s";
                case >= 60: // Minutes
                    return $"{t.Minutes}m {t.Seconds}s";
                default:
                    return $"{t.Seconds}s";
            }
        }

        static long ParseDenotation(string _denotation, long _duration)
        {
            switch (_denotation.ToLower())
            {
                case "s":
                    _duration *= 1000;
                    break;
                case "m":
                    _duration *= 60000;
                    break;
                case "h":
                    _duration *= 3600000;
                    break;
                default:
                    break;
            }
            return _duration;
        }
    }
}
