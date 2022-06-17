public class HistoryEntry{
    public DateTime ChangeDate { get; set; }
    public int ElementType { get; set; }
    public int ElementId { get; set; }
    public int ChangeType { get; set; }
    public string Changes { get; set; }

    public override bool Equals(Object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            HistoryEntry toCompare = (HistoryEntry)obj;
            return ChangeDate == toCompare.ChangeDate && ElementType == toCompare.ElementType && ElementId == toCompare.ElementId
                && ChangeType == toCompare.ChangeType;
        }
    }
    public override int GetHashCode()
    {
        int hash = HashCode.Combine(ChangeDate,ElementType,ElementId,ChangeType,Changes);
        return hash;
    }
}