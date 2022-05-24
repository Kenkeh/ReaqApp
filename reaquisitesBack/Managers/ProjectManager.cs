using reaquisites.Models;
using reaquisites.Services.DB;
namespace reaquisites.Managers
{
    public static class ProjectManager
    {
        static private string cString="";
        static internal string connString {
            get{return cString;} 
            set{
                cString = value;
                DBProjectService.connString = value;
        }}
        
        static internal KeyValuePair<int,Project> addNewProject(string accName, NewProjectDTO newProject){
            Project project = new Project();
            project.Name = newProject.projectName;
            project.Description = newProject.projectDesc;
            project.Version = "0";
            project.ArtefactDefs = new List<ArtefactDefinition>();
            project.Artefacts = new List<Artefact>();
            project.RelationshipDefs = new List<RelationshipDefinition>();
            project.Relationships = new List<Relationship>();
            project.IsPublished = project.IsTemplate = false;
            project.Visualizations = new List<Visualization>();
            project.HistoryEntries = new List<HistoryEntry>();
            project.HistoryEntries.Add(HEFactory.createProjectCreationEntry(project));
            return new KeyValuePair<int, Project>(AddProjectToUser(accName, project), project);
        }
        
        static internal int AddProjectToUser(string userAccount, Project project)
        {
            int userID = DBUserService.GetUserId(userAccount);
            if (userID<0) return -1;
            DBProjectService.AddProject(userID, project.Name, project.Description,project.IsPublished, project.IsTemplate);
            int projectID = DBProjectService.GetProjectID(userID,project.Name);

            Dictionary<AttributeDefinition, int> attribDefs = new Dictionary<AttributeDefinition, int>();
            
            // ARTEFACT DEFINITIONS
            Dictionary<ArtefactDefinition, int> artDefs = new Dictionary<ArtefactDefinition, int>();
            foreach (ArtefactDefinition artDef in project.ArtefactDefs){
                DBProjectService.AddArtefactDefinition(projectID,artDef.Name,artDef.Description,artDef.Shape);
                int artDefID = DBProjectService.GetArtefactDefID(projectID, artDef.Name);
                foreach(AttributeDefinition artAttribDef in artDef.AttributeDefinitions){
                    DBProjectService.AddArtefactAttributeDefinition(artAttribDef.Name,artAttribDef.Description,artAttribDef.Values,artDefID);
                    int artAttribDefID = DBProjectService.GetArtefactAttributeDefID(artDefID, artAttribDef.Name);
                    foreach (HistoryEntry he in artAttribDef.HistoryEntries){
                        DBProjectService.AddProjectHistoryEntry(projectID,3,artAttribDefID, he.Type, he.Changes, he.ChangeDate);
                    }
                    attribDefs.Add(artAttribDef,artAttribDefID);
                }
                foreach (HistoryEntry he in artDef.HistoryEntries){
                    DBProjectService.AddProjectHistoryEntry(projectID,1,artDefID, he.Type, he.Changes, he.ChangeDate);
                }
                artDefs.Add(artDef,artDefID);
            }

            // RELATIONSHIP DEFINITIONS
            Dictionary<RelationshipDefinition, int> relDefs = new Dictionary<RelationshipDefinition, int>();
            foreach (RelationshipDefinition relDef in project.RelationshipDefs){
                DBProjectService.AddRelationshipDefinition(projectID, relDef.Name, relDef.Description, relDef.Shape);
                int relDefID = DBProjectService.GetRelationshipDefID(projectID, relDef.Name);
                foreach(AttributeDefinition artAttribDef in relDef.AttributeDefinitions){
                    DBProjectService.AddRelationshipAttributeDefinition(artAttribDef.Name,artAttribDef.Description,artAttribDef.Values, relDefID);
                    int artAttribDefID = DBProjectService.GetRelationshipAttributeDefID(relDefID, artAttribDef.Name);
                    foreach (HistoryEntry he in artAttribDef.HistoryEntries){
                        DBProjectService.AddProjectHistoryEntry(projectID,4,artAttribDefID, he.Type, he.Changes, he.ChangeDate);
                    }
                    attribDefs.Add(artAttribDef,artAttribDefID);
                }
                foreach (HistoryEntry he in relDef.HistoryEntries){
                    DBProjectService.AddProjectHistoryEntry(projectID,2,relDefID, he.Type, he.Changes, he.ChangeDate);
                }
                relDefs.Add(relDef,relDefID);
            }
            
            // ARTEFACTS
            Dictionary<Artefact, int> artefacts = new Dictionary<Artefact, int>();
            foreach (Artefact artefact in project.Artefacts){
                int artDefID = artDefs[artefact.Definition];
                DBProjectService.AddArtefact(artefact.Name, artDefID);
                int artID = DBProjectService.GetArtefactID(projectID, artefact.Name);
                foreach (Attribute attrib in artefact.Attributes){
                    int attributeDefID = attribDefs[attrib.Definition];
                    DBProjectService.AddArtefactAttribute(artID,attributeDefID,attrib.Value);
                }

                foreach (HistoryEntry he in artefact.HistoryEntries){
                    DBProjectService.AddProjectHistoryEntry(projectID,5,artID, he.Type, he.Changes, he.ChangeDate);
                }
                artefacts.Add(artefact,artID);
            }

            // RELATIONSHIPS
            foreach (Relationship relation in project.Relationships){
                int relDefID = relDefs[relation.Definition];
                int parentID = artefacts[relation.Parent];
                int childID = artefacts[relation.Child];
                DBProjectService.AddRelationship(relDefID,parentID,childID);
                int relID = DBProjectService.GetRelationshipID(relDefID, parentID, childID);
                foreach (Attribute attrib in relation.Attributes){
                    int attributeDefID = attribDefs[attrib.Definition];
                    DBProjectService.AddRelationshipAttribute(relID, attributeDefID, attrib.Value);
                }
                foreach (HistoryEntry he in relation.HistoryEntries){
                    DBProjectService.AddProjectHistoryEntry(projectID,6,relID, he.Type, he.Changes, he.ChangeDate);
                }
            }


            //VISUALIZATION TEMPLATES
            foreach (Visualization visual in project.Visualizations){
                DBProjectService.AddVisualizationTemplate(visual.Name,visual.Description,projectID);
                int visualID = DBProjectService.GetVisualizationID(projectID, visual.Name);

                //ARTEFACT COLOR FACTOR
                foreach (ColorFactor colorFactor in visual.ArtefactColorFactors){
                    int attribDefID = attribDefs[colorFactor.Definition];
                    DBProjectService.AddArtefactColorFactor(attribDefID, visualID, colorFactor.Weight);
                    int colorFactorID = DBProjectService.GetArtefactColorFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, System.Drawing.Color> points in colorFactor.Values){
                        DBProjectService.AddArtefactColorFactorValue(colorFactorID, points.Key, points.Value.R, points.Value.G, points.Value.B, points.Value.A);
                    }
                    foreach (HistoryEntry he in colorFactor.HistoryEntries){
                        DBProjectService.AddProjectHistoryEntry(projectID,7,colorFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }

                //RELATIONSHIP COLOR FACTOR
                foreach (ColorFactor colorFactor in visual.RelationshipColorFactors){
                    int attribDefID = attribDefs[colorFactor.Definition];
                    DBProjectService.AddArtefactColorFactor(attribDefID,visualID,colorFactor.Weight);
                    int colorFactorID = DBProjectService.GetRelationshipColorFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, System.Drawing.Color> points in colorFactor.Values){
                        DBProjectService.AddRelationshipColorFactorValue(colorFactorID,points.Key,points.Value.R,points.Value.G,points.Value.B,points.Value.A);
                    }
                    foreach (HistoryEntry he in colorFactor.HistoryEntries){
                        DBProjectService.AddProjectHistoryEntry(projectID,8,colorFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }
                
                //ARTEFACT SIZE FACTOR
                foreach (SizeFactor sizeFactor in visual.ArtefactSizeFactors){
                    int attribDefID = attribDefs[sizeFactor.Definition];
                    DBProjectService.AddArtefactSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = DBProjectService.GetArtefactSizeFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, int> points in sizeFactor.Values){
                        DBProjectService.AddArtefactSizeFactorValue(sizeFactorID,points.Key,points.Value);
                    }
                    foreach (HistoryEntry he in sizeFactor.HistoryEntries){
                        DBProjectService.AddProjectHistoryEntry(projectID,9,sizeFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }

                //RELATIONSHIP SIZE FACTOR
                foreach (SizeFactor sizeFactor in visual.RelationshipSizeFactors){
                    int attribDefID = attribDefs[sizeFactor.Definition];
                    DBProjectService.AddRelationshipSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = DBProjectService.GetRelationshipSizeFactorID(visualID,attribDefID);
                    foreach (KeyValuePair<string, int> points in sizeFactor.Values){
                        DBProjectService.AddRelationshipSizeFactorValue(sizeFactorID,points.Key,points.Value);
                    }
                    foreach (HistoryEntry he in sizeFactor.HistoryEntries){
                        DBProjectService.AddProjectHistoryEntry(projectID,10,sizeFactorID,he.Type,he.Changes,he.ChangeDate);
                    }
                }
                foreach (HistoryEntry he in visual.HistoryEntries){
                    DBProjectService.AddProjectHistoryEntry(projectID,11,visualID,he.Type,he.Changes,he.ChangeDate);
                }
            }
            foreach (HistoryEntry he in project.HistoryEntries){
                DBProjectService.AddProjectHistoryEntry(projectID,0,projectID,he.Type,he.Changes,he.ChangeDate);
            }
            return 0;
        }
    }

}