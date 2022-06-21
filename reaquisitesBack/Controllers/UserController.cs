using reaquisites.Models;
using reaquisites.Managers;
using reaquisites.Services.DB;
using Microsoft.AspNetCore.Mvc;

namespace reaquisites.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(IConfiguration configuration)
        {
            string BBDDstring = configuration.GetConnectionString("DefaultConnection");
            UsersManager.connString = ProjectManager.connString = BBDDstring;
        }

        // GET actions
        [HttpGet]
        public ActionResult<List<User>> GetAll() => DBUserService.GetAll();

        [HttpGet("Register/{reg}")] //we use path param to be able to generate urls for emails
        public RedirectResult CheckRegister(string reg)
        {
            if (UsersManager.checkRegister(reg)){
                return new RedirectResult(AppConsts.AppFrontRoute+"/register-acc");
            }else{
                return new RedirectResult(AppConsts.AppFrontRoute+"/register-exp");

            }
        }

        // GET action
        [HttpGet("{accountName}/projects")]
        public ActionResult<List<SimpleProjectDTO>> GetProjectsPreview(string accountName)
        {
            KeyValuePair<int,List<SimpleProjectDTO>> result = UsersManager.allUserProjectsPreview(accountName);
            if (result.Key>0){
                return result.Value;
            }else{
                return BadRequest("User not found");
            }
        }

        
        // POST action
        [HttpPost("EmailVer")]
        public void SendConfEmail([FromBody] RegisterInfoDTO user)
        {
            UsersManager.sendRegisterMessage(new RegisterInfo(user));
        }

        // POST action
        [HttpPost("Auth")]
        public ActionResult<LoggedUserDTO> UserAuth([FromBody] LogginInfoDTO loggin)
        {
            KeyValuePair<LoggedUserDTO, int> loggedUser = UsersManager.checkUserLogging(loggin.logName, loggin.logPass);
            if (loggedUser.Key!=null){
                return loggedUser.Key;
            }else{
                switch (loggedUser.Value){
                    case 0:
                        return ARFactory.createJSONErrorResult(loggedUser.Value, "Provided password not matching");
                    case 1: 
                        return ARFactory.createJSONErrorResult(loggedUser.Value, "Provided password not matching");
                    default:
                        return ARFactory.createJSONErrorResult(loggedUser.Value, "UNKOWN SERVER ERROR");

                }
            }
        }



        // PUT action

        // DELETE action
    }
}