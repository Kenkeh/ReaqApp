

namespace reaquisites.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public int ProjectId { get; set; }
        public List<ArtefactDefinition> ArtefactDefs { get; set; }
        public List<Artefact> Artefacts { get; set; }
        public List<RelationshipDefinition> RelationshipDefs { get; set; }
        public List<Relationship> Relationships { get; set; }
        public List<HistoryEntry> HistoryEntries { get; set; }
        public List<Visualization> Visualizations { get; set; }

        public override bool Equals(Object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                Project toCompare = (Project)obj;
                return Name == toCompare.Name;
            }
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}