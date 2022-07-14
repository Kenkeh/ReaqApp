using reaquisites.Models;
using Npgsql;

namespace reaquisites.Services.DB
{
    public static class DBProjectService
    {
        internal static string connString;
        
        
        

        static internal List<SimpleProjectDTO> GetUserProjectsSimpleDTO(int userID){
            List<SimpleProjectDTO> userProjects = new List<SimpleProjectDTO>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, version, published, ref FROM reaquisites.\"Projects\" where user_id = "+userID+" and template = false";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return userProjects;
                        }
                        while (reader.Read())
                        {
                            SimpleProjectDTO project = new SimpleProjectDTO();
                            project.Name = reader[1].ToString();
                            project.Description = reader[2].ToString();
                            project.Version = reader[3].ToString();
                            project.IsPublished = (bool) reader[4];
                            project.LastModified = GetLastModificationDate((int)reader[0]);
                            project.Id = (int)reader[5];
                            userProjects.Add(project);
                        }
                    }
                    con.Close();
                }
            }
            return userProjects;
        }

        static internal DateTime GetLastModificationDate(int projectID){
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

        // ID GETTERS
        static internal int GetProjectID(int userID, int projectId){
            int projectID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Projects\" where user_id = "+userID+" AND ref = "+projectId;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectID;
                        }
                        while (reader.Read())
                        {
                            projectID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return projectID;
        }
        static internal int GetProjectRef(int userID, int projectId){
            int projectID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT ref FROM reaquisites.\"Projects\" where user_id = "+userID+" AND id = "+projectId;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectID;
                        }
                        while (reader.Read())
                        {
                            projectID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return projectID;
        }
        static internal int GetLastProjectID(int userID){
            int projectID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Projects\" where user_id = "+userID+" order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectID;
                        }
                        while (reader.Read())
                        {
                            projectID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return projectID;
        }

        static internal int GetArtefactDefID(int projectID, int artefactDefRef){
            int artDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactDefs\" where project = "+projectID+" AND ref = "+artefactDefRef;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artDefID;
                        }
                        while (reader.Read())
                        {
                            artDefID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artDefID;
        }
        static internal int GetLastArtefactDefID(int projectID){
            int artDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactDefs\" where project = "+projectID+" order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artDefID;
                        }
                        while (reader.Read())
                        {
                            artDefID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artDefID;
        }
        static internal int GetRelationshipDefID(int projectID, int relationshipDefId){
            int artDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipDefs\" where project = "+projectID+" AND ref = "+relationshipDefId;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artDefID;
                        }
                        while (reader.Read())
                        {
                            artDefID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artDefID;
        }
        static internal int GetLastRelationshipDefID(int projectID){
            int relDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipDefs\" where project = "+projectID+" order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return relDefID;
                        }
                        while (reader.Read())
                        {
                            relDefID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return relDefID;
        }

        static internal int GetArtefactAttributeDefID(int artDefID, string artAttribName){
            int artAttribDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactAttributeDefs\" where artefactdef = "+artDefID+" AND name = '"+artAttribName+"'";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artAttribDefID;
                        }
                        while (reader.Read())
                        {
                            artAttribDefID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artAttribDefID;
        }
        static internal int GetRelationshipAttributeDefID(int relDefID, string artAttribName){
            int relAttribDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipAttributeDefs\" where relationshipdef = "+relDefID+" AND name = '"+artAttribName+"'";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return relAttribDefID;
                        }
                        while (reader.Read())
                        {
                            relAttribDefID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return relAttribDefID;
        }
        
        static internal int GetArtefactID(int artDefID, int artRef){
            int artID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Artefacts\" where artefactdef = "+artDefID+" AND ref = "+artRef;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artID;
                        }
                        while (reader.Read())
                        {
                            artID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artID;
        }
        static internal int GetLastArtefactID(int artDefID){
            int artID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Artefacts\" where artefactdef = "+artDefID+" order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artID;
                        }
                        while (reader.Read())
                        {
                            artID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artID;
        }
        static internal int GetArtefactAttributeID(int artID, string artAttribDefID){
            int artAttribID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactAttributes\" where artefact = "+artID+" AND artefactattributedef = "+artAttribDefID+"";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artAttribID;
                        }
                        while (reader.Read())
                        {
                            artAttribID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artAttribID;
        }
        static internal int GetRelationshipID(int relDefID, int relId){
            int relID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Relationships\" where relationshipdef = "+relDefID+" AND ref = "+relId;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return relID;
                        }
                        while (reader.Read())
                        {
                            relID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return relID;
        }
        static internal int GetLastRelationshipID(int relDefID){
            int relID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Relationships\" where relationshipdef = "+relDefID+" order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return relID;
                        }
                        while (reader.Read())
                        {
                            relID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return relID;
        }
        static internal int GetRelationshipAttributeID(int relID, string relAttribDefID){
            int artAttribID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipAttributes\" where relationship = "+relID+" AND relationshipattributedef = "+relAttribDefID+"";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artAttribID;
                        }
                        while (reader.Read())
                        {
                            artAttribID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artAttribID;
        }

        static internal int GetVisualizationID(int projectID, int visualRefID){
            int visualizationID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"VisualizationTemplates\" where project = "+projectID+" AND ref = "+visualRefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return visualizationID;
                        }
                        while (reader.Read())
                        {
                            visualizationID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return visualizationID;
        }
        static internal int GetLastVisualizationID(int projectID){
            int visualID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"VisualizationTemplates\" where project = "+projectID+" order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return visualID;
                        }
                        while (reader.Read())
                        {
                            visualID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return visualID;
        }
        static internal int GetArtefactColorFactorID(int visualizationID, int attribDefID){
            int artColorFactorID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactColorFactors\" where artefactattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artColorFactorID;
                        }
                        while (reader.Read())
                        {
                            artColorFactorID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artColorFactorID;
        }
        static internal int GetLastArtefactColorFactorID(int attribDefID, int visualID){
            int id = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactColorFactors\" where artefactattributedef = "
                +attribDefID+" and visualizationtemplate = "+visualID+ " order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return id;
                        }
                        while (reader.Read())
                        {
                            id = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return id;
        }

        static internal int GetRelationshipColorFactorID(int visualizationID, int attribDefID){
            int relColorFactorID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipColorFactors\" where relationshipattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return relColorFactorID;
                        }
                        while (reader.Read())
                        {
                            relColorFactorID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return relColorFactorID;
        }
        static internal int GetLastRelationshipColorFactorID(int attribDefID, int visualID){
            int id = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipColorFactors\" where relationshipattributedef = "
                +attribDefID+" and visualizationtemplate = "+visualID+ " order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return id;
                        }
                        while (reader.Read())
                        {
                            id = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return id;
        }

        static internal int GetArtefactSizeFactorID(int visualizationID, int attribDefID){
            int artSizeFactorID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactSizeFactors\" where artefactattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artSizeFactorID;
                        }
                        while (reader.Read())
                        {
                            artSizeFactorID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return artSizeFactorID;
        }
        static internal int GetLastArtefactSizeFactorID(int attribDefID, int visualID){
            int id = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactSizeFactors\" where artefactattributedef = "
                +attribDefID+" and visualizationtemplate = "+visualID+ " order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return id;
                        }
                        while (reader.Read())
                        {
                            id = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return id;
        }

        static internal int GetRelationshipSizeFactorID(int visualizationID, int attribDefID){
            int relSizeFactorID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipSizeFactors\" where relationshipattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return relSizeFactorID;
                        }
                        while (reader.Read())
                        {
                            relSizeFactorID = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return relSizeFactorID;
        }
        static internal int GetLastRelationshipSizeFactorID(int attribDefID, int visualID){
            int id = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipSizeFactors\" where relationshipattributedef = "
                +attribDefID+" and visualizationtemplate = "+visualID+ " order by ref desc limit 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return id;
                        }
                        while (reader.Read())
                        {
                            id = (int)reader[0];
                        }
                    }
                    con.Close();
                }
            }
            return id;
        }


        //ADD ELEMENTS
        static internal void AddProject(int userID, string projectName, string projectDescription, bool projectIsPublished, bool projectIsTemplate, string version){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Projects\" (name, description, published, template, version, user_id, ref) "+
                "VALUES ('"+projectName+"','"+projectDescription+"',"+projectIsPublished+", "+projectIsTemplate+", "+version+", "+userID+","+
                "(SELECT CASE WHEN COUNT(1) > 0 THEN max(ref)+1 ELSE 0 END FROM reaquisites.\"Projects\" where user_id="+userID+"))";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactDefinition(int projectID, string artDefName, string? artDefDescription, int artDefShape){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactDefs\" (project, name, description, shape, ref) "+
                "VALUES ("+projectID+", '"+artDefName+"', '"+artDefDescription+"', "+artDefShape+", "+
                "(SELECT CASE WHEN COUNT(1) > 0 THEN max(ref)+1 ELSE 0 END FROM reaquisites.\"ArtefactDefs\" where project="+projectID+"))";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactDefinition(int projectID, string artDefName, string? artDefDescription, int artDefShape, int refId){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactDefs\" (project, name, description, shape, ref) "+
                "VALUES ("+projectID+", '"+artDefName+"', '"+artDefDescription+"', "+artDefShape+", "+refId+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactAttributeDefinition(string artAttribDefName, int type, string artAttribDefDescription, string artAttribDefValues, int artDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactAttributeDefs\" (name, type, description, values, artefactdef) "+
                "VALUES ('"+artAttribDefName+"', "+type+", '"+artAttribDefDescription+"', '"+artAttribDefValues+"', "+artDefID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipDefinition(int projectID, string relDefName, string? relDefDescription, int relDefShape){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipDefs\" (project, name, description, shape, ref) "+
                "VALUES ("+projectID+", '"+relDefName+"', '"+relDefDescription+"', "+relDefShape+", "+
                "(SELECT CASE WHEN COUNT(1) > 0 THEN max(ref)+1 ELSE 0 END FROM reaquisites.\"RelationshipDefs\" where project="+projectID+"))";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipDefinition(int projectID, string relDefName, string? relDefDescription, int relDefShape, int refId){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipDefs\" (project, name, description, shape, ref) "+
                "VALUES ("+projectID+", '"+relDefName+"', '"+relDefDescription+"', "+relDefShape+", "+refId+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipAttributeDefinition(string relAttribDefName, int type, string relAttribDefDescription, string relAttribDefValues, int relDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipAttributeDefs\" (name, type, description, values, relationshipdef) "+
                "VALUES ('"+relAttribDefName+"', "+type+", '"+relAttribDefDescription+"', '"+relAttribDefValues+"', "+relDefID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefact(string artName, string? artDesc, int artDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Artefacts\" (name, artefactdef, description, ref) "+
                "VALUES ('"+artName+"', "+artDefID+", '"+artDesc+"', "+
                "(SELECT CASE WHEN COUNT(1) > 0 THEN max(ref)+1 ELSE 0 END FROM reaquisites.\"Artefacts\" where artefactdef="+artDefID+"))";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefact(string artName, string? artDesc, int artDefID, int refId){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Artefacts\" (name, artefactdef, description, ref) "+
                "VALUES ('"+artName+"', "+artDefID+", '"+artDesc+"', "+refId+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactAttribute(int artID, int attributeDefID, string value){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactAttributes\" (artefact, artefactattributedef, value) "+
                "VALUES ("+artID+", "+attributeDefID+",'"+value+"')";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationship(int relDefID, string? relDesc, int parentID, int childID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Relationships\" (parent, child, relationshipdef, description, ref) "+
                "VALUES ("+parentID+", "+childID+", "+relDefID+", '"+relDesc+"', "+
                "(SELECT CASE WHEN COUNT(1) > 0 THEN max(ref)+1 ELSE 0 END FROM reaquisites.\"Relationships\" where relationshipdef="+relDefID+"))";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationship(int relDefID, string? relDesc, int parentID, int childID, int refId){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Relationships\" (parent, child, relationshipdef, description, ref) "+
                "VALUES ("+parentID+", "+childID+", "+relDefID+", '"+relDesc+"', "+refId+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipAttribute(int relID, int attributeDefID, string value){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipAttributes\" (relationshipattributedef, relationship, value) "+
                "VALUES ("+attributeDefID+", "+relID+",'"+value+"')";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddVisualizationTemplate(string visualName, string visualDescription, int projectID, int refID, int graph){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"VisualizationTemplates\" (name, description, project, ref, graphTemplate) "+
                "VALUES ( '"+visualName+"', '"+visualDescription+"', "+projectID+", "+refID+", "+graph+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactColorFactor(int attribDefID, int visualID, float weight, bool interpolate){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactColorFactors\" (artefactattributedef, visualizationtemplate, weight, interpolate) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+", "+interpolate+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactColorFactorValue(int colorFactorID, string defvalue, byte R, byte G, byte B, byte A){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactColorFactorValues\" (artefactcolorfactor, defvalue, r, g, b, a) "+
                "VALUES ( "+colorFactorID+", '"+defvalue+"', "+R+", "+G+", "+B+", "+A+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipColorFactor(int attribDefID, int visualID, float weight, bool interpolate){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipColorFactors\" (relationshipattributedef, visualizationtemplate, weight, interpolate) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+", "+interpolate+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipColorFactorValue(int colorFactorID, string defvalue, byte R, byte G, byte B, byte A){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipColorFactorValues\" (relationshipcolorfactor, defvalue, r, g, b, a) "+
                "VALUES ( "+colorFactorID+", '"+defvalue+"', "+R+", "+G+", "+B+", "+A+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactSizeFactor(int attribDefID, int visualID, float weight, bool interpolate){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactSizeFactors\" (artefactattributedef, visualizationtemplate, weight, interpolate) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+", "+interpolate+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactSizeFactorValue(int sizeFactorID, string defvalue, float value){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactSizeFactorValues\" (artefactcolorfactor, defvalue, value) "+
                "VALUES ( "+sizeFactorID+", '"+defvalue+"', "+value+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipSizeFactor(int attribDefID, int visualID, float weight, bool interpolate){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipSizeFactors\" (relationshipattributedef, visualizationtemplate, weight, interpolate) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+", "+interpolate+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipSizeFactorValue(int sizeFactorID, string defvalue, float value){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipSizeFactorValues\" (relationshipcolorfactor, defvalue, value) "+
                "VALUES ( "+sizeFactorID+", '"+defvalue+"', "+value+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddProjectHistoryEntry(int projectID, int elementType, int element, int type, string changes, DateTime date){

            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"HistoryEntries\" (element_type, type, element, changes, project, date) "+
                "VALUES ( "+elementType+", "+type+", "+element+", '"+changes+"', "+projectID+", '"+date+"')";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }



        //ELEMENT GETTERS
        static internal List<HistoryEntry> GetAllProjectHistoryEntries(int projectID){
            List<HistoryEntry> HEs = new List<HistoryEntry>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT type, changes, date, element_type, element FROM reaquisites.\"HistoryEntries\" where project = "+projectID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return HEs;
                        }
                        while (reader.Read())
                        {
                            HistoryEntry he = new HistoryEntry();
                            he.ChangeType = (int)reader[0];
                            he.Changes = reader[1].ToString();
                            he.ChangeDate = DateTime.Parse(reader[2].ToString());
                            he.ElementType = (int)reader[3];
                            he.ElementId = (int)reader[4];
                            HEs.Add(he);
                        }
                    }
                    con.Close();
                }
            }
            return HEs;
        }
        static internal (int, Project) GetUserProject(int userID, int projectId){
            (int, Project) userProject = (-1,new Project());
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, published, version, ref FROM reaquisites.\"Projects\" where user_id = "+userID+" AND ref = "+projectId;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return userProject;
                        }
                        while (reader.Read())
                        {
                            userProject.Item1 = (int)reader[0];
                            userProject.Item2.Name = reader[1].ToString();
                            userProject.Item2.Description = reader[2].ToString();
                            userProject.Item2.IsPublished = (bool)reader[3];
                            userProject.Item2.Version = reader[4].ToString();
                            userProject.Item2.ProjectId = (int)reader[5];
                            userProject.Item2.ProjectId = projectId;
                        }
                    }
                    con.Close();
                }
            }
            return userProject;
        }
        static internal List<(int, ArtefactDefinition)> GetProjectArtefactDefs(int projectID){
            List<(int, ArtefactDefinition)> projectArtDefs = new List<(int, ArtefactDefinition)>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, shape, ref FROM reaquisites.\"ArtefactDefs\" where project = "+projectID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectArtDefs;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            ArtefactDefinition artDef = new ArtefactDefinition();
                            artDef.Name = reader[1].ToString();
                            artDef.Description = reader[2] !=null ? reader[2].ToString() : null;
                            artDef.Shape = (int)reader[3];
                            artDef.ID = (int)reader[4];
                            projectArtDefs.Add((id, artDef));
                        }
                    }
                    con.Close();
                }
            }
            return projectArtDefs;
        }
        static internal Dictionary<int, AttributeDefinition> GetArtefactDefAttributeDefs(int artDefID){
            Dictionary<int, AttributeDefinition> projectArtAttribDefs = new Dictionary<int, AttributeDefinition>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, type, values FROM reaquisites.\"ArtefactAttributeDefs\" where artefactdef = "+artDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectArtAttribDefs;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            AttributeDefinition artAttribDef = new AttributeDefinition();
                            artAttribDef.Name = reader[1].ToString();
                            artAttribDef.Description = reader[2].ToString();
                            artAttribDef.Type = (int)reader[3];
                            artAttribDef.Values = reader[4].ToString();
                            projectArtAttribDefs.Add(id, artAttribDef);
                        }
                    }
                    con.Close();
                }
            }
            return projectArtAttribDefs;
        }
        static internal List<(int, RelationshipDefinition)> GetProjectRelationshipDefs(int projectID){
            List<(int, RelationshipDefinition)> projectRelDefs = new List<(int, RelationshipDefinition)>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, shape, ref FROM reaquisites.\"RelationshipDefs\" where project = "+projectID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectRelDefs;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            RelationshipDefinition relDef = new RelationshipDefinition();
                            relDef.Name = reader[1].ToString();
                            relDef.Description = reader[2] !=null ? reader[2].ToString() : null;
                            relDef.Shape = (int)reader[3];
                            relDef.ID = (int)reader[4];
                            projectRelDefs.Add((id, relDef));
                        }
                    }
                    con.Close();
                }
            }
            return projectRelDefs;
        }
        static internal Dictionary<int, AttributeDefinition> GetRelationshipDefAttributeDefs(int relDefID){
            Dictionary<int, AttributeDefinition> projectRelAttribDefs = new Dictionary<int, AttributeDefinition>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, type, values FROM reaquisites.\"RelationshipAttributeDefs\" where relationshipdef = "+relDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectRelAttribDefs;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            AttributeDefinition relAttribDef = new AttributeDefinition();
                            relAttribDef.Name = reader[1].ToString();
                            relAttribDef.Description = reader[2].ToString();
                            relAttribDef.Type = (int)reader[3];
                            relAttribDef.Values = reader[4].ToString();
                            projectRelAttribDefs.Add(id, relAttribDef);
                        }
                    }
                    con.Close();
                }
            }
            return projectRelAttribDefs;
        }
        static internal List<(int, Artefact)> GetArtefactsForDefinition(int artDefID){
            List<(int, Artefact)> projectArtefacts = new List<(int, Artefact)>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, ref FROM reaquisites.\"Artefacts\" where artefactdef = "+artDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectArtefacts;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            Artefact artefact = new Artefact();
                            artefact.Name = reader[1].ToString();
                            artefact.Description = reader[2] !=null ? reader[2].ToString() : null;
                            artefact.ID = (int)reader[3];
                            projectArtefacts.Add((id, artefact));
                        }
                    }
                    con.Close();
                }
            }
            return projectArtefacts;
        }
        static internal List<(Attribute, int)> GetArtefactAttributes(int artefactID){
            List<(Attribute, int)> artefactAttribs = new List<(Attribute, int)>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT artefactattributedef, value FROM reaquisites.\"ArtefactAttributes\" where artefact = "+artefactID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artefactAttribs;
                        }
                        while (reader.Read())
                        {
                            int attribDefID = (int)reader[0];
                            Attribute artefact = new Attribute();
                            artefact.Value = reader[1].ToString();
                            artefactAttribs.Add((artefact,attribDefID));
                        }
                    }
                    con.Close();
                }
            }
            return artefactAttribs;
        }
        static internal List<(int, (int,int,string?,int))> GetRelationshipsForDefinition(int relDefID){
            List<(int, (int,int,string?,int))> relationshipsForDef = new List<(int, (int,int,string?,int))>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, parent, child, description, ref FROM reaquisites.\"Relationships\" where relationshipdef = "+relDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return relationshipsForDef;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            int parentID = (int)reader[1];
                            int childID = (int)reader[2];
                            string? description = reader[3] !=null ? reader[3].ToString() : null;
                            int relID = (int)reader[4];
                            relationshipsForDef.Add((id, (parentID,childID,description,relID)));
                        }
                    }
                    con.Close();
                }
            }
            return relationshipsForDef;
        }
        static internal List<(Attribute, int)> GetRelationshipAttributes(int relationshipID){
            List<(Attribute, int)> artefactAttribs = new List<(Attribute, int)>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT relationshipattributedef, value FROM reaquisites.\"RelationshipAttributes\" where relationship = "+relationshipID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return artefactAttribs;
                        }
                        while (reader.Read())
                        {
                            int attribDefID = (int)reader[0];
                            Attribute artefact = new Attribute();
                            artefact.Value = reader[1].ToString();
                            artefactAttribs.Add((artefact,attribDefID));
                        }
                    }
                    con.Close();
                }
            }
            return artefactAttribs;
        }
        static internal List<(int, Visualization)> GetProjectVisualizations(int projectID){
            List<(int, Visualization)> projectVisuals = new List<(int, Visualization)>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, graphtemplate, ref FROM reaquisites.\"VisualizationTemplates\" where project = "+projectID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return projectVisuals;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            Visualization visual = new Visualization();
                            visual.Name = reader[1].ToString();
                            visual.Description = reader[2].ToString();
                            visual.GraphTemplate = (int)reader[3];
                            visual.ID = (int) reader[4];
                            projectVisuals.Add((id, visual));
                        }
                    }
                    con.Close();
                }
            }
            return projectVisuals;
        }
        static internal List<(int, (ArtefactColorFactor,int))> GetArtColorFactorsForVisual(int visualID){
            List<(int, (ArtefactColorFactor,int))> visualColorFactors = new List<(int, (ArtefactColorFactor,int))>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, interpolate, weight, artefactattributedef FROM reaquisites.\"ArtefactColorFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return visualColorFactors;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            ArtefactColorFactor colorFactor = new ArtefactColorFactor();
                            colorFactor.Interpolated = (bool)reader[1];
                            colorFactor.Weight = (float)reader[2];
                            int attribID = (int)reader[3];
                            visualColorFactors.Add((id, (colorFactor,attribID)));
                        }
                    }
                    con.Close();
                }
            }
            return visualColorFactors;
        }
        static internal List<ColorFactorValue> GetArtColorFactorValues(int colorFactorID){
            List<ColorFactorValue> colorFactorValues = new List<ColorFactorValue>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT defvalue, r, g, b, a FROM reaquisites.\"ArtefactColorFactorValues\" where artefactcolorfactor = "+colorFactorID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return colorFactorValues;
                        }
                        while (reader.Read())
                        {
                            ColorFactorValue cfv = new ColorFactorValue();
                            cfv.Key = reader[0].ToString();
                            cfv.R = (int)reader[1];
                            cfv.G = (int)reader[2];
                            cfv.B = (int)reader[3];
                            cfv.A = (int)reader[4];

                            colorFactorValues.Add(cfv);
                        }
                    }
                    con.Close();
                }
            }
            return colorFactorValues;
        }
        static internal List<(int, (ArtefactSizeFactor,int))> GetArtSizeFactorsForVisual(int visualID){
            List<(int, (ArtefactSizeFactor,int))> visualSizeFactors = new List<(int, (ArtefactSizeFactor,int))>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, interpolate, weight, artefactattributedef FROM reaquisites.\"ArtefactSizeFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return visualSizeFactors;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            ArtefactSizeFactor sizeFactor = new ArtefactSizeFactor();
                            sizeFactor.Interpolated = (bool)reader[1];
                            sizeFactor.Weight = (float)reader[2];
                            int attribID = (int)reader[3];
                            visualSizeFactors.Add((id, (sizeFactor,attribID)));
                        }
                    }
                    con.Close();
                }
            }
            return visualSizeFactors;
        }
        static internal List<SizeFactorValue> GetArtSizeFactorValues(int sizeFactorID){
            List<SizeFactorValue> sizeFactorValues = new List<SizeFactorValue>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT defvalue, value FROM reaquisites.\"ArtefactSizeFactorValues\" where artefactsizefactor = "+sizeFactorID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return sizeFactorValues;
                        }
                        while (reader.Read())
                        {
                            SizeFactorValue sfv = new SizeFactorValue();
                            sfv.Key = reader[0].ToString();
                            sfv.size = (int)reader[1];

                            sizeFactorValues.Add(sfv);
                        }
                    }
                    con.Close();
                }
            }
            return sizeFactorValues;
        }
        static internal List<(int, (RelationshipColorFactor,int))> GetRelColorFactorsForVisual(int visualID){
            List<(int, (RelationshipColorFactor,int))> visualColorFactors = new List<(int, (RelationshipColorFactor,int))>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, interpolate, weight, relationshipattributedef FROM reaquisites.\"RelationshipColorFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return visualColorFactors;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            RelationshipColorFactor colorFactor = new RelationshipColorFactor();
                            colorFactor.Interpolated = (bool)reader[1];
                            colorFactor.Weight = (float)reader[2];
                            int attribID = (int)reader[3];
                            visualColorFactors.Add((id, (colorFactor,attribID)));
                        }
                    }
                    con.Close();
                }
            }
            return visualColorFactors;
        }
        static internal List<ColorFactorValue> GetRelColorFactorValues(int colorFactorID){
            List<ColorFactorValue> colorFactorValues = new List<ColorFactorValue>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT defvalue, r, g, b, a FROM reaquisites.\"RelationshipColorFactorValues\" where relationshipcolorfactor = "+colorFactorID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return colorFactorValues;
                        }
                        while (reader.Read())
                        {
                            ColorFactorValue cfv = new ColorFactorValue();
                            cfv.Key = reader[0].ToString();
                            cfv.R = (int)reader[1];
                            cfv.G = (int)reader[2];
                            cfv.B = (int)reader[3];
                            cfv.A= (int)reader[4];

                            colorFactorValues.Add(cfv);
                        }
                    }
                    con.Close();
                }
            }
            return colorFactorValues;
        }
        static internal List<(int, (RelationshipSizeFactor,int))> GetRelSizeFactorsForVisual(int visualID){
            List<(int, (RelationshipSizeFactor,int))> visualSizeFactors = new List<(int, (RelationshipSizeFactor,int))>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, interpolate, weight, relationshipattributedef FROM reaquisites.\"RelationshipSizeFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return visualSizeFactors;
                        }
                        while (reader.Read())
                        {
                            int id = (int)reader[0];
                            RelationshipSizeFactor sizeFactor = new RelationshipSizeFactor();
                            sizeFactor.Interpolated = (bool)reader[1];
                            sizeFactor.Weight = (float)reader[2];
                            int attribID = (int)reader[3];
                            visualSizeFactors.Add((id, (sizeFactor,attribID)));
                        }
                    }
                    con.Close();
                }
            }
            return visualSizeFactors;
        }
        static internal List<SizeFactorValue> GetRelSizeFactorValues(int sizeFactorID){
            List<SizeFactorValue> sizeFactorValues = new List<SizeFactorValue>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT defvalue, value FROM reaquisites.\"RelationshipSizeFactorValues\" where relationshipsizefactor = "+sizeFactorID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows){
                            return sizeFactorValues;
                        }
                        while (reader.Read())
                        {
                            SizeFactorValue sfv = new SizeFactorValue();
                            sfv.Key = reader[0].ToString();
                            sfv.size = (int)reader[1];

                            sizeFactorValues.Add(sfv);
                        }
                    }
                    con.Close();
                }
            }
            return sizeFactorValues;
        }

        //REMOVE ELEMENTS
        static internal void DeleteArtDefinition(int projectID, int artefactDefRef){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"ArtefactDefs\" where project = "+projectID+" AND ref = "+artefactDefRef;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteAllArtDefAttributeDefs(int artDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"ArtefactAttributeDefs\" where artefactdef = "+artDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteRelDefinition(int projectID, int relationshipDefRef){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"RelationshipDefs\" where project = "+projectID+" AND ref = "+relationshipDefRef;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteAllRelDefAttributeDefs(int relDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"RelationshipAttributeDefs\" where relationshipdef = "+relDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteArtefact(int artDefID, int artefactRef){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"Artefacts\" where artefactdef = "+artDefID+" AND ref = "+artefactRef;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteAllArtAttributes(int artID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"ArtefactAttributes\" where artefact = "+artID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteRelationship(int relDefID, int relationshipRef){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"Relationships\" where relationshipdef = "+relDefID+" AND ref = "+relationshipRef;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteAllRelAttributes(int relID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"RelationshipAttributes\" where relationship = "+relID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        static internal void DeleteVisualization(int projectID, int visualRefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"VisualizationTemplates\" where project = "+projectID+" and ref = "+visualRefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void DeleteAllVisualizationFactors(int visualID){
            
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "DELETE FROM reaquisites.\"ArtefactColorFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                query = "DELETE FROM reaquisites.\"RelationshipColorFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                query = "DELETE FROM reaquisites.\"ArtefactSizeFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                query = "DELETE FROM reaquisites.\"RelationshipSizeFactors\" where visualizationtemplate = "+visualID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        //UPDATE ELEMENTS
        static internal void UpdateArtDefinition(int artDefID, string artDefName, string? artDefDescription, int artDefShape){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "UPDATE reaquisites.\"ArtefactDefs\" "+
                "SET name = '"+artDefName+"', description = '"+artDefDescription+"', shape = "+artDefShape+
                " where id = "+artDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void UpdateRelDefinition(int relDefID, string relDefName, string? relDefDescription, int relDefShape){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "UPDATE reaquisites.\"RelationshipDefs\" "+
                "SET name = '"+relDefName+"', description = '"+relDefDescription+"', shape = "+relDefShape+
                " where id = "+relDefID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void UpdateArtefact(int artID, string artName, string? artDescription, int newArtDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "UPDATE reaquisites.\"Artefacts\" "+
                "SET name = '"+artName+"', description = '"+artDescription+"', artefactdef = "+newArtDefID+
                " where id = "+artID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void UpdateArtefactAttributeValue(int attributeDefID, int artID, string value){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "UPDATE reaquisites.\"ArtefactAttributes\" "+
                "SET value = '"+value+"' where artefactattributedef = "+attributeDefID+" and artefact = "+artID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void UpdateRelationship(int relID, int relDefID, string? artDescription, int parentID, int childID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "UPDATE reaquisites.\"Relationships\" "+
                "SET relationshipdef = "+relDefID+", description = '"+artDescription+"', parent = "+parentID+", child = "+childID+
                " where id = "+relID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void UpdateRelationshipAttributeValue(int attributeDefID, int relID, string value){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "UPDATE reaquisites.\"RelationshipAttributes\" "+
                "SET value = '"+value+"' where relationshipattributedef = "+attributeDefID+" and relationship = "+relID;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        static internal void UpdateVisualization(int visualID, string visualName, string? visualDescription, int graph){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "UPDATE reaquisites.\"VisualizationTemplates\" "+
                "SET name = '"+visualName+"', description = '"+visualDescription+"'"+", graphTemplate = "+graph+
                " where id = "+visualID;
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