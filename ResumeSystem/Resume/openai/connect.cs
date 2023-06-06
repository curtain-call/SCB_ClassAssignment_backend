using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ResumeSystem.openai
{
    internal class Connect
    {
        public Dictionary<string, object> analysis(string filepath, int n)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"D:\Python\python.exe";
            start.Arguments = string.Format("{0} \"{1}\" {2}", @"D:\PythonCode\openai\chat.py", filepath, n);
            //start.UseShellExecute = true;

            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            Console.WriteLine(start.Arguments);

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    Console.WriteLine("**********************************");
                    string result = reader.ReadToEnd();
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$");
                    Console.WriteLine(result);
                    Console.WriteLine("%%%%%%%%%%%%%%%");

                    // 把输出的JSON字符串转换成一个字典
                    Dictionary<string, object> resumeInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                    // 输出字典中的每一项
                    foreach (KeyValuePair<string, object> item in resumeInfo)
                    {
                        Console.WriteLine($"{item.Key}: {item.Value}");
                    }

                    // 将字典转换为 JSON 字符串并输出
                    string jsonStr = JsonConvert.SerializeObject(resumeInfo);
                    Console.WriteLine(jsonStr);

                    return resumeInfo;
                }
            }
        }
    }
}
