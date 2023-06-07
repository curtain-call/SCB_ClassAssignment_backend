using Newtonsoft.Json;
namespace ResumeSystem.Models
{
    public class WorkExperiences
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        [JsonProperty("地点")]
        public string? CompanyName { get; set; }
        [JsonProperty("职位")]
        public string? Position { get; set; }
        [JsonProperty("时间")]
        public string? Time { get; set; }
        [JsonProperty("任务")]
        public string? Task { get; set; }
        //public Applicant Applicant { get; set; }
    }
}
