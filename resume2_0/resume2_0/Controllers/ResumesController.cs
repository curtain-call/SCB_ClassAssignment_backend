using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume2_0.ResultModel;
using resume2_0.WebSentModel;
using resume2_0.OtherClass;
using System.Security.Cryptography;

namespace resume2_0.Controllers
{
    /// <summary>
    /// 简历界面的webAPI
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        /// <summary>
        /// 返回的是该用户id下所有简历的简略信息
        /// </summary>
        /// <param name="allResumeSentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AllResumes")]
        public AllSimpleResumes ForAllSimpleResumes(AllResumeSentModel allResumeSentModel) {
            int userId = allResumeSentModel.UserId;//前端传来的用户ID
            //查数据库 调用接口
            //传入 userId 返回 AllSimpleResumes


            //测试
            AllSimpleResumes allSimpleResumes = new AllSimpleResumes();
            allSimpleResumes.SimpleResumes = new List<SimpleResume>();
            SimpleResume simpleResume1 = new SimpleResume();
            simpleResume1.Age = 1;
            simpleResume1.PhoneNumber = "1855615";
            allSimpleResumes.SimpleResumes.Add(simpleResume1);
            return allSimpleResumes;

        }


        /// <summary>
        /// 返回该简历ID的所有详细信息
        /// </summary>
        /// <param name="ForoneResume"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DetailedResume")]
        public DetailedResume ForOneDetailedResume(ClickOneResumeSentModel ForoneResume) {
            int RId = ForoneResume.RId;//该简历的ID
            //调用函数，查询对应的简历ID的所有详细信息
            //输入 Rid 返回 detailedResume

            
            
            //测试
            DetailedResume detailedResume = new DetailedResume();
            detailedResume.Age = 1;
            detailedResume.workExperiences =new List<WorkExperience>();
            WorkExperience workExperience1 = new WorkExperience();
            workExperience1.Time = "2022";
            workExperience1.CompanyAddress = "WHU";
            detailedResume.workExperiences.Add(workExperience1);
            WorkExperience workExperience2 = new WorkExperience();
            workExperience2.Time = "202ad";
            workExperience2.CompanyAddress = "WHadU";
            detailedResume.workExperiences.Add(workExperience2);

            return detailedResume;
        }



        /// <summary>
        /// 第一次上传简历
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadResume")]
        public SimpleResume UploadResume(IFormFile file,int userId) {
            //IFormFile file = uploadResumeSentModel.iFormFile;


            ////返回值
            ////先判断file类型，是不是我们需要的文件类型
            //if (!new[] { "application/pdf", "text/plain", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword" }.Contains(file.ContentType))
            //{
               
            //    first.IsSuccess = false;
            //    first.Msg = "仅支持doc，docx，txt，pdf格式文件上传";
            //    return first;
            //}

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
                    return new SimpleResume();

                }
                catch (Exception ex)
                {

                }
             
            }

            return new SimpleResume();
        }


        [HttpPost]
        [Route("SecondModifyResume")]
        public SecondModifyResultModel SecondModifyResume(SimpleResume simpleResume) {
            //将该simpleResume上传数据库
            //返回对应的SecondModifyResultModel

            return new SecondModifyResultModel();

        }

    }
}
