public class Relationship{
    public RelationshipDefinition Definition { get; set; }
    public Artefact Parent { get; set; }
    public Artefact Child { get; set; }
    public List<Attribute> Attributes { get; set; }
    public List<HistoryEntry> HistoryEntries { get; set; }

    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            Relationship toCompare = (Relationship)obj;
            return Parent == toCompare.Parent && Child == toCompare.Child && Definition == toCompare.Definition;
        }
    }
        
    public override int GetHashCode()
    {
        return HashCode.Combine(Definition,Parent,Child);
    }
}