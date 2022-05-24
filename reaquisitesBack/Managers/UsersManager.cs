using System.Net.Mail;
using reaquisites.Models;
using reaquisites.Services.DB;
using System.Security.Cryptography; 
using System.Text;
namespace reaquisites.Managers
{
    public static class UsersManager
    {
        const int registerStringsLength = 20;
        const int registerValidMinutes = 5;
        const int loggedStringsLenght = 20;
        static Dictionary<RegisterInfo,RegistrationTicket> registerTickets = new Dictionary<RegisterInfo, RegistrationTicket>();
        static Dictionary<string, User> loggedHash = new Dictionary<string, User>();
        static private string cString="";
        static internal string connString {
            get{return cString;} 
            set{
                cString = value;
                DBUserService.connString = value;
        }}

        public static void sendRegisterMessage(RegisterInfo user){
            string registerString = "";
            Random randy = new Random();
            for (int i=0; i<registerStringsLength; i++){
                int currentChar = randy.Next(0,AppConsts.Alphanumeric.Length-1);
                registerString = registerString + AppConsts.Alphanumeric[currentChar];
            }


            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("reaquisites@gmail.com"),
                Subject = "Reaquisites Email Verification",
                Body = "<h1>Hello <b>"+user.UserName+"</b>!</h1><br>"+
                "<h2>Thank you for registering in Reaquisites App!</h2><br>"+
                "<h3>Follow next link to complete register:</h3> "+AppConsts.AppBackRoute+"/user/register/"+registerString+"<br>"+
                "<h3>Link will be avaliable for 5 minutes.</h3>",
                IsBodyHtml = true
            };
            mailMessage.To.Add(user.UserEmail);
            SMTPManager.sendMailMessage(mailMessage);
            RegistrationTicket ticket = new RegistrationTicket(registerString,DateTime.Now);
            registerTickets[user] = ticket;
        }

        public static bool checkRegister(string registerString){
            foreach(RegisterInfo registerInfo in registerTickets.Keys){
                if (registerTickets[registerInfo].Ticket == registerString){
                    User newUser = new User();
                    newUser.Nick = registerInfo.UserName;
                    newUser.Account = registerInfo.AccountName;
                    newUser.EMail = registerInfo.UserEmail;
                    
                    //encrypt pass
                    string saltedPass = saltString(registerInfo.UserPassword, registerInfo.UserName);
                    string encodedPass = encodeString(saltedPass);
                    DBUserService.Add(newUser, encodedPass);
                    registerTickets.Remove(registerInfo);
                    return true;
                }
            }
            return false;
        }
        private static string saltString(string toSalt, string salt){
            string saltySalt = ""+salt[0]+salt[salt.Length-1];
            return  toSalt.Substring(0,toSalt.Length/2) + 
                saltySalt + toSalt.Substring(toSalt.Length/2);
        }

        private static string encodeString(string toEncode){
            SHA256 shaha = SHA256.Create();
            byte[] encodedBytes = shaha.ComputeHash(Encoding.UTF8.GetBytes(toEncode));
            StringBuilder sb = new StringBuilder();
            foreach (Byte b in encodedBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static void checkTickets(){
            foreach(RegisterInfo user in registerTickets.Keys){
                if (registerTickets[user].CreationDate - DateTime.Now > TimeSpan.FromMinutes(registerValidMinutes)){
                    registerTickets.Remove(user);
                }
            }
        }

        public static KeyValuePair<LoggedUserDTO,int> checkUserLogging(string logName, string logPass){
            List<KeyValuePair<User, int>> allUsers = DBUserService.GetAllWithID();
            User userFound = null;
            int userID = -1;
            foreach (KeyValuePair<User, int> user in allUsers){
                if (user.Key.Nick == logName || user.Key.EMail == logName){
                    userFound=user.Key;
                    userID = user.Value;
                    break;
                }
            }
            if (userFound!=null){
                string saltedPass = saltString(logPass,userFound.Nick);
                string encodedPass = encodeString(saltedPass);
                string userPass = DBUserService.GetUserPass(userID);
                if (userPass == encodedPass){
                    string logHash = "";
                    Random randy = new Random();
                    for (int i=0; i<loggedStringsLenght; i++){
                        logHash += AppConsts.Alphanumeric[randy.Next(AppConsts.Alphanumeric.Length)];
                    }
                    //GET USER PROJECTS
                    LoggedUserDTO userDTO = new LoggedUserDTO();
                    userDTO.Nick = userFound.Nick;
                    userDTO.Account = userFound.Account;
                    userDTO.EMail = userFound.EMail;
                    userDTO.RegisterDate = userFound.RegisterDate;
                    userDTO.LoginSession = logHash;
                    userDTO.Projects = DBProjectService.GetUserProjectsSimpleDTO(userID);
                    loggedHash.Add(logHash,userFound);
                    return new KeyValuePair<LoggedUserDTO, int>(userDTO, 0);
                }else{
                    return new KeyValuePair<LoggedUserDTO, int>(null, 1);
                }
            }else{
                return new KeyValuePair<LoggedUserDTO, int>(null, 0);
            }
        }

        public static bool checkSession(string accName, string session){
            if (loggedHash.ContainsKey(session)){
                return loggedHash[session].Account == accName;
            }else{
                return false;
            }
        }
        
    }

}