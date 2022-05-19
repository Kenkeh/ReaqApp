using reaquisites.Models;
using reaquisites.Managers;
using reaquisites.Services.DB;
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
        }

        // POST action
        [HttpPost("{accName}/Add")]
        public ActionResult AddNewProject(string accName, [FromBody] NewProjectDTO newProject)
        {
            if (!UsersManager.checkSession(accName,newProject.loginSession))
                return ARFactory.createJSONMessageResult("Login session not found, please log into the application");
            Project project = new Project();
            project.Name = newProject.projectName;
            project.Description = newProject.projectDesc;
            project.Version = "0";
            int result = ProjectManager.addNewProject(accName, project);
            if (result<0){
                if (result == -1)
                    return ARFactory.createJSONMessageResult("User couldn't be found on database");
                else
                    return ARFactory.createJSONMessageResult("There was a problem connecting to the database");

            }else{
                return ARFactory.createJSONMessageResult("Project succesfully created");
            }
        }
        // PUT action

        // DELETE action
    }
}