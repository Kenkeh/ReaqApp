public class SizeFactor {
    public bool Interpolated { get; set; }
    public List<KeyValuePair<string, int>> Values { get; set; }

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
        for (int i=0; i<Values.Count;i++)
            hash = HashCode.Combine(hash,Values[i]);
        return hash;
    }
}