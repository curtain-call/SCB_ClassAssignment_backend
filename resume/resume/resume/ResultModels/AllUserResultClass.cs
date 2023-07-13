namespace resume.ResultModels
{
    public class BriefUser
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Role { get; set; }
    }


    public class AllUserResultClass
    {
        public List<BriefUser> Users { get; set; }

    }
}
