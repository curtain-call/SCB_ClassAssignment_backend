using Newtonsoft.Json;

namespace resume.Models
{
    public class EducationBackground
    {
        public int ID { get; set; }
        public int ApplicantID { get; set; }
        [JsonProperty("时间")]
        public string? Time { get; set; }
        [JsonProperty("学校")]
        public string? School { get; set; }
        [JsonProperty("专业")]
        public string? Major { get; set; }

        //public Applicant Applicant { get; set; }
    }
}
