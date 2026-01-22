namespace PRKHelp.Components
{
    public class AOItem
    {
        internal int lowid { get; set; }
        internal int highid { get; set; }
        internal int lowql { get; set; }
        internal int highql { get; set; }
        internal string name { get; set; }
        internal int icon { get; set; }
    }

    public class Item : Component
    {
        DB DB;
        public Item(DB _db)
        {
            LoadItems();
            DB = _db;
        }

        // First input for Items can be int or string and spaces in names count as separate params
        // Returning true cause Process handles orienting the parameters
        public override (int, string) ValidateParams(string[] _params)
        {
            ParamSyntax = $"/item 100 item name";
            return (1, "true");
        }
        public override int Process(string[] _params)
        {
            
            string name = string.Join(" ", _params);
            name = name.Trim(' ');
            int ql = 0;
            if (int.TryParse(_params[0], out ql))
                name = string.Join(" ", _params[1..]);
            name = name.Replace("'", "''");

            List<AOItem> items = GetItemsByName(name, ql);
            if (items.Count == 0)
            {
                OutputStrings[0] = $"/text No matching items. Inputs should match {ParamSyntax}";
                return 1;
            }
            int page = 0;
            int pageFirstItem = 1;
            int pageItem = 0;
            OutputStrings[page] = $"/text <a href=\"text://{HighlightColor}Item Search Results{EndColor}<br><br>";
            foreach (AOItem item in items)
            {
                int localQl = ql;
                if (ql < 1)
                    localQl = item.lowql;

                string cleanName = item.name.Replace("'", "");
                cleanName = cleanName.Replace("\"", "");

                pageItem++;

                OutputStrings[page] += $"<img src=rdb://{item.icon}><br>";
                OutputStrings[page] += $"{item.name} {BuildItemRef(item.lowid, item.highid, item.lowql, item.lowql.ToString())} [{BuildItemRef(item.lowid, item.highid, item.lowql, item.lowql.ToString())} - {BuildItemRef(item.lowid, item.highid, item.highql, item.highql.ToString())}]<br>";
                OutputStrings[page] += $"<a href='chatcmd:///PRKItemlink {item.lowid} {item.highid} {localQl} {cleanName}'>Link to chat - QL {localQl}</a><br><br>";
                
                if (OutputStrings[page].Length > 3500)
                {
                    OutputStrings[page] += $"\">Item Search Results ({pageFirstItem} - {pageItem} of {items.Count})</a>";
                    OutputStrings.Add($"/text <a href=\"text://{HighlightColor}Item Search Results{EndColor}<br><br>");
                    page++;
                    pageFirstItem = pageItem + 1;
                }
            }
            OutputStrings[page] += $"\">Item Search Results ({pageFirstItem} - {pageItem} of {items.Count})</a>";

            return 1;
        }

        static void LoadItems()
        {
            DB.InsertSQLFile(Path.GetDirectoryName(Application.ExecutablePath)+"\\SQL\\Items.sql");
        }

        static List<AOItem> GetItemsByName(string _name, int _ql)
        {

            string query = $"SELECT * FROM Items WHERE name LIKE '%{_name}%'";
            if (_ql > 0)
                query += $" AND lowql <= {_ql} AND highql >= {_ql}";

            query += " ORDER BY name ASC, highql DESC";

            return DB.QueryItem(query, _name);
        }
    }
}
