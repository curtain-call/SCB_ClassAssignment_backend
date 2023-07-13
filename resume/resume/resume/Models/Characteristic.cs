using Newtonsoft.Json;

namespace resume.Models
{
    public class Characteristic
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        [JsonProperty("分数")]
        public int Score { get; set; }
        [JsonProperty("原因")]
        public string? Reason { get; set; }
    }
}
