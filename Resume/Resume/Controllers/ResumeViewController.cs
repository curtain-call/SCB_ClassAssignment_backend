using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeSystem.ResultModels;
using ResumeSystem.Class;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore;

namespace ResumeSystem.Controllers
{
    /// <summary>
    /// 这个类是，主页的类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeViewController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ResumeViewController(MyDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 此时从主界面切换到简历界面 返回 该用户上传的所有的简历的ID(不显示)+人名+申请岗位+综合分
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ResumeDTO> MainViewToResume(int CompanyId)
        {
            //通过这个来标识，我的用户是哪个
            //调用接口 访问数据库 返回所有CompanyId的值，所对应的信息
            //MainViewToResumeModelClass SearchByCompanyId(int CompanyId)
            var resumes = _context.SearchByCompanyId(CompanyId);
            return resumes;
/*            //用来测试返回
            MainViewToResumeModelClass mainViewToResumeModelClass = new MainViewToResumeModelClass();
            Resume Resume1 = new Resume();
            Resume1.AppliCantName = "lihua";
            Resume1.ResumeId = CompanyId;
            Resume1.JobPosition = "baizi";
            Resume1.AllScore = 80;
            Resume Resume2 = new Resume();
            Resume2.AppliCantName = "xiaoming";
            Resume2.ResumeId = CompanyId+1;
            Resume2.JobPosition = "baizige";
            Resume2.AllScore = 90;
            mainViewToResumeModelClass.ResumesInfo.Add(Resume1);
            mainViewToResumeModelClass.ResumesInfo.Add(Resume2);
            mainViewToResumeModelClass.Test = 0;
            return mainViewToResumeModelClass.ResumesInfo;*/
        }


        /// <summary>
        /// 在简历界面，点击单一简历的处理
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SingleResume")]
        public SingleResumeModelClass ToSingleResume(int resumeId)
        {
            //调用接口获得，所该单简历的所有信息
            //传入：resumeId 返回：SingleResumeModelClass
            var singleResumeModelClass = _context.GetResumeById(resumeId);
            return singleResumeModelClass;
        }


       /// <summary>
       /// 第一次上传文件
       /// </summary>
       /// <param name="file"></param>
       /// <param name="jobs"></param>
       /// <returns></returns>
        [HttpPost]
        [Route("addResume")]
        public FirstAddResumeModelClass FirstAddResume(IFormFile file,string jobs)
        {

            //返回值
            //先判断file类型，是不是我们需要的文件类型
            if (!new[] { "application/pdf", "text/plain", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword" }.Contains(file.ContentType))
            {
               FirstAddResumeModelClass first=new FirstAddResumeModelClass();
                first.IsSuccess = false;
                first.Msg = "仅支持doc，docx，txt，pdf格式文件上传";
                return first;
            }

            //接着判断文件是否为空
            if (file is { Length: > 0 })
            {//非空
                try
                {
                    //将文件存在指定的地点
                    //并获得文件的存储地址
                    string fileName = Path.GetFileName(file.FileName);
                    string staticFileRoot = "Resumes";
                    //这里是文件路径，不包含文件名
                    string fileUrlWithoutFileName = @$"{DateTime.Now.Year}\{DateTime.Now.Month}\{DateTime.Now.Day}";
                    //创建文件，如过文件已经存在，则什么也不做
                    Directory.CreateDirectory($"{staticFileRoot}/{fileUrlWithoutFileName}");

                    //使用哈希的原因是前端可能传递相同的文件，服务端不想保存多个相同的文件
                    SHA256 hash = SHA256.Create();
                    //读取文件刘的值 把文件流转换为哈希值
                    byte[] hashByte = hash.ComputeHash(file.OpenReadStream());
                    //在把哈希值转为字符串 当作文件的文件名
                    string hashedFileName = BitConverter.ToString(hashByte).Replace("-", "");

                    //重新获得一个文件名
                    string newFileName = hashedFileName + "." + fileName.Split('.').Last();
                    //应该创建的文件的绝对地址
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"{staticFileRoot}\{fileUrlWithoutFileName}", newFileName);
                    //创建这个文件，依据绝对地址
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    //将该网络传来的文件，全部都赋值给那个新建的文件
                    file.CopyTo(fileStream);

                    //调用算法接口，对保存的简历进行分析，并将路径保存在数据库中，并返回数据

                    //传入参数：filepath 返回：FirstAddResumeModelClass 并实现将该路径存入数据库


                }
                catch (Exception ex)
                {
                  
                }
             
            }

            //后面删除
            FirstAddResumeModelClass fi = new FirstAddResumeModelClass();
            fi.Msg = file.ContentType;
            return fi;
        }


        /// <summary>
        /// 此时客户，对第一次上传的文件进行了修改，然后，将其返回
        /// 然后 ，后端返回，是否添加成功
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("secondAddReume")]
        public SecondAddResumeModelClass SecondAddResume(FirstAddResumeModelClass first)
        {
            //接下来调用接口，将接着这些修改后的数据写如数据库
            //传入参数：FirstAddResumeModelClass类   返回参数SecondAddResumeModelClass

            //测试
            SecondAddResumeModelClass secondAddResumeModelClass = new SecondAddResumeModelClass();
            secondAddResumeModelClass.AddSuccess = true;
            return secondAddResumeModelClass;
        }





    }
}
