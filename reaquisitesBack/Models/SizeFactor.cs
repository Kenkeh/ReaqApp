public class SizeFactor {
    public bool Interpolated { get; set; }
    public AttributeDefinition Definition { get; set; }
    public Dictionary<string, int> Values { get; set; }
    public float Weight { get; set; }

    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            SizeFactor toCompare = (SizeFactor)obj;
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
        foreach (string id in Values.Keys)
            hash = HashCode.Combine(hash,Values[id]);
        return hash;
    }
}