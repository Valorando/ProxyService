namespace receive_ID.Models
{
    public class ExternalUserData
    {
        public UserData data { get; set; }
    }

    public class UserData
    {
        public int userid { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string avatar { get; set; }
    }
}
