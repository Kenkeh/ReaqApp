public class ArtefactDefinition{
    public int ID { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Shape { get; set; }
    public List<AttributeDefinition> AttributeDefinitions { get; set; }    public List<HistoryEntry> HistoryEntries { get; set; }
    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            ArtefactDefinition toCompare = (ArtefactDefinition)obj;
                return Name == toCompare.Name && Shape == toCompare.Shape;
        }
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}