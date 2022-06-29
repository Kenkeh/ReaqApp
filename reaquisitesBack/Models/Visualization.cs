public class Visualization {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ColorFactor> ArtefactColorFactors { get; set; }
    public List<ColorFactor> RelationshipColorFactors { get; set; }
    public List<SizeFactor> ArtefactSizeFactors { get; set; }
    public List<SizeFactor> RelationshipSizeFactors { get; set; }
}