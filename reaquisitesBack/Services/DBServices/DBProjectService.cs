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
                string query = "INSERT INTO reaquisites.\"Projects\" (name, description, published, template, user_id) "+
                "VALUES ('"+project.Name+"','"+project.Description+"',"+project.IsPublished+", "+project.IsTemplate+", "+userID+")";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            int projectID = GetProjectID(userID,project.Name);

            Dictionary<AttributeDefinition, int> attribDefs = new Dictionary<AttributeDefinition, int>();
            
            // ARTEFACT DEFINITIONS
            Dictionary<ArtefactDefinition, int> artDefs = new Dictionary<ArtefactDefinition, int>();
            foreach (ArtefactDefinition artDef in project.ArtefactDefs){
                AddArtefactDefinition(projectID,artDef.Name,artDef.Description,artDef.Shape);
                int artDefID = GetArtefactDefID(projectID, artDef.Name);
                foreach(AttributeDefinition artAttribDef in artDef.AttributeDefinitions){
                    AddArtefactAttributeDefinition(artAttribDef.Name,artAttribDef.Description,artAttribDef.Values,artDefID);
                    int artAttribDefID = GetArtefactAttributeDefID(artDefID, artAttribDef.Name);
                    foreach (HistoryEntry he in artAttribDef.HistoryEntries){
                        AddProjectHistoryEntry(projectID,3,artAttribDefID, he.Type, he.Changes, he.ChangeDate);
                    }
                    attribDefs.Add(artAttribDef,artAttribDefID);
                }
                foreach (HistoryEntry he in artDef.HistoryEntries){
                    AddProjectHistoryEntry(projectID,1,artDefID, he.Type, he.Changes, he.ChangeDate);
                }
                artDefs.Add(artDef,artDefID);
            }

            // RELATIONSHIP DEFINITIONS
            Dictionary<RelationshipDefinition, int> relDefs = new Dictionary<RelationshipDefinition, int>();
            foreach (RelationshipDefinition relDef in project.RelationshipDefs){
                AddRelationshipDefinition(projectID, relDef.Name, relDef.Description, relDef.Shape);
                int relDefID = GetRelationshipDefID(projectID, relDef.Name);
                foreach(AttributeDefinition artAttribDef in relDef.AttributeDefinitions){
                    AddRelationshipAttributeDefinition(artAttribDef.Name,artAttribDef.Description,artAttribDef.Values, relDefID);
                    int artAttribDefID = GetRelationshipAttributeDefID(relDefID, artAttribDef.Name);
                    foreach (HistoryEntry he in artAttribDef.HistoryEntries){
                        AddProjectHistoryEntry(projectID,4,artAttribDefID, he.Type, he.Changes, he.ChangeDate);
                    }
                    attribDefs.Add(artAttribDef,artAttribDefID);
                }
                foreach (HistoryEntry he in relDef.HistoryEntries){
                    AddProjectHistoryEntry(projectID,2,relDefID, he.Type, he.Changes, he.ChangeDate);
                }
                relDefs.Add(relDef,relDefID);
            }
            
            // ARTEFACTS
            Dictionary<Artefact, int> artefacts = new Dictionary<Artefact, int>();
            foreach (Artefact artefact in project.Artefacts){
                int artDefID = artDefs[artefact.Definition];
                AddArtefact(artefact.Name, artDefID);
                int artID = GetArtefactID(projectID, artefact.Name);
                foreach (Attribute attrib in artefact.Attributes){
                    int attributeDefID = attribDefs[attrib.Definition];
                    AddArtefactAttribute(artID,attributeDefID,attrib.Value);
                }

                foreach (HistoryEntry he in artefact.HistoryEntries){
                    AddProjectHistoryEntry(projectID,5,artID, he.Type, he.Changes, he.ChangeDate);
                }
                artefacts.Add(artefact,artID);
            }

            // RELATIONSHIPS
            foreach (Relationship relation in project.Relationships){
                int relDefID = relDefs[relation.Definition];
                int parentID = artefacts[relation.Parent];
                int childID = artefacts[relation.Child];
                AddRelationship(relDefID,parentID,childID);
                int relID = GetRelationshipID(relDefID, parentID, childID);
                foreach (Attribute attrib in relation.Attributes){
                    int attributeDefID = attribDefs[attrib.Definition];
                    AddRelationshipAttribute(relID, attributeDefID, attrib.Value);
                }
                foreach (HistoryEntry he in relation.HistoryEntries){
                    AddProjectHistoryEntry(projectID,6,relID, he.Type, he.Changes, he.ChangeDate);
                }
            }


            //VISUALIZATION TEMPLATES
            foreach (Visualization visual in project.Visualizations){
                AddVisualizationTemplate(visual.Name,visual.Description,projectID);
                int visualID = GetVisualizationID(projectID, visual.Name);

                //ARTEFACT COLOR FACTOR
                foreach (ColorFactor colorFactor in visual.ArtefactColorFactors){
                    int attribDefID = attribDefs[colorFactor.Definition];
                    AddArtefactColorFactor(attribDefID, visualID, colorFactor.Weight);
                    int colorFactorID = GetArtefactColorFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, System.Drawing.Color> points in colorFactor.Values){
                        AddArtefactColorFactorValue(colorFactorID, points.Key, points.Value.R, points.Value.G, points.Value.B, points.Value.A);
                    }
                    foreach (HistoryEntry he in colorFactor.HistoryEntries){
                        AddProjectHistoryEntry(projectID,7,colorFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }

                //RELATIONSHIP COLOR FACTOR
                foreach (ColorFactor colorFactor in visual.RelationshipColorFactors){
                    int attribDefID = attribDefs[colorFactor.Definition];
                    AddArtefactColorFactor(attribDefID,visualID,colorFactor.Weight);
                    int colorFactorID = GetRelationshipColorFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, System.Drawing.Color> points in colorFactor.Values){
                        AddRelationshipColorFactorValue(colorFactorID,points.Key,points.Value.R,points.Value.G,points.Value.B,points.Value.A);
                    }
                    foreach (HistoryEntry he in colorFactor.HistoryEntries){
                        AddProjectHistoryEntry(projectID,8,colorFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }
                
                //ARTEFACT SIZE FACTOR
                foreach (SizeFactor sizeFactor in visual.ArtefactSizeFactors){
                    int attribDefID = attribDefs[sizeFactor.Definition];
                    AddArtefactSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = GetArtefactSizeFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, int> points in sizeFactor.Values){
                        AddArtefactSizeFactorValue(sizeFactorID,points.Key,points.Value);
                    }
                    foreach (HistoryEntry he in sizeFactor.HistoryEntries){
                        AddProjectHistoryEntry(projectID,9,sizeFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }

                //RELATIONSHIP SIZE FACTOR
                foreach (SizeFactor sizeFactor in visual.RelationshipSizeFactors){
                    int attribDefID = attribDefs[sizeFactor.Definition];
                    AddRelationshipSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = GetRelationshipSizeFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, int> points in sizeFactor.Values){
                        AddRelationshipSizeFactorValue(sizeFactorID,points.Key,points.Value);
                    }
                    foreach (HistoryEntry he in sizeFactor.HistoryEntries){
                        AddProjectHistoryEntry(projectID,10,sizeFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }
                foreach (HistoryEntry he in visual.HistoryEntries){
                    AddProjectHistoryEntry(projectID,11,visualID,he.Type,he.Changes,he.ChangeDate);
                }
            }
            foreach (HistoryEntry he in project.HistoryEntries){
                AddProjectHistoryEntry(projectID,0,projectID,he.Type,he.Changes,he.ChangeDate);
            }
            return 0;
        }
        public static int AddProjectHistoryEntry(int projectID, int elementType, int element, int type, string changes, DateTime date){

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

        public static List<SimpleProjectDTO> GetUserProjectsSimpleDTO(int userID){
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

        // ID GETTERS
        private static int GetProjectID(int userID, string projectName){
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

        private static int GetArtefactDefID(int projectID, string artefactDefName){
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
        private static int GetRelationshipDefID(int projectID, string relationshipDefName){
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
        private static int GetArtefactAttributeDefID(int artDefID, string artAttribName){
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
        private static int GetRelationshipAttributeDefID(int relDefID, string artAttribName){
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
        
        private static int GetArtefactID(int artDefID, string artName){
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
        private static int GetArtefactAttributeID(int artID, string artAttribDefID){
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
        private static int GetRelationshipID(int relDefID, int parentID, int childID){
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

        private static int GetVisualizationID(int projectID, string visualizationName){
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
        private static int GetArtefactColorFactorID(int visualizationID, int attribDefID){
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
        private static int GetRelationshipColorFactorID(int visualizationID, int attribDefID){
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
        private static int GetArtefactSizeFactorID(int visualizationID, int attribDefID){
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
        private static int GetRelationshipSizeFactorID(int visualizationID, int attribDefID){
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
        private static void AddArtefactDefinition(int projectID, string artDefName, string artDefDescription, int artDefShape){
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
        private static void AddArtefactAttributeDefinition(string artAttribDefName, string artAttribDefDescription, string artAttribDefValues, int artDefID){
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
        private static void AddRelationshipDefinition(int projectID, string relDefName, string relDefDescription, int relDefShape){
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
        private static void AddRelationshipAttributeDefinition(string relAttribDefName, string relAttribDefDescription, string relAttribDefValues, int relDefID){
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
        private static void AddArtefact(string artName, int artDefID){
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
        private static void AddArtefactAttribute(int artID, int attributeDefID, string value){
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
        private static void AddRelationship(int relDefID, int parentID, int childID){
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
        private static void AddRelationshipAttribute(int relID, int attributeDefID, string value){
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
        private static void AddVisualizationTemplate(string visualName, string visualDescription, int projectID){
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
        private static void AddArtefactColorFactor(int attribDefID, int visualID, float weight){
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
        private static void AddArtefactColorFactorValue(int colorFactorID, string defvalue, byte R, byte G, byte B, byte A){
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
        private static void AddRelationshipColorFactor(int attribDefID, int visualID, float weight){
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
        private static void AddRelationshipColorFactorValue(int colorFactorID, string defvalue, byte R, byte G, byte B, byte A){
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
        private static void AddArtefactSizeFactor(int attribDefID, int visualID, float weight){
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
        private static void AddArtefactSizeFactorValue(int sizeFactorID, string defvalue, float value){
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
        private static void AddRelationshipSizeFactor(int attribDefID, int visualID, float weight){
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
        private static void AddRelationshipSizeFactorValue(int sizeFactorID, string defvalue, float value){
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