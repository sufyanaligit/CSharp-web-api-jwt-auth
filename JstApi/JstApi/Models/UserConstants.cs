namespace JstApi.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel(){ Username = "jason_admin", EmailAddress = "jason.admin@email.com", Password = "MyPass_w0rd", GivenName = "Jason", Surname = "Bryant", Role = "Administrator"},


            new UserModel(){ Username = "jason_admin2", EmailAddress = "jason.admin2@email.com", Password = "MyPass_w0rd2", GivenName = "Jason2", Surname = "Bryant2", Role = "Seller"},
        };
    }
}
