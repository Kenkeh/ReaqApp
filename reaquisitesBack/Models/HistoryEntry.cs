public class HistoryEntry{
    public DateTime ChangeDate { get; set; }
    public int ElementType { get; set; }
    public int ElementId { get; set; }
    public int ChangeType { get; set; }
    public string Changes { get; set; }
}