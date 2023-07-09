using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace resume.Others
{
   /// <summary>
   /// 上传简历时，岗位下拉框里面所需的信息
   /// </summary>
    public class JobInfoForUpload
    {
        public string JobName { get; set; }//岗位的名称
        public int JobId { get; set; }//岗位在数据库中的ID
    }
}
