using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using resume.Others;

namespace resume.open
{
    internal class Connect
    {

        /*
            由于`Run`函数被定义为`async Task`，它是一个异步函数，所以在调用它时你应该使用`await`关键字。
            `await`关键字用于等待一个异步操作的完成。它可以帮助你编写更清晰、更易读的异步代码，而不需要嵌套大量的回调函数。
            当你在一个方法中使用`await`关键字时，这个方法需要被声明为`async`。
            在你的例子中，`Main`方法被声明为`async Task Main`，这意味着它是一个异步的`Main`方法，可以使用`await`关键字。
            如果你不在`Main`方法中使用`await`，那么程序可能会在异步操作完成之前退出，这可能会导致程序不能按预期的方式工作。
            总的来说，如果你想要在`Main`方法中等待一个异步操作的完成，你应该使用`await`关键字。
         */

        /*public Dictionary<string, object> analysis(string filepath)
        {
            //string filePathTest = @"D:\visualStudio workspace\SCB_ClassAssignment_backend\end\word\94.docx";
            Dictionary<string, object> result = Run(filepath).GetAwaiter().GetResult();
            foreach (var property in result)
            {
                string propertyName = property.Key;
                object propertyValue = property.Value;

                Console.WriteLine($"Property: {propertyName}, Value: {propertyValue}");
                Console.Out.Flush();
            }
            return result;
        }*/
        /*        ame(filepath);
                    string fileName = Path.GetFileNameWithoutExtension(filepath);
                    string extension = Path.GetExtension(filepath);
        */

        public CombineDictionary analysis(string filepath)
        {

            Console.OutputEncoding = Encoding.UTF8;
            Console.Out.Flush();


            RunBase(filepath); //转换文件类型


            Console.Out.Flush();
            Console.WriteLine("文件转换完成");

            Console.Out.Flush();

            Dictionary<string, object> result1 = Run1(filepath).GetAwaiter().GetResult();//基本信息+人才画像

            Console.Out.Flush();

            foreach (var property in result1)
            {
                string propertyName = property.Key;
                object propertyValue = property.Value;

                Console.WriteLine($"Property: {propertyName}, Value: {propertyValue}");
                Console.Out.Flush();
            }
            Console.Out.Flush();

            Dictionary<string, object> result2 = Run2(filepath).GetAwaiter().GetResult();//人岗匹配
            Console.Out.Flush();


            foreach (var property in result2)
            {
                string propertyName = property.Key;
                object propertyValue = property.Value;

                Console.WriteLine($"Property: {propertyName}, Value: {propertyValue}");
                Console.Out.Flush();
            }
            Console.Out.Flush();
            CombineDictsToJson(result1, result2, filepath);//两个字典合为一个json
            CombineDictionary result = new CombineDictionary()
            {
                BasicResult = result1,
                JobMatchResult = result2,
            };

            Console.Out.Flush();
            Console.WriteLine("生成json文件成功啦");
            Console.Out.Flush();
            return result;
        }





        private static void RunBase(string filepath)
        {
            int n1 = 1001;


            string directoryPath = Path.GetDirectoryName(filepath);
            string fileName = Path.GetFileNameWithoutExtension(filepath);
            string extension = Path.GetExtension(filepath);

            getFile(directoryPath, fileName, extension, n1);
            Console.WriteLine("文件成功转换");
            Console.Out.Flush();

        }


        static void CombineDictsToJson(Dictionary<string, object> dict1, Dictionary<string, object> dict2, string filepath)
        {
            // Combine the two dictionaries
            Dictionary<string, object> combinedData = new Dictionary<string, object>(dict1);
            foreach (var pair in dict2)
            {
                combinedData[pair.Key] = pair.Value;
            }

            string directoryPath = Path.GetDirectoryName(filepath);
            string fileName = Path.GetFileNameWithoutExtension(filepath);

            // Convert the combined data to JSON
            string combinedJson = JsonConvert.SerializeObject(combinedData, Formatting.Indented);

            string parentDirectoryPath = Path.GetDirectoryName(directoryPath);
            string jsonFolderPath = Path.Combine(parentDirectoryPath, "json");

            // Ensure that the JSON folder exists
            Directory.CreateDirectory(jsonFolderPath);

            string combinedJsonFileName = $"{fileName}.json";
            string combinedJsonFilePath = Path.Combine(jsonFolderPath, combinedJsonFileName);

            // Write the combined JSON to a file
            //File.AppendAllText(combinedJsonFilePath, combinedJson);
            File.WriteAllText(combinedJsonFilePath, combinedJson);
            Console.Out.Flush();

            Console.WriteLine("Combined JSON file created: " + combinedJsonFilePath);
            Console.Out.Flush();
        }

        private static void getFile(string directoryPath, string fileName, string extension, int n)
        {
            try
            {
                string key = " ";
                Console.WriteLine("开始文件转换啦");
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = @"D:\Python\python.exe";
                start.Arguments = string.Format("{0} \"{1}\" {2} \" {3} \" {4} \" {5}", @"D:\GitHub\end\fileRelate.py", directoryPath, fileName, extension, n, key);
                start.UseShellExecute = false; // Do not use OS shell
                start.CreateNoWindow = true; // We don't need new window
                start.RedirectStandardOutput = true; // Any output, generated by application will be redirected back
                start.RedirectStandardError = true; // Any error in standard output will be redirected back

                StringBuilder sb = new StringBuilder();
                using (Process process = Process.Start(start))
                {
                    process.OutputDataReceived += (sender, args) =>
                    {
                        Console.WriteLine(args.Data);
                        sb.AppendLine(args.Data);
                    };
                    process.BeginOutputReadLine();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("Python script completed successfully");
                    }
                    else
                    {
                        Console.WriteLine($"Python script failed with exit code: {process.ExitCode}");
                    }
                }

                Console.WriteLine(sb.ToString()); // Print all the output
                Console.Out.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in running python script: " + ex.Message);
                Console.Out.Flush();
            }
        }



        private static async Task<Dictionary<string, object>> Run(string filepath)
        {

            string directoryPath = Path.GetDirectoryName(filepath);
            string fileName = Path.GetFileNameWithoutExtension(filepath);
            string extension = Path.GetExtension(filepath);



            // Run the first script and get the results
            Dictionary<string, object> results1 = await Run1(filepath);

            // Run the second script and get the results
            Dictionary<string, object> results2 = await Run2(filepath);




            // Merge the results from the two scripts
            Dictionary<string, object> combinedResults = results1.Concat(results2)
                .GroupBy(pair => pair.Key)
                .ToDictionary(group => group.Key, group => group.First().Value);



            // Convert the combined data to JSON
            string combinedJson = JsonConvert.SerializeObject(combinedResults, Formatting.Indented);

            string parentDirectoryPath = Path.GetDirectoryName(directoryPath);
            string jsonFolderPath = Path.Combine(parentDirectoryPath, "json");

            string combinedJsonFileName = $"{fileName}.json";
            string combinedJsonFilePath = Path.Combine(jsonFolderPath, combinedJsonFileName);

            // Write the combined JSON to a file
            File.AppendAllText(combinedJsonFilePath, combinedJson);
            Console.Out.Flush();

            Console.WriteLine("Combined JSON file created: " + combinedJsonFilePath);
            Console.Out.Flush();

            return combinedResults;
        }



        private static async Task<Dictionary<string, object>> Run1(string filepath)
        {
            int n1 = 1001;
            int n2 = 1002;
            int n3 = 1003;
            int maxTries = 3;
            int n4 = 1004;
            int n5 = 1005;
            int n6 = 1006;


            string key = "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";

            string directoryPath = Path.GetDirectoryName(filepath);
            string fileName = Path.GetFileNameWithoutExtension(filepath);
            string extension = Path.GetExtension(filepath);




            // Create a list of tasks to run python scripts
            List<Task<string>> tasks = new List<Task<string>> {
                RunPythonScript(directoryPath, fileName, extension, n1, maxTries, key),
                RunPythonScript(directoryPath, fileName, extension, n2, maxTries, key),
                RunPythonScript(directoryPath, fileName, extension, n4, maxTries, key),

            };

            await CombineDataAndWriteToFile(tasks, directoryPath, fileName);
            // Wait for all tasks to complete and combine the data
            Dictionary<string, object> combinedData = await CombineDataAndWriteToFile(tasks, directoryPath, fileName);

            string schoolRanksFilePath = @"D:\GitHub\end\school.json";

            // Get the school ranks data
            string schoolRanksJson = File.ReadAllText(schoolRanksFilePath);
            JObject schoolRanks = JObject.Parse(schoolRanksJson);

            // Convert JArrays to List<string>
            List<string> schoolRank985 = schoolRanks["985"].ToObject<List<string>>();
            List<string> schoolRank211 = schoolRanks["211"].ToObject<List<string>>();

            // Read the "毕业院校" attribute from combinedData
            string schoolName = combinedData["毕业院校"].ToString();

            // Check if the school is in 985 or 211
            if (schoolRank985.Contains(schoolName))
            {
                combinedData["最高学历学校等级"] = "985";
            }
            else if (schoolRank211.Contains(schoolName))
            {
                combinedData["最高学历学校等级"] = "211";
            }
            else
            {
                // If the school is not in 985 or 211 and "最高学历学校等级" is not set, set it to "普通一本"
                if (!combinedData.ContainsKey("最高学历学校等级") || combinedData["最高学历学校等级"] == null)
                {
                    combinedData["最高学历学校等级"] = "普通一本";
                }
            }





            return combinedData;

        }

        private static async Task<Dictionary<string, object>> Run2(string filepath)
        {
            int n1 = 1001;
            int n2 = 1002;
            int n3 = 1003;
            int maxTries = 3;
            int n4 = 1004;
            int n5 = 1005;
            int n6 = 1006;

            string key = "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";


            string directoryPath = Path.GetDirectoryName(filepath);
            string fileName = Path.GetFileNameWithoutExtension(filepath);
            string extension = Path.GetExtension(filepath);




            // Create a list of tasks to run python scripts
            List<Task<string>> tasks = new List<Task<string>> {
                RunPythonScript(directoryPath, fileName, extension, n5, maxTries, key),
                RunPythonScript(directoryPath, fileName, extension, n6, maxTries, key),


            };

            await CombineDataAndWriteToFile(tasks, directoryPath, fileName);
            // Wait for all tasks to complete and combine the data
            Dictionary<string, object> combinedData = await CombineDataAndWriteToFile(tasks, directoryPath, fileName);



            return combinedData;

        }


        private static async Task<Dictionary<string, object>> CombineDataAndWriteToFile(List<Task<string>> tasks, string directoryPath, string r_id)
        {
            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
            Console.Out.Flush();

            // Create a dictionary to store the combined data
            Dictionary<string, object> combinedData = new Dictionary<string, object>();
            Console.Out.Flush();

            // Extract data from the output and combine them
            foreach (var task in tasks)
            {
                string result = await task;
                await Console.Out.WriteLineAsync(result);
                string jsonStr = ExtractJson(result);
                JObject json = JObject.Parse(jsonStr);
                foreach (var property in json)
                {
                    combinedData[property.Key] = property.Value;
                }
            }
            Console.Out.Flush();


            return combinedData;
        }
        private static string ExtractJson(string input)
        {
            // Use regex to extract valid JSON from the input string
            // This pattern uses balanced groups to correctly match nested braces.
            string pattern = @"(?<=\{)(([^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!)))(?=\})";
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                // Add back the outer braces to form a valid JSON object
                return "{" + match.Value + "}";
            }
            else
            {
                return null;
            }
        }






        private static Task<string> RunPythonScript(string filepath, string fileName, string extension, int n, int maxTries, string key)
        {
            var tcs = new TaskCompletionSource<string>();
            StringBuilder sb = new StringBuilder();

            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = @"D:\Python\python.exe";
                start.Arguments = string.Format("{0} \"{1}\" {2} \" {3} \" {4} \" {5}", @"D:\GitHub\end\chat.py", filepath, fileName, extension, n, key);
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;

                Console.WriteLine("开始分析啦,请稍等");
                Process process = new Process();
                process.StartInfo = start;
                process.OutputDataReceived += (sender, args) =>
                {
                    // This is your real-time stream here
                    sb.AppendLine(args.Data);
                };
                process.EnableRaisingEvents = true;
                process.Exited += (sender, args) =>
                {
                    tcs.SetResult(sb.ToString());
                    process.Dispose();
                };
                process.Start();
                process.BeginOutputReadLine();
            }
            catch (Exception ex) // 捕获异常
            {
                // 打印出异常信息
                Console.WriteLine($"运行出错: {ex.Message}");
            }
            return tcs.Task;
        }



    }
}

