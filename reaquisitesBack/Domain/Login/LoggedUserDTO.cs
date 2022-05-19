using reaquisites.Models;
public class LoggedUserDTO {
    public string Nick { get; set; }
    public string Account { get; set; }
    public string EMail { get; set; }
    public DateTime RegisterDate {get; set; }
    public string LoginSession { get; set; }
    public List<SimpleProjectDTO> Projects { get; set; }
}