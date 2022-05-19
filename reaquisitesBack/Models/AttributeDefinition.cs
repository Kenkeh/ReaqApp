
public class AttributeDefinition{
    public string Name { get; set; }
    public int Type { get; set; }
    public string Description { get; set; }
    public string Values { get; set; }
    public List<ColorFactor> ColorFactors { get; set; }
    public List<SizeFactor> SizeFactors { get; set; }
    public List<HistoryEntry> HistoryEntries { get; set; }

    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            AttributeDefinition toCompare = (AttributeDefinition)obj;
                return Name == toCompare.Name;
        }
    }
        
    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}