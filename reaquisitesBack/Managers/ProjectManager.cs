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
            project.Visualizations = new List<Visualization>();
            project.HistoryEntries = new List<HistoryEntry>();
            HistoryEntry he = new HistoryEntry();
            he.ChangeType = 1;
            he.ElementType = 0;
            he.ElementId = 0;
            he.Changes = "Project created with name: \""+project.Name+"\" and description: \""+project.Description+"\"";
            he.ChangeDate = DateTime.Now;
            project.HistoryEntries.Add(he);
            return new KeyValuePair<int, Project>(AddProjectToUser(accName, project), project);
        }
        
        static internal int AddProjectToUser(string userAccount, Project project)
        {
            int userID = DBUserService.GetUserId(userAccount);
            if (userID<0) return -1;
            DBProjectService.AddProject(userID, project.Name, project.Description,project.IsPublished, false, project.Version);
            int projectID = DBProjectService.GetProjectID(userID,project.Name);
            if (projectID<0) return -2;

            Dictionary<AttributeDefinition, int> attribDefs = new Dictionary<AttributeDefinition, int>();
            
            // ARTEFACT DEFINITIONS
            Dictionary<ArtefactDefinition, int> artDefs = new Dictionary<ArtefactDefinition, int>();
            foreach (ArtefactDefinition artDef in project.ArtefactDefs){
                DBProjectService.AddArtefactDefinition(projectID,artDef.Name,artDef.Description,artDef.Shape);
                int artDefID = DBProjectService.GetArtefactDefID(projectID, artDef.Name);
                if (artDefID<0) return -3;
                foreach(AttributeDefinition artAttribDef in artDef.AttributeDefinitions){
                    DBProjectService.AddArtefactAttributeDefinition(artAttribDef.Name,artAttribDef.Description,artAttribDef.Values,artDefID);
                    int artAttribDefID = DBProjectService.GetArtefactAttributeDefID(artDefID, artAttribDef.Name);
                    attribDefs.Add(artAttribDef,artAttribDefID);
                }

                artDefs.Add(artDef,artDefID);
            }

            // RELATIONSHIP DEFINITIONS
            Dictionary<RelationshipDefinition, int> relDefs = new Dictionary<RelationshipDefinition, int>();
            foreach (RelationshipDefinition relDef in project.RelationshipDefs){
                DBProjectService.AddRelationshipDefinition(projectID, relDef.Name, relDef.Description, relDef.Shape);
                int relDefID = DBProjectService.GetRelationshipDefID(projectID, relDef.Name);
                if (relDefID<0) return -4;
                foreach(AttributeDefinition artAttribDef in relDef.AttributeDefinitions){
                    DBProjectService.AddRelationshipAttributeDefinition(artAttribDef.Name,artAttribDef.Description,artAttribDef.Values, relDefID);
                    int artAttribDefID = DBProjectService.GetRelationshipAttributeDefID(relDefID, artAttribDef.Name);
                    attribDefs.Add(artAttribDef,artAttribDefID);
                }

                relDefs.Add(relDef,relDefID);
            }
            
            // ARTEFACTS
            Dictionary<Artefact, int> artefacts = new Dictionary<Artefact, int>();
            foreach (Artefact artefact in project.Artefacts){
                int artDefID = artDefs[artefact.Definition];
                DBProjectService.AddArtefact(artefact.Name, artefact.Description, artDefID);
                int artID = DBProjectService.GetArtefactID(projectID, artefact.Name);
                if (artID<0) return -5;
                foreach (Attribute attrib in artefact.Attributes){
                    int attributeDefID = attribDefs[attrib.Definition];
                    DBProjectService.AddArtefactAttribute(artID,attributeDefID,attrib.Value);
                }

                artefacts.Add(artefact,artID);
            }

            // RELATIONSHIPS
            foreach (Relationship relation in project.Relationships){
                int relDefID = relDefs[relation.Definition];
                int parentID = artefacts[relation.Parent];
                int childID = artefacts[relation.Child];
                DBProjectService.AddRelationship(relDefID,relation.Description, parentID,childID);
                int relID = DBProjectService.GetRelationshipID(relDefID, parentID, childID);
                if (relID<0) return -6;
                foreach (Attribute attrib in relation.Attributes){
                    int attributeDefID = attribDefs[attrib.Definition];
                    DBProjectService.AddRelationshipAttribute(relID, attributeDefID, attrib.Value);
                }
            }


            //VISUALIZATION TEMPLATES
            foreach (Visualization visual in project.Visualizations){
                DBProjectService.AddVisualizationTemplate(visual.Name,visual.Description,projectID);
                int visualID = DBProjectService.GetVisualizationID(projectID, visual.Name);
                if (visualID<0) return -7;

                //ARTEFACT COLOR FACTOR
                foreach (ColorFactor colorFactor in visual.ArtefactColorFactors){
                    int attribDefID = attribDefs[colorFactor.Definition];
                    DBProjectService.AddArtefactColorFactor(attribDefID, visualID, colorFactor.Weight);
                    int colorFactorID = DBProjectService.GetArtefactColorFactorID(visualID,attribDefID);
                    if (colorFactorID<0) return -8;
                    foreach (KeyValuePair<string, System.Drawing.Color> points in colorFactor.Values){
                        DBProjectService.AddArtefactColorFactorValue(colorFactorID, points.Key, points.Value.R, points.Value.G, points.Value.B, points.Value.A);
                    }
                }

                //RELATIONSHIP COLOR FACTOR
                foreach (ColorFactor colorFactor in visual.RelationshipColorFactors){
                    int attribDefID = attribDefs[colorFactor.Definition];
                    DBProjectService.AddArtefactColorFactor(attribDefID,visualID,colorFactor.Weight);
                    int colorFactorID = DBProjectService.GetRelationshipColorFactorID(visualID,attribDefID);
                    if (colorFactorID<0) return -9;
                    foreach (KeyValuePair<string, System.Drawing.Color> points in colorFactor.Values){
                        DBProjectService.AddRelationshipColorFactorValue(colorFactorID,points.Key,points.Value.R,points.Value.G,points.Value.B,points.Value.A);
                    }
                }
                
                //ARTEFACT SIZE FACTOR
                foreach (SizeFactor sizeFactor in visual.ArtefactSizeFactors){
                    int attribDefID = attribDefs[sizeFactor.Definition];
                    DBProjectService.AddArtefactSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = DBProjectService.GetArtefactSizeFactorID(visualID,attribDefID);
                    if (sizeFactorID<0) return -10;
                    foreach (KeyValuePair<string, int> points in sizeFactor.Values){
                        DBProjectService.AddArtefactSizeFactorValue(sizeFactorID,points.Key,points.Value);
                    }
                }

                //RELATIONSHIP SIZE FACTOR
                foreach (SizeFactor sizeFactor in visual.RelationshipSizeFactors){
                    int attribDefID = attribDefs[sizeFactor.Definition];
                    DBProjectService.AddRelationshipSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = DBProjectService.GetRelationshipSizeFactorID(visualID,attribDefID);
                    if (sizeFactorID<0) return -11;
                    foreach (KeyValuePair<string, int> points in sizeFactor.Values){
                        DBProjectService.AddRelationshipSizeFactorValue(sizeFactorID,points.Key,points.Value);
                    }
                }
            }

            foreach (HistoryEntry he in project.HistoryEntries){
                DBProjectService.AddProjectHistoryEntry(projectID, he.ElementType, he.ElementId, he.ChangeType, he.Changes, he.ChangeDate);
            }

            return 0;
        }


        static internal (int, Project) getUserProject(string account, int projectId){
            int userID = DBUserService.GetUserId(account);
            if (userID<0) return (-1, null);
            (int, Project) userProject = DBProjectService.GetUserProject(userID, projectId);
            if (userProject.Item1<0) return (-2, null);
            int projectID = userProject.Item1;
            Project theProject = userProject.Item2;
            theProject.ArtefactDefs = new List<ArtefactDefinition>();
            theProject.Artefacts = new List<Artefact>();
            theProject.RelationshipDefs = new List<RelationshipDefinition>();
            theProject.Relationships = new List<Relationship>();
            theProject.Visualizations = new List<Visualization>();

            Dictionary<int, Artefact> projectArtefacts = new Dictionary<int, Artefact>();
            Dictionary<int, AttributeDefinition> projectArtAttributeDefs = new Dictionary<int, AttributeDefinition>();
            Dictionary<int, AttributeDefinition> projectRelAttributeDefs = new Dictionary<int, AttributeDefinition>();

            List<(int, ArtefactDefinition)> projectArtDefs = DBProjectService.GetProjectArtefactDefs(projectID);
            foreach ((int, ArtefactDefinition) artDef in  projectArtDefs){

                Dictionary<int, AttributeDefinition> artAttribDefs = DBProjectService.GetArtefactDefAttributeDefs(artDef.Item1);
                artDef.Item2.AttributeDefinitions = new List<AttributeDefinition>();
                foreach (KeyValuePair<int, AttributeDefinition> attribDef in artAttribDefs){
                    projectArtAttributeDefs.Add(attribDef.Key,attribDef.Value);
                    artDef.Item2.AttributeDefinitions.Add(attribDef.Value);
                }
                List<(int, Artefact)> artefactsForDef = DBProjectService.GetArtefactsForDefinition(artDef.Item1);
                foreach((int, Artefact) artDefArtefact in artefactsForDef){
                    artDefArtefact.Item2.Definition = artDef.Item2;
                    artDefArtefact.Item2.Attributes = new List<Attribute>();
                    List<(Attribute, int)> artefactAttrs = DBProjectService.GetArtefactAttributes(artDefArtefact.Item1);
                    foreach ((Attribute, int) attr in artefactAttrs){
                        attr.Item1.Definition = artAttribDefs[attr.Item2];
                        artDefArtefact.Item2.Attributes.Add(attr.Item1);
                    }
                    projectArtefacts.Add(artDefArtefact.Item1,artDefArtefact.Item2);
                    theProject.Artefacts.Add(artDefArtefact.Item2);
                }
                theProject.ArtefactDefs.Add(artDef.Item2);
            }

            List<(int, RelationshipDefinition)> projectRelDefs = DBProjectService.GetProjectRelationshipDefs(projectID);
            foreach ((int, RelationshipDefinition) relDef in  projectRelDefs){

                Dictionary<int, AttributeDefinition> relAttribDefs = DBProjectService.GetRelationshipDefAttributeDefs(relDef.Item1);
                relDef.Item2.AttributeDefinitions = new List<AttributeDefinition>();
                foreach (KeyValuePair<int, AttributeDefinition> attribDef in relAttribDefs){
                    projectRelAttributeDefs.Add(attribDef.Key,attribDef.Value);
                    relDef.Item2.AttributeDefinitions.Add(attribDef.Value);
                }
                List<(int, (int,int,string,int))> relsForDef = DBProjectService.GetRelationshipsForDefinition(relDef.Item1);
                foreach ((int, (int,int,string,int)) relation in relsForDef){
                    Relationship rel = new Relationship();
                    rel.Definition = relDef.Item2;
                    rel.Parent = projectArtefacts[relation.Item2.Item1];
                    rel.Parent = projectArtefacts[relation.Item2.Item2];
                    rel.Description = relation.Item2.Item3;
                    rel.ID = relation.Item2.Item4;
                    rel.Attributes = new List<Attribute>();
                    List<(Attribute, int)> relationAttrs = DBProjectService.GetRelationshipAttributes(relation.Item1);
                    foreach ((Attribute, int) attr in relationAttrs){
                        attr.Item1.Definition = relAttribDefs[attr.Item2];
                        rel.Attributes.Add(attr.Item1);
                    }
                    theProject.Relationships.Add(rel);
                }

                theProject.RelationshipDefs.Add(relDef.Item2);
            }
            
            List<(int, Visualization)> projectVisualizations = DBProjectService.GetProjectVisualizations(projectID);
            foreach ((int, Visualization) vis in projectVisualizations){
                int visualID = vis.Item1;
                Visualization visual = vis.Item2;
                visual.ArtefactColorFactors = new List<ColorFactor>();
                visual.ArtefactSizeFactors = new List<SizeFactor>();
                visual.RelationshipColorFactors = new List<ColorFactor>();
                visual.RelationshipSizeFactors = new List<SizeFactor>();


                List<(int, (ColorFactor,int))> artColorFactors = DBProjectService.GetArtColorFactorsForVisual(visualID);
                foreach ((int, (ColorFactor,int)) cF in artColorFactors){
                    int colorFactorID = cF.Item1;
                    ColorFactor colorFactor = cF.Item2.Item1;
                    int attribDefID = cF.Item2.Item2;

                    colorFactor.Definition = projectArtAttributeDefs[attribDefID];
                    colorFactor.Values = DBProjectService.GetArtColorFactorValues(colorFactorID);

                    visual.ArtefactColorFactors.Add(colorFactor);
                }

                List<(int, (SizeFactor,int))> artSizeFactors = DBProjectService.GetArtSizeFactorsForVisual(visualID);
                foreach ((int, (SizeFactor,int)) sF in artSizeFactors){
                    int colorFactorID = sF.Item1;
                    SizeFactor sizeFactor = sF.Item2.Item1;
                    int attribDefID = sF.Item2.Item2;

                    sizeFactor.Definition = projectArtAttributeDefs[attribDefID];
                    sizeFactor.Values = DBProjectService.GetArtSizeFactorValues(colorFactorID);

                    visual.ArtefactSizeFactors.Add(sizeFactor);
                }

                
                List<(int, (ColorFactor,int))> relColorFactors = DBProjectService.GetRelColorFactorsForVisual(visualID);
                foreach ((int, (ColorFactor,int)) cF in relColorFactors){
                    int colorFactorID = cF.Item1;
                    ColorFactor colorFactor = cF.Item2.Item1;
                    int attribDefID = cF.Item2.Item2;

                    colorFactor.Definition = projectRelAttributeDefs[attribDefID];
                    colorFactor.Values = DBProjectService.GetRelColorFactorValues(colorFactorID);

                    visual.RelationshipColorFactors.Add(colorFactor);
                }

                List<(int, (SizeFactor,int))> relSizeFactors = DBProjectService.GetRelSizeFactorsForVisual(visualID);
                foreach ((int, (SizeFactor,int)) sF in relSizeFactors){
                    int colorFactorID = sF.Item1;
                    SizeFactor sizeFactor = sF.Item2.Item1;
                    int attribDefID = sF.Item2.Item2;

                    sizeFactor.Definition = projectRelAttributeDefs[attribDefID];
                    sizeFactor.Values = DBProjectService.GetRelSizeFactorValues(colorFactorID);

                    visual.RelationshipSizeFactors.Add(sizeFactor);
                }

                theProject.Visualizations.Add(visual);
            }
            theProject.HistoryEntries = DBProjectService.GetAllProjectHistoryEntries(projectID);
            return (0,theProject);
        }

        static internal int SaveProject(string userAccount, int projectID, Project updatedProject){
            int userID = DBUserService.GetUserId(userAccount);
            if (userID<0) return 1;
            (int, Project) userProject = DBProjectService.GetUserProject(userID, projectID);
            if (userProject.Item1<0) return 2;

            return 0;
        }

    }

}