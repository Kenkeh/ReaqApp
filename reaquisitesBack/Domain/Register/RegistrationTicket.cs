public class RegistrationTicket{
    private string ticket;
    internal string Ticket {get{return ticket;}}
    private DateTime creationDate;
    internal DateTime CreationDate {get{return creationDate;}}

    public RegistrationTicket(string ticket, DateTime creationDate){
        this.ticket = ticket;
        this.creationDate = creationDate;
    }
}