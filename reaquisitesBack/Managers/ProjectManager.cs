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
            project.IsPublished = project.IsTemplate = false;
            project.Visualizations = new List<Visualization>();
            project.HistoryEntries = new List<HistoryEntry>();
            project.HistoryEntries.Add(HEFactory.createProjectCreationEntry(project));
            return new KeyValuePair<int, Project>(DBProjectService.Add(accName, project), project);
        }
    }

}