public class Attribute {
    public string Value { get; set;}
    public AttributeDefinition Definition { get; set; }
    public List<HistoryEntry> HistoryEntries { get; set; }

    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            Attribute toCompare = (Attribute)obj;
            return Value == toCompare.Value && Definition.Equals(toCompare.Definition);
        }
    }
        
    public override int GetHashCode()
    {
        return HashCode.Combine(Definition,Value);
    }
}