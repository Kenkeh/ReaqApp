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
        
        static internal int addNewProject(string accName, Project newProject){
            return DBProjectService.Add(accName, newProject);
        }
    }

}