using reaquisites.Models;
using reaquisites.Services.DB;
using System.Text.Json;
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
            int projectRef = AddProjectToUser(accName, project);
            //si es menor que 0 el controller captura el error
            project.ProjectId = projectRef;
            return new KeyValuePair<int, Project>(projectRef, project);
        }
        
        static internal int AddProjectToUser(string userAccount, Project project)
        {
            int userID = DBUserService.GetUserId(userAccount);
            if (userID<0) return -1;
            DBProjectService.AddProject(userID, project.Name, project.Description,project.IsPublished, false, project.Version);
            int projectID = DBProjectService.GetLastProjectID(userID);
            if (projectID<0) return -2;

            List<(int, List<(string, int)>)> artDefAttribDefs = new List<(int, List<(string, int)>)>();
            List<(int, List<(string, int)>)> relDefAttribDefs = new List<(int, List<(string, int)>)>();
            
            // ARTEFACT DEFINITIONS
            Dictionary<ArtefactDefinition, int> artDefs = new Dictionary<ArtefactDefinition, int>();
            foreach (ArtefactDefinition artDef in project.ArtefactDefs){
                DBProjectService.AddArtefactDefinition(projectID,artDef.Name,artDef.Description,artDef.Shape,artDef.ID);
                int artDefID = DBProjectService.GetLastArtefactDefID(projectID);
                if (artDefID<0) return -3;
                List<(string, int)> attribs = new List<(string, int)>();
                foreach(AttributeDefinition artAttribDef in artDef.AttributeDefinitions){
                    DBProjectService.AddArtefactAttributeDefinition(artAttribDef.Name, artAttribDef.Type, artAttribDef.Description,artAttribDef.Values,artDefID);
                    int artAttribDefID = DBProjectService.GetArtefactAttributeDefID(artDefID, artAttribDef.Name);
                    attribs.Add((artAttribDef.Name, artAttribDefID));
                }
                artDefAttribDefs.Add((artDefID, attribs));
                artDefs.Add(artDef,artDefID);
            }

            // RELATIONSHIP DEFINITIONS
            Dictionary<RelationshipDefinition, int> relDefs = new Dictionary<RelationshipDefinition, int>();
            foreach (RelationshipDefinition relDef in project.RelationshipDefs){
                DBProjectService.AddRelationshipDefinition(projectID, relDef.Name, relDef.Description, relDef.Shape, relDef.ID);
                int relDefID = DBProjectService.GetLastRelationshipDefID(projectID);
                if (relDefID<0) return -4;
                List<(string, int)> attribs = new List<(string, int)>();
                foreach(AttributeDefinition artAttribDef in relDef.AttributeDefinitions){
                    DBProjectService.AddRelationshipAttributeDefinition(artAttribDef.Name, artAttribDef.Type, artAttribDef.Description,artAttribDef.Values, relDefID);
                    int artAttribDefID = DBProjectService.GetRelationshipAttributeDefID(relDefID, artAttribDef.Name);
                    attribs.Add((artAttribDef.Name, artAttribDefID));
                }
                relDefAttribDefs.Add((relDefID, attribs));
                relDefs.Add(relDef,relDefID);
            }
            
            // ARTEFACTS
            Dictionary<Artefact, int> artefacts = new Dictionary<Artefact, int>();
            foreach (Artefact artefact in project.Artefacts){
                int artDefID = artDefs[artefact.Definition];
                DBProjectService.AddArtefact(artefact.Name, artefact.Description, artDefID, artefact.ID);
                int artID = DBProjectService.GetLastArtefactID(projectID);
                if (artID<0) return -5;
                foreach (Attribute attrib in artefact.Attributes){
                    int attributeDefID = artDefAttribDefs.Where((attrs) =>
                        attrs.Item1 == artDefID).ElementAt(0).Item2.Find((attr) =>{ return attr.Item1 == attrib.Definition.Name;}).Item2;
                    DBProjectService.AddArtefactAttribute(artID,attributeDefID,attrib.Value);
                }

                artefacts.Add(artefact,artID);
            }

            // RELATIONSHIPS
            foreach (Relationship relation in project.Relationships){
                int relDefID = relDefs[relation.Definition];
                int parentID = artefacts[relation.Parent];
                int childID = artefacts[relation.Child];
                DBProjectService.AddRelationship(relDefID,relation.Description, parentID,childID, relation.ID);
                int relID = DBProjectService.GetLastRelationshipID(relDefID);
                if (relID<0) return -6;
                foreach (Attribute attrib in relation.Attributes){
                    int attributeDefID = relDefAttribDefs.Where((attrs) =>
                        attrs.Item1 == relDefID).ElementAt(0).Item2.Find((attr) =>{ return attr.Item1 == attrib.Definition.Name;}).Item2;
                    DBProjectService.AddRelationshipAttribute(relID, attributeDefID, attrib.Value);
                }
            }


            //VISUALIZATION TEMPLATES
            foreach (Visualization visual in project.Visualizations){
                DBProjectService.AddVisualizationTemplate(visual.Name,visual.Description,projectID, visual.ID);
                int visualID = DBProjectService.GetVisualizationID(projectID, visual.Name);
                if (visualID<0) return -7;

                //ARTEFACT COLOR FACTOR
                foreach (ArtefactColorFactor colorFactor in visual.ArtefactColorFactors){
                    int artDefID = DBProjectService.GetArtefactDefID(projectID, colorFactor.ElementDefinition.ID);
                    int attribDefID = artDefAttribDefs.Where((attrs) =>
                        attrs.Item1 == artDefID).ElementAt(0).Item2.Find((attr) =>{ return attr.Item1 == colorFactor.AttributeDefinition.Name;}).Item2;
                    DBProjectService.AddArtefactColorFactor(attribDefID, visualID, colorFactor.Weight);
                    int colorFactorID = DBProjectService.GetArtefactColorFactorID(visualID,attribDefID);
                    if (colorFactorID<0) return -8;
                    foreach ((string, (int, int, int)) points in colorFactor.Values){
                        DBProjectService.AddArtefactColorFactorValue(colorFactorID, points.Item1, (byte)points.Item2.Item1, (byte)points.Item2.Item2, (byte)points.Item2.Item3, 1);
                    }
                }

                //RELATIONSHIP COLOR FACTOR
                foreach (RelationshipColorFactor colorFactor in visual.RelationshipColorFactors){
                    int relDefID = DBProjectService.GetRelationshipDefID(projectID, colorFactor.ElementDefinition.ID);
                    int attribDefID = relDefAttribDefs.Where((attrs) =>
                        attrs.Item1 == relDefID).ElementAt(0).Item2.Find((attr) =>{ return attr.Item1 == colorFactor.AttributeDefinition.Name;}).Item2;
                    DBProjectService.AddRelationshipColorFactor(attribDefID,visualID,colorFactor.Weight);
                    int colorFactorID = DBProjectService.GetRelationshipColorFactorID(visualID,attribDefID);
                    if (colorFactorID<0) return -9;
                    foreach ((string, (int, int, int)) points in colorFactor.Values){
                        DBProjectService.AddRelationshipColorFactorValue(colorFactorID, points.Item1, (byte)points.Item2.Item1, (byte)points.Item2.Item2, (byte)points.Item2.Item3, 1);
                    }
                }
                
                //ARTEFACT SIZE FACTOR
                foreach (ArtefactSizeFactor sizeFactor in visual.ArtefactSizeFactors){
                    int artDefID = DBProjectService.GetArtefactDefID(projectID, sizeFactor.ElementDefinition.ID);
                    int attribDefID = artDefAttribDefs.Where((attrs) =>
                        attrs.Item1 == artDefID).ElementAt(0).Item2.Find((attr) =>{ return attr.Item1 == sizeFactor.AttributeDefinition.Name;}).Item2;
                    DBProjectService.AddArtefactSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = DBProjectService.GetArtefactSizeFactorID(visualID,attribDefID);
                    if (sizeFactorID<0) return -10;
                    foreach ((string, int) points in sizeFactor.Values){
                        DBProjectService.AddArtefactSizeFactorValue(sizeFactorID,points.Item1,points.Item2);
                    }
                }

                //RELATIONSHIP SIZE FACTOR
                foreach (RelationshipSizeFactor sizeFactor in visual.RelationshipSizeFactors){
                    int relDefID = DBProjectService.GetRelationshipDefID(projectID, sizeFactor.ElementDefinition.ID);
                    int attribDefID = relDefAttribDefs.Where((attrs) =>
                        attrs.Item1 == relDefID).ElementAt(0).Item2.Find((attr) =>{ return attr.Item1 == sizeFactor.AttributeDefinition.Name;}).Item2;
                    DBProjectService.AddRelationshipSizeFactor(attribDefID,visualID,sizeFactor.Weight);
                    int sizeFactorID = DBProjectService.GetRelationshipSizeFactorID(visualID,attribDefID);
                    if (sizeFactorID<0) return -11;
                    foreach ((string, int) points in sizeFactor.Values){
                        DBProjectService.AddRelationshipSizeFactorValue(sizeFactorID,points.Item1,points.Item2);
                    }
                }
            }

            foreach (HistoryEntry he in project.HistoryEntries){
                DBProjectService.AddProjectHistoryEntry(projectID, he.ElementType, he.ElementId, he.ChangeType, he.Changes, he.ChangeDate);
            }
            

            return DBProjectService.GetProjectRef(userID,projectID);
        }


        static internal (int, Project?) getUserProject(string account, int projectId){
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
            Dictionary<int, Dictionary<int, AttributeDefinition>> projectArtAttributeDefs = new Dictionary<int, Dictionary<int, AttributeDefinition>>();
            Dictionary<int, Dictionary<int, AttributeDefinition>> projectRelAttributeDefs = new Dictionary<int, Dictionary<int, AttributeDefinition>>();
            Dictionary<int, ArtefactDefinition> projectArtDefs = new Dictionary<int, ArtefactDefinition>();
            Dictionary<int, RelationshipDefinition> projectRelDefs = new Dictionary<int, RelationshipDefinition>();

            List<(int, ArtefactDefinition)> projectArtDefsFromDB = DBProjectService.GetProjectArtefactDefs(projectID);
            foreach ((int, ArtefactDefinition) artDef in  projectArtDefsFromDB){

                Dictionary<int, AttributeDefinition> artAttribDefs = DBProjectService.GetArtefactDefAttributeDefs(artDef.Item1);
                artDef.Item2.AttributeDefinitions = new List<AttributeDefinition>();
                projectArtAttributeDefs.Add(artDef.Item1, new Dictionary<int, AttributeDefinition>());
                foreach (KeyValuePair<int, AttributeDefinition> attribDef in artAttribDefs){
                    projectArtAttributeDefs[artDef.Item1].Add(attribDef.Key,attribDef.Value);
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
                projectArtDefs.Add(artDef.Item1,artDef.Item2);
            }

            List<(int, RelationshipDefinition)> projectRelDefsFromDB = DBProjectService.GetProjectRelationshipDefs(projectID);
            foreach ((int, RelationshipDefinition) relDef in  projectRelDefsFromDB){

                Dictionary<int, AttributeDefinition> relAttribDefs = DBProjectService.GetRelationshipDefAttributeDefs(relDef.Item1);
                relDef.Item2.AttributeDefinitions = new List<AttributeDefinition>();
                projectRelAttributeDefs.Add(relDef.Item1, new Dictionary<int, AttributeDefinition>());
                foreach (KeyValuePair<int, AttributeDefinition> attribDef in relAttribDefs){
                    projectRelAttributeDefs[relDef.Item1].Add(attribDef.Key,attribDef.Value);
                    relDef.Item2.AttributeDefinitions.Add(attribDef.Value);
                }
                List<(int, (int,int,string?,int))> relsForDef = DBProjectService.GetRelationshipsForDefinition(relDef.Item1);
                foreach ((int, (int,int,string?,int)) relation in relsForDef){
                    Relationship rel = new Relationship();
                    rel.Definition = relDef.Item2;
                    rel.Parent = projectArtefacts[relation.Item2.Item1];
                    rel.Child = projectArtefacts[relation.Item2.Item2];
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
                projectRelDefs.Add(relDef.Item1, relDef.Item2);
            }
            
            List<(int, Visualization)> projectVisualizations = DBProjectService.GetProjectVisualizations(projectID);
            foreach ((int, Visualization) vis in projectVisualizations){
                int visualID = vis.Item1;
                Visualization visual = vis.Item2;
                visual.ArtefactColorFactors = new List<ArtefactColorFactor>();
                visual.ArtefactSizeFactors = new List<ArtefactSizeFactor>();
                visual.RelationshipColorFactors = new List<RelationshipColorFactor>();
                visual.RelationshipSizeFactors = new List<RelationshipSizeFactor>();


                List<(int, (ArtefactColorFactor,int))> artColorFactors = DBProjectService.GetArtColorFactorsForVisual(visualID);
                foreach ((int, (ArtefactColorFactor,int)) cF in artColorFactors){
                    int colorFactorID = cF.Item1;
                    ArtefactColorFactor colorFactor = cF.Item2.Item1;
                    int attribDefID = cF.Item2.Item2;
                    foreach (int artDefID in projectArtAttributeDefs.Keys){
                        if (projectArtAttributeDefs[artDefID][attribDefID] != null){
                            colorFactor.ElementDefinition = projectArtDefs[artDefID];
                            colorFactor.AttributeDefinition = projectArtAttributeDefs[artDefID][attribDefID];
                            break;
                        }
                    }
                    colorFactor.Values = DBProjectService.GetArtColorFactorValues(colorFactorID);

                    visual.ArtefactColorFactors.Add(colorFactor);
                }

                List<(int, (ArtefactSizeFactor,int))> artSizeFactors = DBProjectService.GetArtSizeFactorsForVisual(visualID);
                foreach ((int, (ArtefactSizeFactor,int)) sF in artSizeFactors){
                    int colorFactorID = sF.Item1;
                    ArtefactSizeFactor sizeFactor = sF.Item2.Item1;
                    int attribDefID = sF.Item2.Item2;

                    foreach (int artDefID in projectArtAttributeDefs.Keys){
                        if (projectArtAttributeDefs[artDefID][attribDefID] != null){
                            sizeFactor.ElementDefinition = projectArtDefs[artDefID];
                            sizeFactor.AttributeDefinition = projectArtAttributeDefs[artDefID][attribDefID];
                            break;
                        }
                    }
                    sizeFactor.Values = DBProjectService.GetArtSizeFactorValues(colorFactorID);

                    visual.ArtefactSizeFactors.Add(sizeFactor);
                }

                
                List<(int, (RelationshipColorFactor,int))> relColorFactors = DBProjectService.GetRelColorFactorsForVisual(visualID);
                foreach ((int, (RelationshipColorFactor,int)) cF in relColorFactors){
                    int colorFactorID = cF.Item1;
                    RelationshipColorFactor colorFactor = cF.Item2.Item1;
                    int attribDefID = cF.Item2.Item2;

                    foreach (int relDefID in projectRelAttributeDefs.Keys){
                        if (projectRelAttributeDefs[relDefID][attribDefID] != null){
                            colorFactor.ElementDefinition = projectRelDefs[relDefID];
                            colorFactor.AttributeDefinition = projectRelAttributeDefs[relDefID][attribDefID];
                            break;
                        }
                    }
                    colorFactor.Values = DBProjectService.GetRelColorFactorValues(colorFactorID);

                    visual.RelationshipColorFactors.Add(colorFactor);
                }

                List<(int, (RelationshipSizeFactor,int))> relSizeFactors = DBProjectService.GetRelSizeFactorsForVisual(visualID);
                foreach ((int, (RelationshipSizeFactor,int)) sF in relSizeFactors){
                    int colorFactorID = sF.Item1;
                    RelationshipSizeFactor sizeFactor = sF.Item2.Item1;
                    int attribDefID = sF.Item2.Item2;

                    foreach (int relDefID in projectRelAttributeDefs.Keys){
                        if (projectRelAttributeDefs[relDefID][attribDefID] != null){
                            sizeFactor.ElementDefinition = projectRelDefs[relDefID];
                            sizeFactor.AttributeDefinition = projectRelAttributeDefs[relDefID][attribDefID];
                            break;
                        }
                    }
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

            List<HistoryEntry> oldHEs = DBProjectService.GetAllProjectHistoryEntries(userProject.Item1);
            List<HistoryEntry> newHEs = updatedProject.HistoryEntries.Except(oldHEs).ToList();
            //cogemos la última entrada de las nuevas para cada elemento
            List<HistoryEntry> editedElems = newHEs.GroupBy(he => (he.ElementType, he.ElementId)).Select(
                heGroup => heGroup.OrderByDescending(he => he.ChangeDate).First()).ToList();
            editedElems.Sort((he1,he2) => {
                if (he1.ChangeType==he2.ChangeType) return 0;
                else if (he1.ChangeType>he2.ChangeType) return -1;
                else return 1;
            });
            foreach (HistoryEntry lastHE in editedElems){
                if (lastHE.ChangeType<3){
                    if (lastHE.ChangeType==1){
                        //CREATE
                        switch (lastHE.ElementType){
                            case 1:
                                //ARTEFACT DEF
                                ArtefactDefinition artDefToCreate = 
                                JsonSerializer.Deserialize<ArtefactDefinition>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                DBProjectService.AddArtefactDefinition(userProject.Item1, artDefToCreate.Name, artDefToCreate.Description, artDefToCreate.Shape, artDefToCreate.ID);
                                int artDefID = DBProjectService.GetLastArtefactDefID(userProject.Item1);
                                foreach (AttributeDefinition attributeDef in artDefToCreate.AttributeDefinitions){
                                    DBProjectService.AddArtefactAttributeDefinition(attributeDef.Name, 
                                    attributeDef.Type, attributeDef.Description, attributeDef.Values, artDefID);
                                }
                                break;
                            case 2:
                                //RELATIONSHIP DEF
                                RelationshipDefinition relDefToCreate = 
                                JsonSerializer.Deserialize<RelationshipDefinition>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                DBProjectService.AddRelationshipDefinition(userProject.Item1, relDefToCreate.Name, relDefToCreate.Description, relDefToCreate.Shape, relDefToCreate.ID);
                                int relDefID = DBProjectService.GetLastRelationshipDefID(userProject.Item1);
                                foreach (AttributeDefinition attributeDef in relDefToCreate.AttributeDefinitions){
                                    DBProjectService.AddRelationshipAttributeDefinition(attributeDef.Name, 
                                    attributeDef.Type, attributeDef.Description, attributeDef.Values, relDefID);
                                }
                                break;
                            case 3:
                                //ARTEFACT
                                Artefact artefactToCreate = 
                                JsonSerializer.Deserialize<Artefact>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                int artDefId = DBProjectService.GetArtefactDefID(userProject.Item1, artefactToCreate.Definition.ID);
                                DBProjectService.AddArtefact(artefactToCreate.Name, artefactToCreate.Description, artDefId, artefactToCreate.ID);
                                int artID = DBProjectService.GetLastArtefactID(artDefId);
                                foreach (Attribute attrib in artefactToCreate.Attributes){
                                    int attribDefID = DBProjectService.GetArtefactAttributeDefID(artDefId, attrib.Definition.Name);
                                    DBProjectService.AddArtefactAttribute(artID,attribDefID,attrib.Value);
                                }
                                break;
                            case 4:
                                //RELATIONSHIP
                                Relationship relationshipToCreate = 
                                JsonSerializer.Deserialize<Relationship>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                int relDefId = DBProjectService.GetRelationshipDefID(userProject.Item1, relationshipToCreate.Definition.ID);
                                int artDefChildId = DBProjectService.GetArtefactDefID(userProject.Item1, relationshipToCreate.Child.Definition.ID);
                                int childID = DBProjectService.GetArtefactID(artDefChildId, relationshipToCreate.Child.ID);
                                int artDefParentId = DBProjectService.GetArtefactDefID(userProject.Item1, relationshipToCreate.Parent.Definition.ID);
                                int parentID = DBProjectService.GetArtefactID(artDefParentId, relationshipToCreate.Parent.ID);
                                DBProjectService.AddRelationship(relDefId,relationshipToCreate.Description, parentID, childID, relationshipToCreate.ID);
                                int relID = DBProjectService.GetLastRelationshipID(relDefId);
                                foreach (Attribute attrib in relationshipToCreate.Attributes){
                                    int attribDefID = DBProjectService.GetRelationshipAttributeDefID(relDefId, attrib.Definition.Name);
                                    DBProjectService.AddRelationshipAttribute(relID, attribDefID, attrib.Value);
                                }
                                break;
                            case 5:
                                //VISUALIZATION
                                break;
                            default:
                                break;
                        }
                    }else{
                        //UPDATE
                        switch (lastHE.ElementType){
                            case 1:
                                //ARTEFACT DEF
                                int artDefID = DBProjectService.GetArtefactDefID(userProject.Item1,lastHE.ElementId);
                                ModifiedElementDTO<ArtefactDefinition> artDefMod = 
                                JsonSerializer.Deserialize<ModifiedElementDTO<ArtefactDefinition>>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                ArtefactDefinition artDefToUpdate = artDefMod.New;
                                if (artDefID>0){
                                    //EXISTE (MODIFICAMOS)
                                    DBProjectService.UpdateArtDefinition(artDefID,artDefToUpdate.Name,artDefToUpdate.Description,artDefToUpdate.Shape);
                                    //restauramos los atributos
                                    DBProjectService.DeleteAllArtDefAttributeDefs(artDefID);
                                    List<(int, Artefact)> artDefArtefacts = DBProjectService.GetArtefactsForDefinition(artDefID);
                                    foreach (AttributeDefinition attributeDef in artDefToUpdate.AttributeDefinitions){
                                        DBProjectService.AddArtefactAttributeDefinition(attributeDef.Name, 
                                        attributeDef.Type, attributeDef.Description, attributeDef.Values, artDefID);
                                        //ADD DEFAULT VALUES TO RELATED ARTEFACTS
                                        int attribDefId = DBProjectService.GetArtefactAttributeDefID(artDefID, attributeDef.Name);
                                        string value = "";
                                        switch (attributeDef.Type){
                                            case 2:
                                                break;
                                            default:
                                                value = attributeDef.Values.Substring(1).Split(',')[0];
                                                break;
                                        }
                                        foreach ((int, Artefact) artDefArtID in artDefArtefacts){
                                            DBProjectService.AddArtefactAttribute(artDefArtID.Item1, attribDefId,value);
                                        }
                                    }
                                }else{
                                    //NO EXISTE (CREAMOS)
                                    DBProjectService.AddArtefactDefinition(userProject.Item1, artDefToUpdate.Name, artDefToUpdate.Description, artDefToUpdate.Shape, artDefToUpdate.ID);
                                    int newArtDefID = DBProjectService.GetLastArtefactDefID(userProject.Item1);
                                    foreach (AttributeDefinition attributeDef in artDefToUpdate.AttributeDefinitions){
                                        DBProjectService.AddArtefactAttributeDefinition(attributeDef.Name, 
                                        attributeDef.Type, attributeDef.Description, attributeDef.Values, newArtDefID);
                                    }
                                }
                                break;
                            case 2:
                                //RELATIONSHIP DEF
                                int relDefID = DBProjectService.GetRelationshipDefID(userProject.Item1,lastHE.ElementId);
                                ModifiedElementDTO<RelationshipDefinition> relDefMod = 
                                    JsonSerializer.Deserialize<ModifiedElementDTO<RelationshipDefinition>>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                RelationshipDefinition relDefToUpdate = relDefMod.New;
                                if (relDefID>0){
                                    //EXISTE (MODIFICAMOS)
                                    DBProjectService.UpdateRelDefinition(relDefID,relDefToUpdate.Name,relDefToUpdate.Description,relDefToUpdate.Shape);
                                    //restauramos los atributos
                                    DBProjectService.DeleteAllRelDefAttributeDefs(relDefID);
                                    List<(int, (int, int, string?, int))> relDefRelations = DBProjectService.GetRelationshipsForDefinition(relDefID);
                                    foreach (AttributeDefinition attributeDef in relDefToUpdate.AttributeDefinitions){
                                        DBProjectService.AddRelationshipAttributeDefinition(attributeDef.Name, 
                                        attributeDef.Type, attributeDef.Description, attributeDef.Values, relDefID);
                                        //ADD DEFAULT VALUES TO RELATED RELATIONSHIPS
                                        int attribDefId = DBProjectService.GetRelationshipAttributeDefID(relDefID, attributeDef.Name);
                                        string value = "";
                                        switch (attributeDef.Type){
                                            case 2:
                                                break;
                                            default:
                                                value = "0";
                                                break;
                                        }
                                        foreach ((int, (int, int, string?, int)) artDefArtID in relDefRelations){
                                            DBProjectService.AddArtefactAttribute(artDefArtID.Item1, attribDefId,value);
                                        }
                                    }
                                }else{
                                    //NO EXISTE (CREAMOS)
                                    DBProjectService.AddRelationshipDefinition(userProject.Item1, relDefToUpdate.Name, relDefToUpdate.Description, relDefToUpdate.Shape, relDefToUpdate.ID);
                                    int newRelDefID = DBProjectService.GetLastRelationshipDefID(userProject.Item1);
                                    foreach (AttributeDefinition attributeDef in relDefToUpdate.AttributeDefinitions){
                                        DBProjectService.AddArtefactAttributeDefinition(attributeDef.Name, 
                                        attributeDef.Type, attributeDef.Description, attributeDef.Values, newRelDefID);
                                    }
                                }
                                break;
                            case 3:
                                //ARTEFACT
                                ModifiedElementDTO<Artefact> artMod = 
                                JsonSerializer.Deserialize<ModifiedElementDTO<Artefact>>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                Artefact artToUpdate = artMod.New;
                                int oldArtefactArtDefID = DBProjectService.GetArtefactDefID(userProject.Item1,artMod.Old.Definition.ID);
                                if (oldArtefactArtDefID<0) return 3;
                                int newArtefactArtDefID = DBProjectService.GetArtefactDefID(userProject.Item1,artToUpdate.Definition.ID);
                                if (newArtefactArtDefID<0) return 4;
                                int artID = DBProjectService.GetArtefactID(oldArtefactArtDefID,lastHE.ElementId);
                                if (artID>0){
                                    //EXISTE (MODIFICAMOS)
                                    DBProjectService.UpdateArtefact(artID,artToUpdate.Name,artToUpdate.Description,newArtefactArtDefID);
                                    if (oldArtefactArtDefID == newArtefactArtDefID){
                                        //si no ha cambiado el tipo de artefacto updateamos los valores de sus atributos
                                        foreach (Attribute attribute in artToUpdate.Attributes){
                                            int attributeDefID =  DBProjectService.GetArtefactAttributeDefID(oldArtefactArtDefID, attribute.Definition.Name);
                                            DBProjectService.UpdateArtefactAttributeValue(attributeDefID,artID, attribute.Value);
                                        }
                                    }else{
                                        //si ha cambiado el tipo de artefacto dropeamos todos los atributos y los creamos de nuevo
                                        DBProjectService.DeleteAllArtAttributes(artID);
                                        foreach (Attribute attribute in artToUpdate.Attributes){
                                            int attribDefID = DBProjectService.GetArtefactAttributeDefID(artID, attribute.Definition.Name);
                                            DBProjectService.AddArtefactAttribute(artID, attribDefID, attribute.Value);
                                        }
                                    }
                                }else{
                                    //NO EXISTE (CREAMOS)
                                    DBProjectService.AddArtefact(artToUpdate.Name,artToUpdate.Description, newArtefactArtDefID, artToUpdate.ID);
                                    foreach (Attribute attribute in artToUpdate.Attributes){
                                        int attribDefID = DBProjectService.GetArtefactAttributeDefID(artID, attribute.Definition.Name);
                                        DBProjectService.AddArtefactAttribute(artID, attribDefID, attribute.Value);
                                    }
                                }
                                break;
                            case 4:
                                //RELATIONSHIP
                                ModifiedElementDTO<Relationship> relMod = 
                                JsonSerializer.Deserialize<ModifiedElementDTO<Relationship>>(lastHE.Changes, new JsonSerializerOptions 
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });
                                Relationship relToUpdate = relMod.New;

                                int oldRelationshipRelDefID = DBProjectService.GetRelationshipDefID(userProject.Item1,relMod.Old.Definition.ID);
                                if (oldRelationshipRelDefID<0) return 5;
                                int newRelationshipRelDefID = DBProjectService.GetRelationshipDefID(userProject.Item1,relToUpdate.Definition.ID);
                                if (newRelationshipRelDefID<0) return 6;
                                
                                int newParentDefID = DBProjectService.GetArtefactDefID(userProject.Item1, relToUpdate.Parent.Definition.ID);
                                if (newParentDefID<0) return 7;
                                int newParentID = DBProjectService.GetArtefactID(newParentDefID, relToUpdate.Parent.ID);
                                if (newParentID<0) return 8;

                                int newChildDefID = DBProjectService.GetArtefactDefID(userProject.Item1, relToUpdate.Child.Definition.ID);
                                if (newChildDefID<0) return 9;
                                int newChildID = DBProjectService.GetArtefactID(newChildDefID, relToUpdate.Child.ID);
                                if (newChildID<0) return 10;

                                int relID = DBProjectService.GetRelationshipID(oldRelationshipRelDefID, lastHE.ElementId);
                                if (relID>0){
                                    //EXISTE (MODIFICAMOS)
                                    DBProjectService.UpdateRelationship(relID,newRelationshipRelDefID,relToUpdate.Description,newParentID,newChildID);
                                    if (oldRelationshipRelDefID == newRelationshipRelDefID){
                                        //si no ha cambiado el tipo de artefacto updateamos los valores de sus atributos
                                        foreach (Attribute attribute in relToUpdate.Attributes){
                                            int attributeDefID =  DBProjectService.GetRelationshipAttributeDefID(oldRelationshipRelDefID, attribute.Definition.Name);
                                            DBProjectService.UpdateRelationshipAttributeValue(attributeDefID,relID, attribute.Value);
                                        }
                                    }else{
                                        //si ha cambiado el tipo de artefacto dropeamos todos los atributos y los creamos de nuevo
                                        DBProjectService.DeleteAllRelAttributes(relID);
                                        foreach (Attribute attribute in relToUpdate.Attributes){
                                            int attribDefID = DBProjectService.GetRelationshipAttributeDefID(relID, attribute.Definition.Name);
                                            DBProjectService.AddRelationshipAttribute(relID, attribDefID, attribute.Value);
                                        }
                                    }
                                }else{
                                    //NO EXISTE (CREAMOS)
                                    DBProjectService.AddRelationship(newRelationshipRelDefID, relToUpdate.Description, newParentID, newChildID, relToUpdate.ID);
                                    foreach (Attribute attribute in relToUpdate.Attributes){
                                            int attribDefID = DBProjectService.GetRelationshipAttributeDefID(relID, attribute.Definition.Name);
                                            DBProjectService.AddRelationshipAttribute(relID, attribDefID, attribute.Value);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }else{
                    //DELETE
                    switch (lastHE.ElementType){
                        //lo borramos se haya creado o no
                        case 1:
                            //ARTEFACT DEF
                            DBProjectService.DeleteArtDefinition(userProject.Item1,lastHE.ElementId);
                            break;
                        case 2:
                            //RELATIONSHIP DEF
                            DBProjectService.DeleteRelDefinition(userProject.Item1,lastHE.ElementId);
                            break;
                        case 3:
                            //ARTEFACT
                            Artefact artToDelete = JsonSerializer.Deserialize<Artefact>(lastHE.Changes, new JsonSerializerOptions 
                                {
                                    PropertyNameCaseInsensitive = true
                                }
                            );
                            int artDefID = DBProjectService.GetArtefactDefID(userProject.Item1,artToDelete.Definition.ID);
                            DBProjectService.DeleteArtefact(artDefID,lastHE.ElementId);
                            break;
                        case 4:
                            //RELATIONSHIP
                            Relationship relToDelete = JsonSerializer.Deserialize<Relationship>(lastHE.Changes, new JsonSerializerOptions 
                                {
                                    PropertyNameCaseInsensitive = true
                                }
                            );
                            int relDefID = DBProjectService.GetArtefactDefID(userProject.Item1,relToDelete.Definition.ID);
                            DBProjectService.DeleteRelationship(relDefID,lastHE.ElementId);
                            break;
                        default:
                            break;
                    }
                }
            }
            foreach(HistoryEntry he in newHEs){
                DBProjectService.AddProjectHistoryEntry(userProject.Item1, he.ElementType, he.ElementId, he.ChangeType, he.Changes, he.ChangeDate);
            }
            return 0;
        }

    }

}