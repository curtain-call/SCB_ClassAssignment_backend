using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing.Printing;

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
                    string result = reader.ReadToEnd();

                    // Find the first occurrence of a left curly brace and remove all characters before it
                    int index = result.IndexOf('{');
                    if (index >= 0)
                    {
                        result = result.Substring(index);
                    }
                    Console.WriteLine(result);
                    // 把输出的JSON字符串转换成一个字典
                    Dictionary<string, object> resumeInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                    return resumeInfo;
                }
            }
        }
    }
}
