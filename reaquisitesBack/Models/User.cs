

namespace reaquisites.Models
{
    public class User
    {
        public string Nick { get; set; }
        public string Account { get; set; }
        public string EMail { get; set; }
        public DateTime RegisterDate {get; set;}
        public List<Project> Projects {get;set;}

        public override bool Equals(Object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                User toCompare = (User)obj;
                return Nick == toCompare.Nick || Account == toCompare.Account || EMail == toCompare.EMail;
            }
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Nick,Account,EMail);
        }
    }
}