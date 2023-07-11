using System.Security.Cryptography.X509Certificates;
using resume.Others;
namespace resume.ResultModels
{
    /// <summary>
    /// 一点击上传简历，需要我们返回的岗位下拉框里多由岗位的名称
    /// </summary>
    public class HomeToUploadResume
    {
      public List<JobInfoForUpload> UploadNeedJobsInfo { get; set; }
    }
}
