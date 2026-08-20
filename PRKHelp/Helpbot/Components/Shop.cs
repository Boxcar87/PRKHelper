using PRKHelp;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace PRKHelper.Helpbot.Components
{
    public class ShopItem
    {
        internal int lowid { get; set; }
        internal int highid { get; set; }
        internal int ql { get; set; }
        internal string name { get; set; }
        internal int type { get; set; }
    }
        
    public class ShopInventory
    {
        internal List<ShopItem> Unsorted { get; set; }
        internal List<ShopItem> Weapons { get; set; }
        internal List<ShopItem> Armor { get; set; }
        internal List<ShopItem> Symbiants { get; set; }
        internal List<ShopItem> Utilities { get; set; }
        internal List<ShopItem> Nanos { get; set; }
    }
    public class Shop : Component
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
        public Shop(DB _db)
        {
            DB = _db;
            // Base class will perform basic validation on params
            ParamSyntax = "/shop add (insert item)";
        }

        // If you have variable inputs you can override ValidateParams, otherwise this performs basic param validation
        public override (int, string) ValidateParams(string[] _params)
        {
            int statusCode = -1;
            string statusMessage = "Invalid params";

            if (_params.Length > 0)
            {
                if (_params[0] == "<a" || _params[0] == "add" && _params[1] == "<a")
                {
                    statusCode = 1;
                    statusMessage = "Accepted";
                }

                if (_params.Length == 3)
                {
                    bool allInts = true;
                    foreach (string param in _params)
                    {
                        bool isInt = int.TryParse(param, out statusCode);
                        if (!isInt)
                        {
                            allInts = false;
                            break;
                        }
                    }

                    if (allInts)
                    {
                        statusCode = 1;
                        statusMessage = "Accepted";
                    }
                }
            }            

            return (statusCode, statusMessage);
        }

        // Perform function logic here
        public override int Process(string[] _params)
        {
            int statusCode = 1; // -1 for error 1 for success

            int lowid = 0;
            int highid = 0;
            int ql = 0;

            if (_params[0] == "add" || _params[0] == "<a")
            {
                Debug.WriteLine(_params[0]);
                if (_params[0] == "add")
                {
                    _params = _params[1..];

                    Debug.WriteLine("yes again");
                }

                ShopItem newItem = ParseItem(string.Join(" ",_params));
                lowid = newItem.lowid;
                highid = newItem.highid;
                ql = newItem.ql;
            }
            else
            {
                lowid = int.Parse(_params[0]);
                highid = int.Parse(_params[1]);
                ql = int.Parse(_params[2]);
            }

            (int status, string shopText) = AddToShop(lowid, highid, ql);
            if (status < 0)
            {
                statusCode = -1;
                OutputStrings[0] = $"{TextColor}Item was unable to be added.";
            }
            if (shopText.Length > 1000)
            {
                statusCode = -1;
                OutputStrings[0] = $"{TextColor}Maximum script length reached.";
            }
            else
            {
                OutputStrings[0] = $"{TextColor}Item successfully added to shop";
                ScriptManager.UpdateShop(shopText);
            }

            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        private (int, string) AddToShop(int lowid, int highid, int ql) /* _params should be as follows | add category <a href.... |*/
        {
            //Dictionary<string, List<ShopItem>> ShopItemsByName = new Dictionary<string, List<ShopItem>>();
            Dictionary<string, Dictionary<string, List<ShopItem>>> oldItems = new Dictionary<string, Dictionary<string, List<ShopItem>>>();

            //Open shop script        
            using (FileStream fileStream = new(Path.Combine(ScriptManager.ScriptFolder, "PRKHelp/Shop"), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    //Store all items into memory (in sections, ie: Unsorted, Weapons, Symbiants, etc)
                    //"<br>     -" will mean following is an item, read until </a>
                    //"<br>|" will be a Category header, read text between | |
                    string rawText = streamReader.ReadToEnd();
                    string category = "";
                    string textToRead = rawText;

                    while (textToRead.Length > 0)
                    {
                        int categoryIndex = textToRead.IndexOf("<br>|", StringComparison.Ordinal);
                        int itemIndex = textToRead.IndexOf("<br>     -", StringComparison.Ordinal);

                        if (categoryIndex == -1)
                        {
                            //Prevents items in last category from failing
                            categoryIndex = 100000;
                        }

                        if (itemIndex > -1 && itemIndex < categoryIndex)
                        {
                            // Moving to end of item index flag
                            itemIndex += 10;
                            int itemEndIndex = textToRead.IndexOf("</a>")+4;
                            string currentItemString = textToRead[itemIndex..itemEndIndex];
                            ShopItem currentItem = ParseItem(currentItemString);

                            if (!oldItems.ContainsKey(category))
                            {
                                oldItems[category] = new Dictionary<string, List<ShopItem>>();
                                oldItems[category][currentItem.name] = new List<ShopItem> { currentItem };
                            }
                            else
                            {
                                if (oldItems[category].ContainsKey(currentItem.name))
                                {
                                    bool inserted = false;
                                    List<ShopItem> sameNameItems = oldItems[category][currentItem.name];
                                    for (var i = 0; i < sameNameItems.Count; i++)
                                    {
                                        if (currentItem.ql > sameNameItems[i].ql)
                                        {
                                            oldItems[category][currentItem.name].Insert(i, currentItem);
                                            inserted = true;
                                            break;
                                        }
                                    }
                                    if (!inserted)
                                    {
                                        oldItems[category][currentItem.name].Add(currentItem);
                                    }
                                }
                                else
                                {
                                    oldItems[category][currentItem.name] = new List<ShopItem> { currentItem };
                                }
                            }
                            textToRead = textToRead[textToRead.IndexOf("</a>")..];
                            textToRead = textToRead[3..];
                        }

                        else if (categoryIndex < itemIndex)
                        {
                            // Moving to end of category index flag
                            categoryIndex += 5;
                            textToRead = textToRead[categoryIndex..];
                            category = textToRead[..textToRead.IndexOf("|")];
                            textToRead = textToRead[textToRead.IndexOf("|")..];
                        }

                        if (itemIndex == -1)
                            textToRead = "";
                    }
                }
            }

            // We Can determine the category of the item to some degree. item.c 1 == Weapon | item.c 2 == Armor. We can parse Nano Crystal to check for nanos and Symbiant to check for symb(3rd word is always Symbiant)
            // This would leave us with Weapons|Armor|Nanos|Symbiants|Other as categories. The user never needs to input the category this way.
            // For each shop item added, lookup the low and high id paired with ql in itemDB 

            Dictionary<int, string> categories = new Dictionary<int, string>
            {
                [0] = "Other",
                [1] = "Weapons",
                [2] = "Armor"
            };

            AOItem itemActual = GetItemByIDs(lowid, highid);
            string itemCategory = "Other";

            if (itemActual.name == null)
            {
                return (-1, "Item not inside database. Filled implants not supported");
            }
            if (itemActual.name.Length > 13)
            {
                if (itemActual.name[..12] == "Nano Crystal" || itemActual.name[..12] == "NanoCrystal ")
                {
                    itemCategory = "Nanos";
                }
                else if (itemActual.name.Contains(","))
                {
                    if (itemActual.name.Substring(itemActual.name.IndexOf(",") - 8, 8) == "Symbiant")
                    {
                        itemCategory = "Symbiants";
                    }
                }
                else if (categories.ContainsKey(itemActual.type))
                {
                    itemCategory = categories[itemActual.type];
                }
            }            
            else if (categories.ContainsKey(itemActual.type))
            {
                itemCategory = categories[itemActual.type];
            }
            ShopItem itemToAdd = new ShopItem
            {
                lowid = lowid,
                highid = highid,
                ql = ql,
                name = itemActual.name
            };
            //Insert new item in correct section in alphabetical order followed by ql order
            if (!oldItems.ContainsKey(itemCategory))
            {
                oldItems[itemCategory] = new Dictionary<string, List<ShopItem>>();
                oldItems[itemCategory][itemToAdd.name] = new List<ShopItem> { itemToAdd };
            }
            else
            {
                if (!oldItems[itemCategory].ContainsKey(itemToAdd.name))
                {
                    oldItems[itemCategory][itemToAdd.name] = new List<ShopItem> { itemToAdd };
                }
                else
                {
                    bool inserted = false;
                    for (var i = 0; i < oldItems[itemCategory][itemToAdd.name].Count; i++)
                    {
                        if (itemToAdd.ql > oldItems[itemCategory][itemToAdd.name][i].ql)
                        {
                        oldItems[itemCategory][itemToAdd.name].Insert(i, itemToAdd);
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        oldItems[itemCategory][itemToAdd.name].Add(itemToAdd);
                    }
                }
            }

            //Rewrite shop script
            //Manual ordering of shop categories
            string shopText = "<a href=\"text://";
            string editShopText = "<a href=\"text://";
            if (oldItems.ContainsKey("Weapons"))
            {
                shopText += $"<br>|Weapons|";
                foreach ((string key, List<ShopItem> value) in oldItems["Weapons"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} - [{ValueColor}{item.ql}{EndColor}] ";
                        shopText += $"- {RedColor}Remove{EndColor} <a href='chatcmd:///shop remove {item.lowid} {item.highid} {item.ql}'>[X]</a>";
                        editShopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} - [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $"<br>          {RedColor}Remove{EndColor} item from shop <a href='chatcmd:///shop remove {item.lowid} {item.highid} {item.ql}'>[X]</a>";
                    }
                }
                shopText += "<br>";
            }
            if (oldItems.ContainsKey("Armor"))
            {
                shopText += "<br>|Armor|";
                foreach ((string key, List<ShopItem> value) in oldItems["Armor"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} - [{ValueColor}{item.ql}{EndColor}]";
                    }
                }
                shopText += "<br>";
            }
            if (oldItems.ContainsKey("Nanos"))
            {
                shopText += "<br>|Nanos|";
                foreach ((string key, List<ShopItem> value) in oldItems["Nanos"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} - [{ValueColor}{item.ql}{EndColor}]";
                    }
                }
                shopText += "<br>";
            }
            if (oldItems.ContainsKey("Symbiants"))
            {
                shopText += "<br>|Symbiants|";
                foreach ((string key, List<ShopItem> value) in oldItems["Symbiants"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} - [{ValueColor}{item.ql}{EndColor}]";
                    }
                }
                shopText += "<br>";
            }
            if (oldItems.ContainsKey("Implants"))
            {
                shopText += "<br>|Implants|";
                foreach ((string key, List<ShopItem> value) in oldItems["Implants"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} - [{ValueColor}{item.ql}{EndColor}]";
                    }
                }
                shopText += "<br>";
            }
            if (oldItems.ContainsKey("Other"))
            {
                shopText += "<br>|Other|";
                foreach ((string key, List<ShopItem> value) in oldItems["Other"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} - [{ValueColor}{item.ql}{EndColor}]";
                    }
                }
                shopText += "<br>";
            }
            shopText += "<br><br><div align=right>Generated with PRKHelper\">Shop</a>";
            return (1, shopText);
        }
        private ShopItem ParseItem(string _itemString)
        {
            //"<a href=\'itemref://{_minID}/{_maxID}/{_QL}\'>{_name}</a>"
            _itemString = _itemString.Replace("\"", "\'");
            int start = _itemString.IndexOf("//")+2;
            string clipped = _itemString[start..^4];
            string numberString = clipped[..clipped.IndexOf("\'")];
            string[] numbers = numberString.Split('/');
            int nameStart = clipped.IndexOf(">")+1;

            return new ShopItem
            {
                lowid = int.Parse(numbers[0]),
                highid = int.Parse(numbers[1]),
                ql = int.Parse(numbers[2]),
                name = clipped[nameStart..]
            };
        }
        static AOItem GetItemByIDs(int _lowid, int _highid)
        {

            string query = $"SELECT * FROM Items WHERE lowid == {_lowid} AND highid == {_highid}";

            return DB.QueryItemByIDs(query);
        }
    }
}
