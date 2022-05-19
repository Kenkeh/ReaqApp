public class RegisterInfo {
    private string userName;
    internal string UserName{ get{ return userName; } }
    private string accountName;
    internal string AccountName{ get{ return accountName; } }
    private string userEmail;
    internal string UserEmail{get{return userEmail;}}
    private string userPassword;
    internal string UserPassword{get{return userPassword;}}

    public RegisterInfo(RegisterInfoDTO dto){
        this.userEmail = dto.userEmail;
        this.userName = dto.userName;
        this.userPassword = dto.userPassword;
        this.accountName = dto.userAccount;
    }
}