using reaquisites.Models;
using Npgsql;

namespace reaquisites.Services.DB
{
    public static class DBProjectService
    {
        internal static string connString;
        
        
        static internal int AddProjectHistoryEntry(int projectID, int elementType, int element, int type, string changes, DateTime date){

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
            return 0;
        }

        static internal List<SimpleProjectDTO> GetUserProjectsSimpleDTO(int userID){
            List<SimpleProjectDTO> userProjects = new List<SimpleProjectDTO>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string projectsQuery = "SELECT id, name, description, version, published FROM reaquisites.\"Projects\" where user_id = "+userID;
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
                string projectsQuery = "SELECT id FROM reaquisites.\"Projects\" where user_id = "+userID+" AND name = '"+projectName+"'";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return projectID;
                        }
                        while (projectsReader.Read())
                        {
                            projectID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"ArtefactDefs\" where project = "+projectID+" AND name = '"+artefactDefName+"'";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return artDefID;
                        }
                        while (projectsReader.Read())
                        {
                            artDefID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"RelationshipDefs\" where project = "+projectID+" AND name = '"+relationshipDefName+"'";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return artDefID;
                        }
                        while (projectsReader.Read())
                        {
                            artDefID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"ArtefactAttributeDefs\" where artefactdef = "+artDefID+" AND name = '"+artAttribName+"'";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return artAttribDefID;
                        }
                        while (projectsReader.Read())
                        {
                            artAttribDefID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"RelationshipAttributeDefs\" where relationshipdef = "+relDefID+" AND name = '"+artAttribName+"'";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return relAttribDefID;
                        }
                        while (projectsReader.Read())
                        {
                            relAttribDefID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"Artefacts\" where artefactdef = "+artDefID+" AND name = '"+artName+"'";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return artID;
                        }
                        while (projectsReader.Read())
                        {
                            artID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"ArtefactAttributes\" where artefact = "+artID+" AND artefactattributedef = "+artAttribDefID+"";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return artAttribID;
                        }
                        while (projectsReader.Read())
                        {
                            artAttribID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"Relationships\" where relationshipdef = "+relDefID+" AND parent = "+parentID+" AND child = "+childID;
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return relID;
                        }
                        while (projectsReader.Read())
                        {
                            relID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"VisualizationTemplates\" where project = "+projectID+" AND name = '"+visualizationName+"'";
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return visualizationID;
                        }
                        while (projectsReader.Read())
                        {
                            visualizationID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"ArtefactColorFactors\" where artefactattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return artColorFactorID;
                        }
                        while (projectsReader.Read())
                        {
                            artColorFactorID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"RelationshipColorFactors\" where relationshipattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return relColorFactorID;
                        }
                        while (projectsReader.Read())
                        {
                            relColorFactorID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"ArtefactSizeFactors\" where artefactattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return artSizeFactorID;
                        }
                        while (projectsReader.Read())
                        {
                            artSizeFactorID = (int)projectsReader[0];
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
                string projectsQuery = "SELECT id FROM reaquisites.\"RelationshipSizeFactors\" where relationshipattributedef = "+attribDefID+" AND visualizationtemplate = "+visualizationID;
                using (NpgsqlCommand projectsCmd = new NpgsqlCommand(projectsQuery))
                {
                    projectsCmd.Connection = con;
                    con.Open();
                    using (NpgsqlDataReader projectsReader = projectsCmd.ExecuteReader())
                    {
                        if (!projectsReader.HasRows){
                            return relSizeFactorID;
                        }
                        while (projectsReader.Read())
                        {
                            relSizeFactorID = (int)projectsReader[0];
                        }
                    }
                    con.Close();
                }
            }
            return relSizeFactorID;
        }

        //ADD ELEMENTS
        static internal void AddProject(int userID, string projectName, string projectDescription, bool projectIsPublished, bool projectIsTemplate){
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO reaquisites.\"Projects\" (name, description, published, template, user_id) "+
                "VALUES ('"+projectName+"','"+projectDescription+"',"+projectIsPublished+", "+projectIsTemplate+", "+userID+")";
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
    }
}