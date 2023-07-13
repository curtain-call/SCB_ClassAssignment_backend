namespace resume.Models
{
    public class PersonalCharacteristics
    {
        public int ID { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
        public int ApplicantProfileID { get; set; } // ForeignKey
    }

    public class SkillsAndExperiences
    {
        public int ID { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
        public int ApplicantProfileID { get; set; } // ForeignKey
    }

    public class AchievementsAndHighlights
    {
        public int ID { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
        public int ApplicantProfileID { get; set; } // ForeignKey
    }
}
