public class Visualization {
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ArtefactColorFactor> ArtefactColorFactors { get; set; }
    public List<RelationshipColorFactor> RelationshipColorFactors { get; set; }
    public List<ArtefactSizeFactor> ArtefactSizeFactors { get; set; }
    public List<RelationshipSizeFactor> RelationshipSizeFactors { get; set; }
}