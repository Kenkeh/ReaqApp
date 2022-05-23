
using reaquisites.Models;
using Microsoft.AspNetCore.Mvc;
public class ARFactory {
    public static JsonResult createJSONErrorResult(int id){
        return new JsonResult(new { error = id});
    }
    public static JsonResult createJSONMessageResult(string msg){
        return new JsonResult(new { message = msg});
    }
    public static JsonResult createJSONProjectResult(Project project){
        return new JsonResult(new { 
            name = project.Name,
            version = project.Version,
            description = project.Description,
            artDefs = project.ArtefactDefs,
            relDefs = project.RelationshipDefs,
            artefacts = project.Artefacts,
            relationships = project.Relationships,
            published = project.IsPublished
            });
    }
}