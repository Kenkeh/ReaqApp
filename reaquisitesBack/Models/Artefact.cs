public class Artefact {
    public int ID { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ArtefactDefinition Definition { get; set; }
    public List<Attribute> Attributes { get; set; }

    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            Artefact toCompare = (Artefact)obj;
            return Name == toCompare.Name && Definition.Equals(toCompare.Definition); /*&& Attributes.All(
                att => {
                    return toCompare.Attributes.Contains(att);
                }
            ) && toCompare.Attributes.All(
                att => {
                    return Attributes.Contains(att);
                }
            );*/
        }
    }
        
    public override int GetHashCode()
    {
        return HashCode.Combine(Name,Definition);
    }
}