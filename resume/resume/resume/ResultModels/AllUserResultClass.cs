namespace resume.ResultModels
{
    public class User
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Role { get; set; }
    }


    public class AllUserResultClass
    {
        public List<User> Users { get; set; }

    }
}
