using reaquisites.Models;
using reaquisites.Managers;
using Microsoft.AspNetCore.Mvc;

namespace reaquisites.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        public ProjectController(IConfiguration configuration)
        {
            ProjectManager.connString = configuration.GetConnectionString("DefaultConnection");
            UsersManager.connString = configuration.GetConnectionString("DefaultConnection");
        }

        

        // GET action
        [HttpGet("{accName}/Get/{projectId}")] 
        public ActionResult<Project> GetProjectFromUser(string accName, int projectId/*, [FromBody] string loginSession*/)
        {
            // USE TO ENABLE LOGGIN SESSION
            //if (!UsersManager.checkSession(accName, loginSession))
            //    return ARFactory.createJSONErrorResult(0,"Login session not found, please log into the application");
            
            (int, Project) theProject = ProjectManager.getUserProject(accName,projectId);
            if (theProject.Item1<0){
                switch (theProject.Item1){
                    case -1:
                        return ARFactory.createJSONErrorResult(theProject.Item1, "User couldn't be found on database");
                    case -2:
                        return ARFactory.createJSONErrorResult(theProject.Item1, "Project couldn't be found on database");
                    default:
                        return ARFactory.createJSONErrorResult(theProject.Item1, "There was a problem connecting to the database");
                }
            }else{
                return theProject.Item2;
            }
        }
        // POST action
        [HttpPost("{accName}/Add")]
        public ActionResult<Project> AddNewProject(string accName, [FromBody] NewProjectDTO newProject)
        {
            // USE TO ENABLE LOGGIN SESSION
            //if (!UsersManager.checkSession(accName,newProject.loginSession))
            //    return ARFactory.createJSONErrorResult(0,"Login session not found, please log into the application");
            KeyValuePair<int, Project> result = ProjectManager.addNewProject(accName, newProject);
            if (result.Key<0){
                if (result.Key == -1)
                    return ARFactory.createJSONErrorResult(result.Key,"User couldn't be found on database");
                else
                    return ARFactory.createJSONErrorResult(result.Key,"There was a problem connecting to the database");

            }else{
                return result.Value;
            }
        }
        // POST action
        [HttpPost("{accName}/Update/{projectID}")]
        public ActionResult SaveProject(string accName, int projectID, [FromBody] Project toUpdate)
        {
            // USE TO ENABLE LOGGIN SESSION
            //if (!UsersManager.checkSession(accName,newProject.loginSession))
            //    return ARFactory.createJSONErrorResult(0,"Login session not found, please log into the application");
            
            switch(ProjectManager.SaveProject(accName, projectID, toUpdate)){
                case 0:
                    return Ok();
                case 1:
                    return BadRequest("User not found");
                case 2:
                    return BadRequest("Project not found");
                case 3:
                    return BadRequest("Artefact edited from an Artefact Definition not found");
                case 4:
                    return BadRequest("Artefact edited to an Artefact Definition not found");
                case 5:
                    return BadRequest("Relationship edited from an Relationship Definition not found");
                case 6:
                    return BadRequest("Relationship edited to an Relationship Definition not found");
                case 7:
                    return BadRequest("Relationship parent implicated edited to an Artefact from an Artefact Definition not found");
                case 8:
                    return BadRequest("Relationship parent implicated edited to an Artefact not found");
                case 9:
                    return BadRequest("Relationship child implicated edited to an Artefact from an Artefact Definition not found");
                case 10:
                    return BadRequest("Relationship child implicated edited to an Artefact not found");
                default:
                    return BadRequest();
            }
        }
        // PUT action

        // DELETE action
    }
}