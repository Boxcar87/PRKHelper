using PRKHelp;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using PRKHelp.Settings;

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
            string manualItemString = string.Join(" ", _params);
            manualItemString = manualItemString.Replace("&lt;", "<");
            manualItemString = manualItemString.Replace("&gt;", ">");
            _params = manualItemString.Split(" ");

            int statusCode = -1;
            string statusMessage = "Invalid params";

            if (_params.Length == 0)
            {
                statusCode = 1;
                statusMessage = "Accepted";
            }
            else if (_params.Length > 0)
            {
                if (_params[0] == "<a" || _params[0] == "add" && _params[1] == "<a")
                {
                    statusCode = 1;
                    statusMessage = "Accepted";
                }
                else if (_params[0] == "remove")
                {
                    statusCode = 1;
                    statusMessage = "Accepted";
                }
                else if (_params[0] == "text")
                {
                    statusCode = 1;
                    statusMessage = "Accepted";
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

            string route = "";

            if (_params.Length == 0)
            {
                route = "view";
            }
            else if (_params[0] == "add" || _params[0] == "<a")
            {
                route = "add";
                if (_params[0] == "add")
                {
                    _params = _params[1..];
                }

                ShopItem newItem = ParseItem(string.Join(" ", _params));
                lowid = newItem.lowid;
                highid = newItem.highid;
                ql = newItem.ql;
            }
            else if (_params[0] == "remove" && int.TryParse(_params[1], out statusCode) && int.TryParse(_params[2], out statusCode) && int.TryParse(_params[3], out statusCode))
            {
                route = "remove";

                lowid = int.Parse(_params[1]);
                highid = int.Parse(_params[2]);
                ql = int.Parse(_params[3]);
            }
            else if (_params[0] == "text")
            {
                route = "text";
            }

            if (route == "add")
            {
                (int status, string shopText, string editShopText) = AddToShop(lowid, highid, ql);
                if (status < 0)
                {
                    // Using the shopText item to relay soft errors
                    OutputStrings[0] = $"{TextColor}{shopText}";
                }
                else
                {
                    OutputStrings[0] = $"{TextColor}Item successfully added to shop - View {editShopText}";
                    ScriptManager.UpdateShop(shopText, editShopText);
                }
            }
            else if (route == "remove")
            {
                (int status, string shopText, string editShopText) = RemoveFromShop(lowid, highid, ql);
                if (status < 0)
                {
                    // Using the shopText item to relay soft errors
                    OutputStrings[0] = $"{TextColor}{shopText}";
                }
                else
                {
                    OutputStrings[0] = $"{TextColor}Item successfully added to shop - View {editShopText}";
                    ScriptManager.UpdateShop(shopText, editShopText);
                }
            }
            else if (route == "text")
            {
                _params = _params[1..];
                string newText = string.Join(" ", _params);
                OutputStrings[0] = $"{TextColor}Shop text updated - \'{newText}\'";
                (string shopText, string editShopText) = UpdateText(newText);
                ScriptManager.UpdateShop(shopText, editShopText);
            }
            else if (route == "view")
            {
                Debug.WriteLine("view");
                string editShopText = ViewEditShop();
                OutputStrings[0] = $"{TextColor}View {editShopText}";
            }
            

            // Route() will return a generic failure if value here is -1.
            return statusCode;
        }

        private string ViewEditShop()
        {
            (_, Dictionary<string, Dictionary<string, List<ShopItem>>> oldItems) = ReadShopItems();
            (_, string editShopText) = GenerateNewShop(oldItems);
            return editShopText;
        }

        private (string, string) UpdateText(string _message)
        {
            SettingsManager.UpdateShopMessage(_message);
            (_, Dictionary<string, Dictionary<string, List<ShopItem>>> oldItems) = ReadShopItems();
            (string shopText, string editShopText) = GenerateNewShop(oldItems);
            return (shopText, editShopText);
        }

        private (int, string, string) RemoveFromShop(int lowid, int highid, int ql)
        {
            (int itemCount, Dictionary<string, Dictionary<string, List<ShopItem>>> oldItems) = ReadShopItems();

            AOItem itemActual = GetItemByIDs(lowid);
            if (itemActual.name == null)
            {
                return (-1, "Item not inside database. Filled implants not supported", "");
            }
            ShopItem itemToRemove = new ShopItem
            {
                lowid = lowid,
                highid = highid,
                ql = ql,
                name = itemActual.name
            };

            string itemCategory = GetCategory(itemActual);
            for (int i=0; i<oldItems[itemCategory][itemToRemove.name].Count; i++)
            {
                if (oldItems[itemCategory][itemToRemove.name][i].ql == itemToRemove.ql)
                {
                    oldItems[itemCategory][itemToRemove.name].RemoveAt(i);
                    if (oldItems[itemCategory][itemToRemove.name].Count == 0)
                        oldItems[itemCategory].Remove(itemToRemove.name);
                    if (oldItems[itemCategory].Count == 0)
                        oldItems.Remove(itemCategory);
                    break;
                }
            }

            (string shopText, string editShopText) = GenerateNewShop(oldItems);
            return (1, shopText, editShopText);
        }

        private (int, string, string) AddToShop(int lowid, int highid, int ql) /* _params should be as follows | add category <a href.... |*/
        {
            (int itemCount, Dictionary<string, Dictionary<string, List<ShopItem>>> oldItems) = ReadShopItems();

            if (itemCount >= 15)
            {
                return (-1, "Maximum item count reached", "");
            }

            AOItem itemActual = GetItemByIDs(lowid);
            if (itemActual.name == null)
            {
                return (-1, "Item not inside database. Filled implants not supported", "");
            }
            ShopItem itemToAdd = new ShopItem
            {
                lowid = lowid,
                highid = highid,
                ql = ql,
                name = itemActual.name
            };

            string itemCategory = GetCategory(itemActual);

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

            (string shopText, string editShopText) = GenerateNewShop(oldItems);
            return (1, shopText, editShopText);
        }

        private (int, Dictionary<string, Dictionary<string, List<ShopItem>>>) ReadShopItems() 
        {
            Dictionary<string, Dictionary<string, List<ShopItem>>> oldItems = new Dictionary<string, Dictionary<string, List<ShopItem>>>();

            int itemCount = 0;
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
                            int itemEndIndex = textToRead.IndexOf("</a>") + 4;
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
                            itemCount++;
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
            return (itemCount, oldItems);
        }

        private string GetCategory(AOItem _item)
        {
            // We Can determine the category of the item to some degree. item.c 1 == Weapon | item.c 2 == Armor. We can parse Nano Crystal to check for nanos and Symbiant to check for symb(3rd word is always Symbiant)
            // This would leave us with Weapons|Armor|Nanos|Symbiants|Other as categories. The user never needs to input the category this way.
            // For each shop item added, lookup the low and high id paired with ql in itemDB 

            Dictionary<int, string> categories = new Dictionary<int, string>
            {
                [0] = "Other",
                [1] = "Weapons",
                [2] = "Armor",
                [3] = "Implants"
            };

            string itemCategory = "Other";

            if (_item.name.Length > 13)
            {
                if (_item.name[..12] == "Nano Crystal" || _item.name[..12] == "NanoCrystal ")
                {
                    itemCategory = "Nanos";
                }
                else if (_item.name.Contains(","))
                {
                    if (_item.name.Substring(_item.name.IndexOf(",") - 8, 8) == "Symbiant")
                    {
                        itemCategory = "Symbiants";
                    }
                }
                else if (categories.ContainsKey(_item.type))
                {
                    itemCategory = categories[_item.type];
                }
            }
            else if (categories.ContainsKey(_item.type))
            {
                itemCategory = categories[_item.type];
            }
            return itemCategory;
        }

        private (string, string) GenerateNewShop(Dictionary<string, Dictionary<string, List<ShopItem>>> _oldItems)
        {
            string foreText = SettingsManager.GetShopMessage();
            Debug.WriteLine(foreText);
            string shopText = $"{foreText} <a href=\"text://";
            string editShopText = "<a href=\"text://";
            if (_oldItems.ContainsKey("Weapons"))
            {
                shopText += $"<br>|Weapons|";
                editShopText += $"<br>|Weapons|";
                foreach ((string key, List<ShopItem> value) in _oldItems["Weapons"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}] ";
                        editShopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $" | {RedColor}[<a href='chatcmd:///editshop remove {item.lowid} {item.highid} {item.ql}'>X</a>]{EndColor}";
                    }
                }
                shopText += "<br>";
                editShopText += "<br>";
            }
            if (_oldItems.ContainsKey("Armor"))
            {
                shopText += "<br>|Armor|";
                editShopText += "<br>|Armor|";
                foreach ((string key, List<ShopItem> value) in _oldItems["Armor"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $" | {RedColor}[<a href='chatcmd:///editshop remove {item.lowid} {item.highid} {item.ql}'>X</a>]{EndColor}";
                    }
                }
                shopText += "<br>";
                editShopText += "<br>";
            }
            if (_oldItems.ContainsKey("Nanos"))
            {
                shopText += "<br>|Nanos|";
                editShopText += "<br>|Nanos|";
                foreach ((string key, List<ShopItem> value) in _oldItems["Nanos"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $" | {RedColor}[<a href='chatcmd:///editshop remove {item.lowid} {item.highid} {item.ql}'>X</a>]{EndColor}";
                    }
                }
                shopText += "<br>";
                editShopText += "<br>";
            }
            if (_oldItems.ContainsKey("Symbiants"))
            {
                shopText += "<br>|Symbiants|";
                editShopText += "<br>|Symbiants|";
                foreach ((string key, List<ShopItem> value) in _oldItems["Symbiants"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $" | {RedColor}[<a href='chatcmd:///editshop remove {item.lowid} {item.highid} {item.ql}'>X</a>]{EndColor}";
                    }
                }
                shopText += "<br>";
                editShopText += "<br>";
            }
            if (_oldItems.ContainsKey("Implants"))
            {
                shopText += "<br>|Implants|";
                editShopText += "<br>|Implants|";
                foreach ((string key, List<ShopItem> value) in _oldItems["Implants"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $" | {RedColor}[<a href='chatcmd:///editshop remove {item.lowid} {item.highid} {item.ql}'>X</a>]{EndColor}";
                    }
                }
                shopText += "<br>";
                editShopText += "<br>";
            }
            if (_oldItems.ContainsKey("Other"))
            {
                shopText += "<br>|Other|";
                editShopText += "<br>|Other|";
                foreach ((string key, List<ShopItem> value) in _oldItems["Other"])
                {
                    foreach (var item in value)
                    {
                        shopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $"<br>     -{BuildItemRef(item.lowid, item.highid, item.ql, item.name)} [{ValueColor}{item.ql}{EndColor}]";
                        editShopText += $" | {RedColor}[<a href='chatcmd:///editshop remove {item.lowid} {item.highid} {item.ql}'>X</a>]{EndColor}";
                    }
                }
                shopText += "<br>";
                editShopText += "<br>";
            }
            shopText += "<br><br><div align=right>Generated with <a href='chatcmd:///start https://github.com/Boxcar87/PRKHelper'>PRKHelper</a>\">Shop</a>";
            editShopText += "<br><br><div align=right>Generated with PRKHelper\">Shop</a>";
            return (shopText, editShopText);
        }

        private ShopItem ParseItem(string _itemString)
        {
            Debug.WriteLine(_itemString);
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

        static AOItem GetItemByIDs(int _lowid)
        {
            string query = "";
            query = $"SELECT * FROM Items WHERE lowid == {_lowid}";
            
            return DB.QueryItemByIDs(query);
        }
    }
}
