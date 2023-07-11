using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace resume.open
{
    internal class Connect
    {

        /*
         由于`Run`函数被定义为`async Task`，它是一个异步函数，所以在调用它时你应该使用`await`关键字。
`await`关键字用于等待一个异步操作的完成。它可以帮助你编写更清晰、更易读的异步代码，而不需要嵌套大量的回调函数。
当你在一个方法中使用`await`关键字时，这个方法需要被声明为`async`。在你的例子中，`Main`方法被声明为`async Task Main`，这意味着它是一个异步的`Main`方法，可以使用`await`关键字。
如果你不在`Main`方法中使用`await`，那么程序可能会在异步操作完成之前退出，这可能会导致程序不能按预期的方式工作。
总的来说，如果你想要在`Main`方法中等待一个异步操作的完成，你应该使用`await`关键字。
         */

        public async Task<Dictionary<string, object>> analysis(string filepath)
        {

            Dictionary<string, object> result = await Run(filepath);
            foreach (var property in result)
            {
                string propertyName = property.Key;
                object propertyValue = property.Value;

                await Console.Out.WriteLineAsync($"Property: {propertyName}, Value: {propertyValue}");
            }
            return result;  
        }



        private static async Task<Dictionary<string, object>> Run(string filepath)
        {
            int n1 = 1;
            int n2 = 2;
            int n3 = 3;
            int maxTries = 3;



            string directoryPath = Path.GetDirectoryName(filepath);
            string fileName = Path.GetFileNameWithoutExtension(filepath);
            string extension = Path.GetExtension(filepath);




            // Create a list of tasks to run python scripts
            List<Task<string>> tasks = new List<Task<string>> {
                RunPythonScript(directoryPath, fileName, extension, n1, maxTries),
                RunPythonScript(directoryPath, fileName, extension, n2, maxTries),
                RunPythonScript(directoryPath, fileName, extension, n3, maxTries)
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

            // Create a dictionary to store the combined data
            Dictionary<string, object> combinedData = new Dictionary<string, object>();

            // Extract data from the output and combine them
            foreach (var task in tasks)
            {
                string result = await task;
                string jsonStr = ExtractJson(result);
                JObject json = JObject.Parse(jsonStr);
                foreach (var property in json)
                {
                    combinedData[property.Key] = property.Value;
                }
            }



            // Convert the combined data to JSON
            string combinedJson = JsonConvert.SerializeObject(combinedData, Formatting.Indented);

            string parentDirectoryPath = Path.GetDirectoryName(directoryPath);
            string jsonFolderPath = Path.Combine(parentDirectoryPath, "json");

            string combinedJsonFileName = $"combined{r_id}.json";
            string combinedJsonFilePath = Path.Combine(jsonFolderPath, combinedJsonFileName);

            // Write the combined JSON to a file
            File.WriteAllText(combinedJsonFilePath, combinedJson);
            Console.Out.Flush();

            Console.WriteLine("Combined JSON file created: " + combinedJsonFilePath);
            Console.Out.Flush();

            return combinedData;
        }

        private static string ExtractJson(string input)
        {
            // Use regex to extract valid JSON from the input string
            string pattern = @"\{(.|\n)*\}";
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                throw new Exception("Failed to extract JSON from input string.");
            }
        }

        private static async Task<string> RunPythonScript(string filepath, string fileName, string extension, int n, int maxTries)
        {
            string output = string.Empty;

            for (int i = 0; i < maxTries; i++)
            {
                try
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = @"D:\study\Anaconda\install\relate\envs\open\python.exe";
                    start.Arguments = string.Format("{0} \"{1}\" {2} \" {3} \" {4} ", @"E:\softbei\code\chat.py", filepath, fileName, extension, n);


                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = true;

                    Console.WriteLine(start.Arguments);
                    Console.Out.Flush();

                    using (Process process = await Task.Run(() => Process.Start(start)))
                    {
                        using (StreamReader reader = process.StandardOutput)
                        {
                            output = await reader.ReadToEndAsync();

                            Console.WriteLine(output);
                            Console.Out.Flush();
                        }
                    }
                    // 如果代码执行到这里，那么表示没有抛出异常，所以可以跳出循环
                    break;
                }
                catch (Exception ex) // 捕获异常
                {
                    // 打印出异常信息
                    Console.WriteLine($"运行出错: {ex.Message}");
                    Console.Out.Flush();

                    // 如果已经达到最大尝试次数，则抛出异常
                    if (i == maxTries - 1)
                    {
                        throw;
                    }
                    else
                    {
                        Console.WriteLine("正在尝试重新运行...");
                        Console.Out.Flush();
                    }
                }
            }

            return output;
        }
    }
}
