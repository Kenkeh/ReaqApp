public class ColorFactor {
    public bool Interpolated { get; set; }
    public AttributeDefinition Definition;
    public List<KeyValuePair<string, System.Drawing.Color>> Values { get; set; }
    public float Weight { get; set; }
    public List<HistoryEntry> HistoryEntries { get; set; }

    
    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            ColorFactor toCompare = (ColorFactor)obj;
            return Interpolated == toCompare.Interpolated && Values.All(value =>{
                return toCompare.Values.Contains(value);
            }) && toCompare.Values.All(value=>{
                return Values.Contains(value);
            });
        }
    }
    public override int GetHashCode()
    {
        int hash = HashCode.Combine(Interpolated);
        for (int i=0; i<Values.Count;i++)
            hash = HashCode.Combine(hash,Values[i]);
        return hash;
    }
}