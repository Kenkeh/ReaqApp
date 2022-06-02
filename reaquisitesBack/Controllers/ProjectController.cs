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

        // POST action
        [HttpGet("{accName}/Get/{projectName}")] //we use path param to be able to generate urls for emails
        public ActionResult<Project> GetProjectFromUser(string accName, string projectName/*, [FromBody] string loginSession*/)
        {
            // USE TO ENABLE LOGGIN SESSION
            //if (!UsersManager.checkSession(accName, loginSession))
            //    return ARFactory.createJSONErrorResult(0,"Login session not found, please log into the application");
            projectName = projectName.Replace('_',' ');
            (int, Project) theProject = ProjectManager.getUserProject(accName,projectName);
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
        // PUT action

        // DELETE action
    }
}