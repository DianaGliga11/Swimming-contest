namespace mpp_proiect_csharp_DianaGliga11.Model
{

    public class User : Entity<int>
    {
        private String UserName { get; set; }
        private String Password { get; set; }

        public User(int id, String userName, String password)
        {
            Id = id;
            UserName = userName;
            Password = password;
        }

        public override String ToString()
        {
            return $"ID={Id}, userName={UserName}, password={Password}";
        }

    }
}