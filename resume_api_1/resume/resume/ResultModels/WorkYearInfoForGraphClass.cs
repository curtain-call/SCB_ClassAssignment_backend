namespace resume.ResultModels
{
    public class WorkYears
    {
        public int Year0 { get; set; }
        public int Year1 { get; set; }
        public int Year2 { get; set; }
        public int Year3 { get; set; }
        public int Year4 { get; set; }
        public int Year5 { get; set; }
        public int Year6 { get; set; }
        public int Year7 { get; set;}
        public int Year8 { get; set;}
        public int Year9 { get; set;}
        public int Year10 { get; set;}
        public int Year11 { get; set;}
        public int Year12 { get; set;}
        public int Year13 { get; set;}
        public int Year14 { get; set;}

        public int Year15 { get; set;}
        public int Above15{ get; set;}
        // ...
    }

    /// <summary>
    /// 各年龄端的人的数量
    /// </summary>
    public class WorkYearInfoForGraphClass
    {
        public WorkYears WorkYears { get; set; }
    }
}
