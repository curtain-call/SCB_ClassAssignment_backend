using Newtonsoft.Json;

namespace resume.Models
{
    public class JobMatch
    {
        public int ID { get; set; }
        public string? JobTitle { get; set; }
        [JsonProperty("人岗匹配程度分数")]
        public int Score { get; set; }
        [JsonProperty("人岗匹配的理由")]
        public string? Reason { get; set; }
        public int ApplicantProfileID { get; set; } // ForeignKey
    }
}
