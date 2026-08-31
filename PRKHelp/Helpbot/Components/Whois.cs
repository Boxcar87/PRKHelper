using System.Diagnostics;
using PRKHelp.Settings;
using static System.Windows.Forms.AxHost;

namespace PRKHelper.Helpbot.Components
{
    public class CharacterAlts
    {
        internal string className { get; set; }
        internal List<(string, string)> alts { get; set; } = new();
    }

    public class Whois : Component
    {
        //string TextColor
        //string ValueColor
        //string HighlightColor
        //string RedColor
        //string EndColor
        //string Indent

        //List<Type> ParamTypes

        //List<string> OutputStrings; // Inherited object retrieved for response by Route()
        public Whois(/*DB _db*/) // Pass DB reference in from route if needed
        {
            // Base class will perform basic validation on params
            // whois name class classname
            // whois name main name
            // whois name add name
            ParamSyntax = "/whois string (add altName)";
        }

        // If you have variable inputs you can override ValidateParams, otherwise this performs basic param validation
        public override (int, string) ValidateParams(string[] _params)
        {
            if (_params.Length == 1)
                return (1, "Accepted");

            else if (_params.Length < 2)
                return (-1, "Invalid Params");

            if (_params[1] == "add" || _params[1] == "remove")
            {
                if (_params.Length > 4)
                    return (-1, $"Please enter only an alt or alt and class for {_params[0]}");
                else if (_params.Length < 3)
                    return (-1, $"Please enter at least an alt to add for {_params[0]}");
                else
                {
                    if (_params.Length == 4)
                    {
                        switch (_params[3].ToLower())
                        {
                            case "adv":
                            case "adventurer":
                            case "agent":
                            case "crat":
                            case "bureaucrat":
                            case "doc":
                            case "doctor":
                            case "enf":
                            case "enforcer":
                            case "eng":
                            case "engi":
                            case "engineer":
                            case "fix":
                            case "fixer":
                            case "keep":
                            case "keeper":
                            case "ma":
                            case "martial":
                            case "martialartist":
                            case "meta":
                            case "mp":
                            case "metaphysicist":
                            case "meta-physicist":
                            case "nt":
                            case "nanotech":
                            case "nano-tech":
                            case "nanotechnician":
                            case "nano-technicion":
                            case "shade":
                            case "sol":
                            case "sold":
                            case "soldier":
                            case "trade":
                            case "trader":
                                return (1, "Accepter");
                            default:
                                return (-1, "Please enter a valid class as 1 word (eg: martialartist");
                        }
                    }
                    else
                        return (1, "Accepted");
                }
            }
            else if (_params[1] == "class")
            {
                if (_params.Length != 3)
                    return (-1, $"Please enter a class for {_params[0]}");
                switch (_params[2].ToLower())
                {
                    case "adv":
                    case "adventurer":
                    case "agent":
                    case "crat":
                    case "bureaucrat":
                    case "doc":
                    case "doctor":
                    case "enf":
                    case "enforcer":
                    case "eng":
                    case "engi":
                    case "engineer":
                    case "fix":
                    case "fixer":
                    case "keep":
                    case "keeper":
                    case "ma":
                    case "martial":
                    case "martialartist":
                    case "meta":
                    case "mp":
                    case "metaphysicist":
                    case "meta-physicist":
                    case "nt":
                    case "nanotech":
                    case "nano-tech":
                    case "nanotechnician":
                    case "nano-technicion":
                    case "shade":
                    case "sol":
                    case "sold":
                    case "soldier":
                    case "trade":
                    case "trader":
                        return (1, "Accepter");
                    default:
                        return (-1, "Please enter a valid class as 1 word (eg: martialartist");
                }
            }
            else if (_params[1] == "main")
            {
                if (_params.Length != 3)
                    return (-1, $"Please enter a class for {_params[0]}");
                else
                    return (1, "Accepted");
            }

            return (-1, "Invalid Params");
        }

        // Perform function logic here
        public override int Process(string[] _params)
        {
            int statusCode = 1; // -1 for error 1 for success
            _params[0] = _params[0].ToLower();

            List<(string, string, string)> altsList = SettingsManager.GetWhoisList();

            Dictionary<string, CharacterAlts> allAlts = new Dictionary<string, CharacterAlts>();

            string mainCharacter = "";
            string className = "";
            
            // Arrange saved characters into alt lists
            for (int i=0; i<altsList.Count; i++)
            {
                if (altsList[i].Item1 == _params[0])
                {
                    mainCharacter = altsList[i].Item2;
                    className = altsList[i].Item3;
                }

                if (!allAlts.ContainsKey(altsList[i].Item2))
                {
                    allAlts.Add(altsList[i].Item2, new CharacterAlts());
                    if (altsList[i].Item1 == altsList[i].Item2)
                    {
                        allAlts[altsList[i].Item2].className = altsList[i].Item3;
                    }
                    else
                    {
                        if (altsList[i].Item1 != altsList[i].Item2)
                            allAlts[altsList[i].Item2].alts.Add((altsList[i].Item1, altsList[i].Item3));
                    }
                    //allAlts.Add(altsList[i].Item2, character);
                }
                else
                {
                    if (altsList[i].Item1 == altsList[i].Item2)
                        allAlts[altsList[i].Item2].className = altsList[i].Item3;
                    else
                        allAlts[altsList[i].Item2].alts.Add((altsList[i].Item1, altsList[i].Item3));
                }
            }

            // Find the main of the character given and list all associated alts
            if (_params.Length == 1)
            {
                if (mainCharacter == "")
                {
                    OutputStrings[0] = $"No data found for {_params[0]}";
                    return statusCode;
                }
                // Draw all alts of character as response
                string mainName = char.ToUpper(mainCharacter[0]) + mainCharacter.Substring(1).ToLower();
                string searchedName = char.ToUpper(_params[0][0]) + _params[0].Substring(1).ToLower();

                // Name searched is the main
                if (_params[0] == mainCharacter)
                {
                    OutputStrings[0] = $"{ValueColor}{searchedName}{EndColor}{TextColor} ";

                    if (className != "(Unknown Class)")
                        OutputStrings[0] += $"- {className} - ";
                    else
                        OutputStrings[0] += $"- ";
                }
                else
                {
                    OutputStrings[0] = $"{TextColor}{searchedName} ";
                    if (className != "(Unknown Class)")
                        OutputStrings[0] += $"- {className} | Alt of {ValueColor}{mainName}{EndColor} - ";
                    else
                        OutputStrings[0] += $"| Alt of {ValueColor}{mainName}{EndColor} - ";
                }

                string altsHref = $"<a href=\"text://{HighlightColor}{mainName} - {allAlts[mainCharacter].className}{EndColor}<br><br>Alts:<br>";
                if (allAlts[mainCharacter].alts.Count > 0)
                {
                    foreach ((string, string) alt in allAlts[mainCharacter].alts)
                    {
                        string altName = char.ToUpper(alt.Item1[0]) + alt.Item1.Substring(1).ToLower();
                        altsHref += $"{Indent}- {ValueColor}{altName}{EndColor} | {alt.Item2}<br>";
                    }
                }
                else
                    altsHref += $"No alts found. Type \'/whois {mainName} add nameofalt\' to add a new alt to this character.";

                altsHref += $"\">Alts</a>";
                OutputStrings[0] += altsHref;
            }

            // Change class for specific character
            else if (_params[1] == "class")
            {
                string classEntry = GetClass(_params[2]);
                bool inserted = false;
                for (int c = 0; c < altsList.Count; c++)
                {
                    (string, string, string) clone = altsList[c];

                    // Character == input character
                    if (altsList[c].Item1 == _params[0].ToLower())
                    {
                        clone.Item3 = classEntry;
                        altsList[c] = clone;
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                {
                    altsList.Add((_params[0].ToLower(), _params[0].ToLower(), classEntry));
                }
                SettingsManager.OverwriteWhoisList(altsList);

                OutputStrings[0] = $"{TextColor}Set {_params[0]} class to {classEntry}";
            }

            // Add character to saved data with support for inline class declaration
            else if (_params[1] == "add")
            {
                string newName = char.ToUpper(_params[2][0]) + _params[2].Substring(1);
                string mainName = "";

                if (mainCharacter != "")
                {
                    mainName = char.ToUpper(mainCharacter[0]) + mainCharacter.Substring(1);
                }
                else
                    mainName = char.ToUpper(_params[0][0]) + _params[0].Substring(1);

                bool exists = false;
                bool mainExists = false;
                for (int a = 0; a < altsList.Count; a++)
                {
                    (string, string, string) clone = altsList[a];

                    // Character == input character
                    if (altsList[a].Item2 == mainCharacter)
                    {
                        if (altsList[a].Item1 == _params[2].ToLower())
                        {
                            if (_params.Length == 4)
                            {
                                string classEntry = GetClass(_params[3]);
                                clone.Item3 = classEntry;
                                altsList[a] = clone;
                            }
                            exists = true;
                        }
                        // Check to see if there is an entry for the main itself
                        if (altsList[a].Item1 == altsList[a].Item2)
                            mainExists = true;
                    }
                }
                if (!exists)
                {
                    string classEntry = _params.Length == 4 ? GetClass(_params[3]) : "(Unknown Class)";
                    altsList.Add((_params[2].ToLower(), mainCharacter, classEntry));
                    SettingsManager.OverwriteWhoisList(altsList);
                }
                if (!mainExists)
                {
                    altsList.Add((mainName.ToLower(), mainName.ToLower(), "(Unknown Class)"));
                    SettingsManager.OverwriteWhoisList(altsList);
                }

                OutputStrings[0] = $"{TextColor}Added {newName} as an alt of {ValueColor}{mainName}";
            }

            // Remove character from saved ata
            else if (_params[1] == "remove")
            {
                bool removed = false;
                for (int r=0; r<altsList.Count; r++)
                {
                    (string, string, string) clone = altsList[r];

                    if (altsList[r].Item1 == _params[2].ToLower())
                    {
                        altsList.RemoveAt(r);
                        removed = true;
                    }
                }
                if (removed)
                {
                    string newName = char.ToUpper(_params[2][0]) + _params[2].Substring(1);
                    string mainName = char.ToUpper(mainCharacter[0]) + mainCharacter.Substring(1);
                    if (mainCharacter == "")
                    {
                        OutputStrings[0] = $"No character data found for {ValueColor}{_params[0]}";
                    }

                    SettingsManager.OverwriteWhoisList(altsList);
                    OutputStrings[0] = $"{TextColor}Removed {newName} from alts of {ValueColor}{mainName}";
                }
            }

            // Change main character for all associated alts
            else if (_params[1] == "main")
            {
                string newMainName = char.ToUpper(_params[2][0]) + _params[2].Substring(1).ToLower();
                string searchedName = char.ToUpper(_params[0][0]) + _params[0].Substring(1).ToLower();
                for (int m = 0; m < altsList.Count; m++)
                {
                    (string, string, string) clone = altsList[m];

                    // Character == input character
                    if (altsList[m].Item2 == mainCharacter)
                    {
                        clone.Item2 = _params[2].ToLower();
                        altsList[m] = clone;
                    }
                    SettingsManager.OverwriteWhoisList(altsList);
                }
                OutputStrings[0] = $"{TextColor}Set main of {searchedName} and all associated alts as {ValueColor}{newMainName}";
            }

            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        private string GetClass(string _className)
        {
            switch (_className.ToLower())
            {
                case "adv":
                case "adventurer":
                    return "Adventurer";
                case "agent":
                    return "Agent";
                case "crat":
                case "bureaucrat":
                    return "Bureaucrat";
                case "doc":
                case "doctor":
                    return "Doctor";
                case "enf":
                case "enforcer":
                    return "Enforcer";
                case "eng":
                case "engi":
                case "engineer":
                    return "Engineer";
                case "fix":
                case "fixer":
                    return "Fixer";
                case "keep":
                case "keeper":
                    return "Keeper";
                case "ma":
                case "martial":
                case "martialartist":
                    return "Martial Artist";
                case "meta":
                case "mp":
                case "metaphysicist":
                case "meta-physicist":
                    return "Meta-Physicist";
                case "nt":
                case "nanotech":
                case "nano-tech":
                case "nanotechnician":
                case "nano-technician":
                    return "Nano Technician";
                case "shade":
                    return "Shade";
                case "sol":
                case "sold":
                case "soldier":
                    return "Soldier";
                case "trade":
                case "trader":
                    return "Trader";
                default:
                    return "(Unknown Class)";
            }
        }
    }
}
