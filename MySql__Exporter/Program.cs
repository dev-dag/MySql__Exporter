using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MySql__Exporter
{
    internal class Program
    {
        public const string password = "rubestry030409";
        public const string dbSavePath = @"C:\WorkSpace\Unity\RPG\Assets\StreamingAssets/BaseData.db";

        static async Task Main(string[] args)
        {
            Converter converter = new Converter(dbSavePath, password);

            await converter.Convert();
        }
    }

    public class Converter
    {
        private string dbSavePath;
        private string password;

        public Converter(string dbPath, string password)
        {
            this.dbSavePath = dbPath;
            this.password = password;
        }

        public async Task Convert()
        {
            ProcessStartInfo pri = new ProcessStartInfo()
            {
                FileName = @"cmd.exe",
                WorkingDirectory = @"C:\",
                CreateNoWindow = true,
                UseShellExecute = false,

                RedirectStandardInput = true,                //표준 출력을 리다이렉트
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Process pro = new Process();

            pro.StartInfo = pri;

            pro.Start();   //어플리케이션 실행

            pro.StandardInput.AutoFlush = false;

            pro.StandardInput.WriteLine(@"cd C:\Users\pppol\AppData\Local\Programs\Python\Python313\Scripts");
            pro.StandardInput.Flush();

            pro.StandardInput.WriteLine($"mysql2sqlite -d base_data -u root -f {dbSavePath}  --mysql-password {password}");
            pro.StandardInput.Flush();

            pro.StandardInput.Close();

            var output = await pro.StandardOutput.ReadToEndAsync();
            var err = await pro.StandardError.ReadToEndAsync();

            return;
        }
    }

    //public class Exporter
    //{
    //    ProcessStartInfo pri = new ProcessStartInfo();
    //    Process pro = new Process();

    //    public Exporter()
    //    {
    //        pri.FileName = @"cmd.exe";
    //        pri.CreateNoWindow = true;
    //        pri.UseShellExecute = false;

    //        pri.RedirectStandardInput = true;                //표준 출력을 리다이렉트
    //        pri.RedirectStandardOutput = true;
    //        pri.RedirectStandardError = true;
    //    }

    //    public async Task DumpDB(string dumpFilePath)
    //    {
    //        pro = new Process();

    //        pro.StartInfo = pri;

    //        pro.Start();   //어플리케이션 실행

    //        pro.StandardInput.AutoFlush = false;
    //        pro.StandardInput.Write(@"cd C:\Program Files\MySQL\MySQL Server 8.0\bin");
    //        pro.StandardInput.WriteLine();
    //        pro.StandardInput.Write($"mysqldump -u root --password=rubestry030409 --compatible=ansi --skip-extended-insert --compact base_data > {dumpFilePath}");
    //        pro.StandardInput.WriteLine();
    //        pro.StandardInput.Flush();
    //        pro.StandardInput.Close();

    //        System.IO.StreamReader err = pro.StandardError;
    //        string b = "";

    //        while (b == "")
    //        {
    //            await Task.Delay(1000);
    //            b = await err.ReadLineAsync();
    //        }

    //        // 재가공
    //        {
    //            string str = string.Empty;

    //            using (StreamReader reader = new StreamReader(dumpFilePath))
    //            {
    //                str = await reader.ReadToEndAsync();
    //            }

    //            str.Trim();

    //            StringSpliter stringSpliter = new StringSpliter();
    //            stringSpliter.Split(str, "\r\n");

    //            List<string> list = new List<string>(stringSpliter.buffer);

    //            for (int index = 0; index < list.Count; index++)
    //            {
    //                while (list[index].StartsWith(" ")) // 문장 앞쪽 공백 제거
    //                {
    //                    list[index] = list[index].Remove(0, 1);
    //                }

    //                if (list[index].StartsWith("KEY") || list[index].StartsWith("CONSTRAINT")) // KEY 키워드 제거
    //                {
    //                    list[index] = string.Empty;

    //                    if (index > 0) // KEY 키워드 제거된 경우, 윗라인 마지막 줄에 ',' 있으면 제거
    //                    {
    //                        if (list[index - 1] != string.Empty && list[index - 1].EndsWith(","))
    //                        {
    //                            list[index - 1] = list[index - 1].Remove(list[index - 1].Length - 1, 1);
    //                        }
    //                    }
    //                }
    //            }

    //            using (StreamWriter writer = new StreamWriter(dumpFilePath, false))
    //            {
    //                foreach (string line in list)
    //                {
    //                    await writer.WriteLineAsync(line);
    //                }
    //            }
    //        }

    //        pro.Close();
    //    }
    //}

    //public class StringSpliter
    //{
    //    public List<string> buffer = new List<string>();

    //    /// <summary>            
    //    /// Returns a string array that contains the substrings in this instance that are delimited by 
    //    /// a specified string or Unicode character array.
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <param name="separator"></param>
    //    public void Split(string value, string separator)
    //    {
    //        buffer.Clear();

    //        int startIndex = 0;

    //        // Calculate the max index so we don't return false results during the index comparison
    //        // which is set to true at start, so if only one character is left and matches will remain true
    //        int maxIndex = value.Length - separator.Length;
    //        for (int i = 0; i < maxIndex; i++)
    //        {
    //            bool matchFound = true;
    //            for (int n = 0; n < separator.Length && (n + i) < value.Length; n++)
    //            {
    //                if (value[i + n] != separator[n])
    //                {
    //                    matchFound = false;
    //                    break;
    //                }
    //            }

    //            if (matchFound)
    //            {
    //                this.buffer.Add(value.Substring(startIndex, i + separator.Length - startIndex - separator.Length));
    //                startIndex = i + separator.Length;
    //                i += separator.Length - 1;
    //            }
    //        }

    //        this.buffer.Add(value.Substring(startIndex, value.Length - startIndex));
    //    }
    //}
}
