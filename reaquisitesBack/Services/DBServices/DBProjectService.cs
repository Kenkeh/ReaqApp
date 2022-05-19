using reaquisites.Models;
using Npgsql;

namespace reaquisites.Services.DB
{
    public static class DBProjectService
    {
        internal static string connString;
        
        public static int Add(string userAccount, Project project)
        {
            int userID = DBUserService.GetUserId(userAccount);
            if (userID<0) return -1;

            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Projects\" (name, desc, published, template, user) "+
                "VALUES ('"+project.Name+"','"+project.Description+"',false, false, "+userID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return 0;
        }

        public static List<SimpleProjectDTO> GetUserProjectsSimpleDTO(int userID){
            List<SimpleProjectDTO> userProjects = new List<SimpleProjectDTO>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string projectsQuery = "SELECT id, name, desc, version, published FROM reaquisites.\"Users\" where user_id = "+userID;
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return userProjects;
                        }
                        while (projectsReader.Read())
                        {
                            SimpleProjectDTO project = new SimpleProjectDTO();
                            project.Name = projectsReader[1].ToString();
                            project.Description = projectsReader[2].ToString();
                            project.Version = projectsReader[3].ToString();
                            project.IsPublished = (bool) projectsReader[4];
                            project.LastModified = GetLastModificationDate((int)projectsReader[0]);
                            userProjects.Add(project);
                        }
                    }
                    con.Close();
                }
            }
            return userProjects;
        }

        private static DateTime GetLastModificationDate(int projectID){
            string dateString="";
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string historyQuery = "SELECT date FROM reaquisites.\"HistoryEntries\" where project = "+projectID+" order by date desc limit 1";
                using (NpgsqlCommand historyCmd = new NpgsqlCommand(historyQuery))
                {
                    historyCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader historyReader = historyCmd.ExecuteReader())
                    {
                        while (historyReader.Read())
                        {
                            dateString = historyReader[0].ToString();
                        }
                    }
                    con.Close();
                }
            }
            return DateTime.Parse(dateString);
        }
    }

}