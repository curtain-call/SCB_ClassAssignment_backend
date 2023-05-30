using System.Security.Cryptography.X509Certificates;

namespace ResumeSystem.ResultModels
{
    public class MainToJobModelClass
    {
        public int FisrtJobId { get; set; }//第一个简历的编号
        public IEnumerable<Job> Jobs;//所有简历
        public IEnumerable<string> FirstJobRequest;

    }
}
