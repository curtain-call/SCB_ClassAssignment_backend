namespace resume.ResultModels
{
    /// <summary>
    /// 当点击单一简历时，返回的类
    /// </summary>
    public class SingleResumeModelClass
    {
        public int ResuemId { get; set; }
        public int Age { get; set; }    
        public string Name { get; set; }

        public string Email { get; set; }
        public string JobPosition { get; set; }
        public string Gender { get; set; }
        public string HighestEducation { get; set;}
        public int WorkExperience { get; set; }
        public int OverallScore { get; set; }   
        public string Label { get; set; }
    }
}
