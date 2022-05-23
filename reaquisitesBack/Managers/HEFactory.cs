using reaquisites.Models;
public static class HEFactory {

    public static HistoryEntry createProjectCreationEntry(Project project){
        HistoryEntry he = new HistoryEntry();
        he.Type = 1;
        he.Changes = "Project created with name: \""+project.Name+"\" and description: \""+project.Description+"\"";
        he.ChangeDate = DateTime.Now;
        return he;
    }
}