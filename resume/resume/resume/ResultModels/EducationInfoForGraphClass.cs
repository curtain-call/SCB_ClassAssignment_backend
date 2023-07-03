namespace resume.ResultModels
{
    public class HighestEducation
    {
        public int HighSchoolOrLess { get; set; }
        public int JuniorCollege { get; set; }
        public int Bachelor { get; set; }
        public int Master { get; set; }
        public int Doctor { get; set; }
        // ...
    }

    public class GraduationSchoolsLevel
    {
        public int _985_211 { get; set; }
        public int OrdinaryFirstClass { get; set; }
        public int SecondClassOrBelow { get; set; }
        // ...
    }

    /// <summary>
    /// 根节点,也就是网页所返回的
    /// </summary>
    public class EducationInfoForGraphClass
    {
        public HighestEducation HighestEducation { get; set; }
        public GraduationSchoolsLevel GraduationSchoolsLevel { get; set; }
    }
}
