public class RelationshipColorFactor {
    public bool Interpolated { get; set; }
    public AttributeDefinition AttributeDefinition { get; set; }
    public RelationshipDefinition ElementDefinition { get; set; }
    public List<ColorFactorValue> Values { get; set; }
    public float Weight { get; set; }

    
    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            ArtefactColorFactor toCompare = (ArtefactColorFactor)obj;
            return Interpolated == toCompare.Interpolated && ElementDefinition.ID == toCompare.ElementDefinition.ID 
                && AttributeDefinition.Name == toCompare.AttributeDefinition.Name
                && Values.All(value =>{
                    return toCompare.Values.Contains(value);
                }) && toCompare.Values.All(value=>{
                    return Values.Contains(value);
                });
        }
    }
    public override int GetHashCode()
    {
        int hash = HashCode.Combine(Interpolated, ElementDefinition.ID, AttributeDefinition.Name, Weight);
        foreach (ColorFactorValue value in Values)
            hash = HashCode.Combine(hash,value.Key, value.R, value.G, value.B, value.A);
        return hash;
    }
}