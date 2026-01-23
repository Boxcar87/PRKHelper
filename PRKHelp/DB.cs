using Microsoft.Data.Sqlite;
using PRKHelp.Components;

namespace PRKHelp
{
    public class DB
    {
        static SqliteConnectionStringBuilder ConnectionString { get; } = new()
        {
            DataSource = "PRKDB",
            Mode = SqliteOpenMode.Memory,
            Cache = SqliteCacheMode.Shared
        };

        static SqliteConnection Connection = new(ConnectionString.ToString());

        internal DB()
        {
            Connection.Open();
        }

        public static void InsertSQLFile(string _path)
        {
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = File.ReadAllText(_path);

                try
                {
                    command.ExecuteNonQuery();
                    //Debug.WriteLine("Database populated successfully from SQL file.");
                }
                catch (SqliteException ex)
                {
                    //Debug.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static List<AOItem> QueryItem(string _query, string _name)
        {
            List<AOItem> items = [];
            using (var command = new SqliteCommand(_query, Connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AOItem item = new()
                        {
                            lowid = reader.GetInt32(reader.GetOrdinal("lowid")),
                            highid = reader.GetInt32(reader.GetOrdinal("highid")),
                            lowql = reader.GetInt32(reader.GetOrdinal("lowql")),
                            highql = reader.GetInt32(reader.GetOrdinal("highql")),
                            name = reader.GetString(reader.GetOrdinal("name")),
                            icon = reader.GetInt32(reader.GetOrdinal("icon")),
                        };
                        if (item.name.Equals(_name, StringComparison.OrdinalIgnoreCase))
                            items.Insert(0, item);
                        else
                            items.Add(item);
                        item.name = item.name.Replace("\"", "\\\"");
                    }
                }
            }
            return items;
        }

        public static Dictionary<string, Dictionary<string, float>> QueryTrickle(string _query)
        {
            Dictionary<string, Dictionary<string, float>> stats = new()
            {
                {"Body & Defense", new Dictionary<string, float>()},
                {"Combat & Healing", new Dictionary<string, float>()},
                {"Melee Weapons", new Dictionary<string, float>()},
                {"Melee Specials", new Dictionary<string, float>()},
                {"Ranged Weapons", new Dictionary<string, float>()},
                {"Ranged Specials", new Dictionary<string, float>()},
                {"Nanos & Casting", new Dictionary<string, float>()},
                {"Trade & Repair", new Dictionary<string, float>()}
            };

            using (var command = new SqliteCommand(_query, Connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string group = reader.GetString(reader.GetOrdinal("group_name"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        float value = reader.GetFloat(reader.GetOrdinal("amount"));
                        stats[group].Add(name, value);
                    }
                }
            }
            return stats;
        }
    }
}
