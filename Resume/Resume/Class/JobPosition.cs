namespace ResumeSystem.Class
{
    public class JobPosition
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // 导航属性
        public Resume Resume { get; set; }
    }
}
