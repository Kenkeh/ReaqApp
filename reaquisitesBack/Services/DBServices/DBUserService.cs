using reaquisites.Models;
using Npgsql;

namespace reaquisites.Services.DB
{
    public static class DBUserService
    {
        internal static string connString;
        public static List<User> GetAll(){
            List<User> allUsers = new List<User>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT * FROM reaquisites.\"Users\"";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return allUsers;
                        }
                        while (reader.Read())
                        {
                            allUsers.Add(new User
                            {
                                Nick = reader[1].ToString(),
                                Account = reader[2].ToString(),
                                EMail = reader[3].ToString(),
                                RegisterDate = Convert.ToDateTime(reader[4])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return allUsers;
        }

        public static List<KeyValuePair<User,int>> GetAllWithID(){
            List<KeyValuePair<User,int>> allUsers = new List<KeyValuePair<User,int>>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT * FROM reaquisites.\"Users\"";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return allUsers;
                        }
                        while (reader.Read())
                        {
                            allUsers.Add(
                                new KeyValuePair<User, int>(
                                    new User
                                    {
                                        Nick = reader[1].ToString(),
                                        Account = reader[2].ToString(),
                                        EMail = reader[3].ToString(),
                                        RegisterDate = Convert.ToDateTime(reader[4])
                                    },
                                    Convert.ToInt32(reader[0])
                                )
                            );
                        }
                    }
                    con.Close();
                }
            }
            return allUsers;
        }

        public static int GetUserId(string account){
            int id = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT * FROM reaquisites.\"Users\" WHERE account_name = '"+account+"'";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return -1;
                        }
                        reader.Read();
                        id = Convert.ToInt32(reader[0]);
                    }
                    con.Close();
                }
            }
            return id;
        }

        public static string GetUserPass(int id){
            string pass = "";
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT security_key FROM reaquisites.\"SecurityLoggin\" WHERE user_id = "+id;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return pass;
                        }
                        reader.Read();
                        pass = reader[0].ToString();
                    }
                    con.Close();
                }
            }
            return pass;
        }

        public static void Add(User user, string password)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Users\" (nick_name, account_name, email, register_date) "+
                "VALUES ('"+user.Nick+"','"+user.Account+"','"+user.EMail+"','"+DateTime.Now+"');";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            int userID = GetUserId(user.Account);
            if (userID<0) return;

            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"SecurityLoggin\" (security_key, user_id) "+
                "VALUES ('"+password+"',"+userID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


    }
}