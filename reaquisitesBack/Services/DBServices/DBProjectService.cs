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
                string query = "SELECT id, name, description, version, published FROM reaquisites.\"Projects\" where user_id = "+userID+" and template = false";
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
        static internal int GetProjectID(int userID, string projectName){
            int projectID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Projects\" where user_id = "+userID+" AND name = '"+projectName+"'";
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

        static internal int GetArtefactDefID(int projectID, string artefactDefName){
            int artDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"ArtefactDefs\" where project = "+projectID+" AND name = '"+artefactDefName+"'";
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
        static internal int GetRelationshipDefID(int projectID, string relationshipDefName){
            int artDefID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"RelationshipDefs\" where project = "+projectID+" AND name = '"+relationshipDefName+"'";
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
        
        static internal int GetArtefactID(int artDefID, string artName){
            int artID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Artefacts\" where artefactdef = "+artDefID+" AND name = '"+artName+"'";
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
        static internal int GetRelationshipID(int relDefID, int parentID, int childID){
            int relID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"Relationships\" where relationshipdef = "+relDefID+" AND parent = "+parentID+" AND child = "+childID;
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

        static internal int GetVisualizationID(int projectID, string visualizationName){
            int visualizationID = -1;
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id FROM reaquisites.\"VisualizationTemplates\" where project = "+projectID+" AND name = '"+visualizationName+"'";
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

        //ADD ELEMENTS
        static internal void AddProject(int userID, string projectName, string projectDescription, bool projectIsPublished, bool projectIsTemplate, string version){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Projects\" (name, description, published, template, version, user_id) "+
                "VALUES ('"+projectName+"','"+projectDescription+"',"+projectIsPublished+", "+projectIsTemplate+", "+version+", "+userID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactDefinition(int projectID, string artDefName, string artDefDescription, int artDefShape){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactDefs\" (project, name, description, shape) "+
                "VALUES ("+projectID+", '"+artDefName+"', '"+artDefDescription+"', "+artDefShape+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactAttributeDefinition(string artAttribDefName, string artAttribDefDescription, string artAttribDefValues, int artDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactAttributeDefs\" (name, description, values, artefactdef) "+
                "VALUES ('"+artAttribDefName+"', '"+artAttribDefDescription+"', '"+artAttribDefValues+"', "+artDefID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipDefinition(int projectID, string relDefName, string relDefDescription, int relDefShape){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipDef\" (project, name, description, shape) "+
                "VALUES ("+projectID+", '"+relDefName+"', '"+relDefDescription+"', "+relDefShape+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddRelationshipAttributeDefinition(string relAttribDefName, string relAttribDefDescription, string relAttribDefValues, int relDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipAttributeDefs\" (name, description, values, relationshipdef) "+
                "VALUES ('"+relAttribDefName+"', '"+relAttribDefDescription+"', '"+relAttribDefValues+"', "+relDefID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefact(string artName, int artDefID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Artefacts\" (name, artefactdef) "+
                "VALUES ('"+artName+"', "+artDefID+")";
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
        static internal void AddRelationship(int relDefID, int parentID, int childID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Relationships\" (parent, child, relationshipdef) "+
                "VALUES ("+parentID+", "+childID+", "+relDefID+")";
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
        static internal void AddVisualizationTemplate(string visualName, string visualDescription, int projectID){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"VisualizationTemplates\" (name, description, project) "+
                "VALUES ( "+visualName+", "+visualDescription+", "+projectID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        static internal void AddArtefactColorFactor(int attribDefID, int visualID, float weight){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactColorFactors\" (artefactattributedef, visualizationtemplate, weight) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+")";
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
                string query = "INSERT INTO reaquisites.\"ArtefactColorFactorValues\" (colorfactor, defvalue, r, g, b, a) "+
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
        static internal void AddRelationshipColorFactor(int attribDefID, int visualID, float weight){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipColorFactors\" (relationshiptattributedef, visualizationtemplate, weight) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+")";
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
                string query = "INSERT INTO reaquisites.\"RelationshipColorFactorValues\" (colorfactor, defvalue, r, g, b, a) "+
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
        static internal void AddArtefactSizeFactor(int attribDefID, int visualID, float weight){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"ArtefactSizeFactors\" (artefactattributedef, visualizationtemplate, weight) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+")";
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
                string query = "INSERT INTO reaquisites.\"ArtefactColorFactorValues\" (colorfactor, defvalue, value) "+
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
        static internal void AddRelationshipSizeFactor(int attribDefID, int visualID, float weight){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"RelationshipSizeFactors\" (relationshipattributedef, visualizationtemplate, weight) "+
                "VALUES ( "+attribDefID+", "+visualID+", "+weight+")";
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
                string query = "INSERT INTO reaquisites.\"RelationshipColorFactorValues\" (colorfactor, defvalue, value) "+
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
        static internal List<HistoryEntry> GetHistoryEntriesForElement(int projectID, int elementType, int elementID){
            List<HistoryEntry> HEs = new List<HistoryEntry>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT type, changes, date FROM reaquisites.\"HistoryEntries\" where project = "+projectID+" AND element_type = "+elementType+" and element = "+elementID;
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
                            he.Type = (int)reader[0];
                            he.Changes = reader[1].ToString();
                            he.ChangeDate = DateTime.Parse(reader[2].ToString());
                            HEs.Add(he);
                        }
                    }
                    con.Close();
                }
            }
            return HEs;
        }
        static internal (int, Project) GetUserProject(int userID, string projectName){
            (int, Project) userProject = (-1,new Project());
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name, description, published, version FROM reaquisites.\"Projects\" where user_id = "+userID+" AND name = '"+projectName+"'";
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
                string query = "SELECT id, name, description, shape FROM reaquisites.\"ArtefactDefs\" where project = "+projectID;
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
                            artDef.Description = reader[2].ToString();
                            artDef.Shape = (int)reader[3];
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
                string query = "SELECT id, name, description, values FROM reaquisites.\"ArtefactAttributeDefs\" where artefactdef = "+artDefID;
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
                            artAttribDef.Values = reader[3].ToString();
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
                string query = "SELECT id, name, description, shape FROM reaquisites.\"RelationshipDefs\" where project = "+projectID;
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
                            RelationshipDefinition artDef = new RelationshipDefinition();
                            artDef.Name = reader[1].ToString();
                            artDef.Description = reader[2].ToString();
                            artDef.Shape = (int)reader[3];
                            projectRelDefs.Add((id, artDef));
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
                string query = "SELECT id, name, description, values FROM reaquisites.\"RelationshipAttributeDefs\" where relationshipdef = "+relDefID;
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
                            AttributeDefinition artAttribDef = new AttributeDefinition();
                            artAttribDef.Name = reader[1].ToString();
                            artAttribDef.Description = reader[2].ToString();
                            artAttribDef.Values = reader[3].ToString();
                            projectRelAttribDefs.Add(id, artAttribDef);
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
                string query = "SELECT id, name FROM reaquisites.\"Artefacts\" where artefactdef = "+artDefID;
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
        static internal List<(int, (int,int))> GetRelationshipsForDefinition(int relDefID){
            List<(int, (int,int))> relationshipsForDef = new List<(int, (int,int))>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, name FROM reaquisites.\"Relationships\" where relationshipdef = "+relDefID;
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
                            relationshipsForDef.Add((id,(parentID,childID)));
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
                string query = "SELECT id, name, description FROM reaquisites.\"VisualizationTemplates\" where project = "+projectID;
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
                            projectVisuals.Add((id, visual));
                        }
                    }
                    con.Close();
                }
            }
            return projectVisuals;
        }
        static internal List<(int, (ColorFactor,int))> GetArtColorFactorsForVisual(int visualID){
            List<(int, (ColorFactor,int))> visualColorFactors = new List<(int, (ColorFactor,int))>();
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
                            ColorFactor colorFactor = new ColorFactor();
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
        static internal Dictionary<string, System.Drawing.Color> GetArtColorFactorValues(int colorFactorID){
            Dictionary<string, System.Drawing.Color> colorFactorValues = new Dictionary<string, System.Drawing.Color>();
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
                            string defvalue = reader[0].ToString();
                            int r = (int)reader[1];
                            int g = (int)reader[2];
                            int b = (int)reader[3];
                            int a = (int)reader[4];

                            colorFactorValues.Add(defvalue, System.Drawing.Color.FromArgb(a,r,g,b));
                        }
                    }
                    con.Close();
                }
            }
            return colorFactorValues;
        }
        static internal List<(int, (SizeFactor,int))> GetArtSizeFactorsForVisual(int visualID){
            List<(int, (SizeFactor,int))> visualSizeFactors = new List<(int, (SizeFactor,int))>();
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
                            SizeFactor sizeFactor = new SizeFactor();
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
        static internal Dictionary<string, int> GetArtSizeFactorValues(int sizeFactorID){
            Dictionary<string, int> sizeFactorValues = new Dictionary<string, int>();
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
                            string defvalue = reader[0].ToString();
                            int value = (int)reader[1];

                            sizeFactorValues.Add(defvalue, value);
                        }
                    }
                    con.Close();
                }
            }
            return sizeFactorValues;
        }
        static internal List<(int, (ColorFactor,int))> GetRelColorFactorsForVisual(int visualID){
            List<(int, (ColorFactor,int))> visualColorFactors = new List<(int, (ColorFactor,int))>();
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
                            ColorFactor colorFactor = new ColorFactor();
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
        static internal Dictionary<string, System.Drawing.Color> GetRelColorFactorValues(int colorFactorID){
            Dictionary<string, System.Drawing.Color> colorFactorValues = new Dictionary<string, System.Drawing.Color>();
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
                            string defvalue = reader[0].ToString();
                            int r = (int)reader[1];
                            int g = (int)reader[2];
                            int b = (int)reader[3];
                            int a = (int)reader[4];

                            colorFactorValues.Add(defvalue, System.Drawing.Color.FromArgb(a,r,g,b));
                        }
                    }
                    con.Close();
                }
            }
            return colorFactorValues;
        }
        static internal List<(int, (SizeFactor,int))> GetRelSizeFactorsForVisual(int visualID){
            List<(int, (SizeFactor,int))> visualSizeFactors = new List<(int, (SizeFactor,int))>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "SELECT id, interpolate, weight, artefactattributedef FROM reaquisites.\"RelationshipSizeFactors\" where visualizationtemplate = "+visualID;
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
                            SizeFactor sizeFactor = new SizeFactor();
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
        static internal Dictionary<string, int> GetRelSizeFactorValues(int sizeFactorID){
            Dictionary<string, int> sizeFactorValues = new Dictionary<string, int>();
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
                            string defvalue = reader[0].ToString();
                            int value = (int)reader[1];

                            sizeFactorValues.Add(defvalue, value);
                        }
                    }
                    con.Close();
                }
            }
            return sizeFactorValues;
        }



    }
}