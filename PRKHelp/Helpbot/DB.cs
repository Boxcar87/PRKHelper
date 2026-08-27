using System.Diagnostics;
using Microsoft.Data.Sqlite;
using PRKHelper.Helpbot.Components;

namespace PRKHelper.Helpbot
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
                    Debug.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static List<AOItem> QueryItem(string _query, string _name)
        {
            List<AOItem> items = [];
            try
            {
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
                                type = reader.GetInt32(reader.GetOrdinal("class"))
                            };
                            if (item.name.Equals(_name, StringComparison.OrdinalIgnoreCase))
                                items.Insert(0, item);
                            else
                                items.Add(item);
                            item.name = item.name.Replace("\"", "\\\"");
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return items;
        }
        public static AOItem QueryItemByIDs(string _query)
        {
            AOItem item = new AOItem();
            try
            {
                using (var command = new SqliteCommand(_query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item.lowid = reader.GetInt32(reader.GetOrdinal("lowid"));
                            item.highid = reader.GetInt32(reader.GetOrdinal("highid"));
                            item.lowql = reader.GetInt32(reader.GetOrdinal("lowql"));
                            item.highql = reader.GetInt32(reader.GetOrdinal("highql"));
                            item.name = reader.GetString(reader.GetOrdinal("name"));
                            item.type = reader.GetInt32(reader.GetOrdinal("class"));
                            item.name = item.name.Replace("\"", "\\\"");
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return item;
        }

        public static int QueryItemValue(string _query)
        {
            int value = 0;
            try
            {
                using (var command = new SqliteCommand(_query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            value = reader.GetInt32(reader.GetOrdinal("value"));
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return value;
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

        public static List<(string, AOItem)> QuerySymbiant(string _query)
        {
            List<(string, AOItem)> symbiantsWithBosses = [];

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
                            name = reader.GetString(reader.GetOrdinal("name"))
                        };
                        item.name = item.name.Replace("\"", "\\\"");
                        string pocketBoss = reader.GetString(reader.GetOrdinal("pocketboss_name"));
                        symbiantsWithBosses.Add((pocketBoss, item));
                    }
                }
            }
            return symbiantsWithBosses;
        }

        public static List<PocketBoss> QueryPocketBoss(string _query)
        {
            List<PocketBoss> pocketBosses = [];
            try
            {
                using (var command = new SqliteCommand(_query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PocketBoss boss = new()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("id")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                playfield = reader.GetString(reader.GetOrdinal("long_name")),
                                mobType = reader.GetString(reader.GetOrdinal("mob_type")),
                                level = reader.GetInt32(reader.GetOrdinal("level")),
                                location = reader.GetString(reader.GetOrdinal("location"))
                            };
                            pocketBosses.Add(boss);
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return pocketBosses;
        }

        public static List<AOItem> QuerySymbiantsByPocketBoss(string _query)
        {
            List<AOItem> symbiants = [];
            try
            {
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
                                name = reader.GetString(reader.GetOrdinal("name")),
                            };
                            item.name = item.name.Replace("\"", "\\\"");
                            symbiants.Add(item);
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return symbiants;
        }

        public static LevelData QueryLevel(string _query)
        {
            List<LevelData> data = [];
            try
            {
                using (var command = new SqliteCommand(_query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LevelData level = new()
                            {
                                level = reader.GetInt16(reader.GetOrdinal("level")),
                                teamMin = reader.GetInt16(reader.GetOrdinal("team_min")),
                                teamMax = reader.GetInt16(reader.GetOrdinal("team_max")),
                                pvpMin = reader.GetInt16(reader.GetOrdinal("pvp_min")),
                                pvpMax = reader.GetInt16(reader.GetOrdinal("pvp_max")),
                                xpToLevel = reader.GetInt32(reader.GetOrdinal("xpsk")),
                                tokens = reader.GetInt16(reader.GetOrdinal("tokens")),
                                missions = reader.GetString(reader.GetOrdinal("missions")),
                            };
                            data.Add(level);
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return data[0];
        }
        public static List<short> QueryMissions(string _query)
        {
            List<short> missions = new();
            try
            {
                using (var command = new SqliteCommand(_query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            missions.Add(reader.GetInt16(reader.GetOrdinal("level")));
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return missions;
        }
        public static Weapon QueryWeaponStats(string _query)
        {
            Weapon weaponStats = new();
            try
            {
                using (var command = new SqliteCommand(_query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            weaponStats.min = reader.GetInt32(reader.GetOrdinal("min"));
                            weaponStats.max = reader.GetInt32(reader.GetOrdinal("max"));
                            weaponStats.crit = reader.GetInt32(reader.GetOrdinal("crit"));
                            weaponStats.attack = reader.GetInt32(reader.GetOrdinal("attack"));
                            weaponStats.recharge = reader.GetInt32(reader.GetOrdinal("recharge"));
                            weaponStats.arCap = reader.GetInt32(reader.GetOrdinal("arcap"));
                            weaponStats.flingShot = reader.GetInt32(reader.GetOrdinal("flingshot"));
                            weaponStats.burst = reader.GetInt32(reader.GetOrdinal("burst"));
                            weaponStats.fullAuto = reader.GetInt32(reader.GetOrdinal("fullauto"));
                            weaponStats.fastAttack = reader.GetInt32(reader.GetOrdinal("fastattack"));
                            weaponStats.brawl = reader.GetInt32(reader.GetOrdinal("brawl"));
                        }
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }            
            return weaponStats;
        }

        public static bool QueryIfWeaponExists(string _query)
        {
            bool weaponExists = false;
            try
            {
                using (var command = new SqliteCommand(_query, Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            weaponExists = true;
                    }
                }
            }
            catch (SqliteException _ex)
            {
                Debug.WriteLine(_ex);
            }
            return weaponExists;
        }
    }
}
