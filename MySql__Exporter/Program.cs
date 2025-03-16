using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql__Exporter
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Exporter exporter = new Exporter();

            await exporter.DumpDB(@"C:\Users\pppol\backup.sql");
        }
    }

    public class Exporter
    {
        ProcessStartInfo pri = new ProcessStartInfo();
        Process pro = new Process();

        public Exporter()
        {
            pri.FileName = @"cmd.exe";
            pri.CreateNoWindow = true;
            pri.UseShellExecute = false;

            pri.RedirectStandardInput = true;                //표준 출력을 리다이렉트
            pri.RedirectStandardOutput = true;
            pri.RedirectStandardError = true;

            pro.StartInfo = pri;

            pro.Start();   //어플리케이션 실행
        }

        public async Task DumpDB(string savePath)
        {
            ProcessStartInfo pri = new ProcessStartInfo();
            Process pro = new Process();

            pri.FileName = @"cmd.exe";
            pri.CreateNoWindow = true;
            pri.UseShellExecute = false;

            pri.RedirectStandardInput = true;                //표준 출력을 리다이렉트
            pri.RedirectStandardOutput = true;
            pri.RedirectStandardError = true;

            pro.StartInfo = pri;
            pro.Start();   //어플리케이션 실행

            pro.StandardInput.AutoFlush = false;
            pro.StandardInput.Write(@"cd C:\Program Files\MySQL\MySQL Server 8.0\bin");
            pro.StandardInput.WriteLine();
            pro.StandardInput.Write($"mysqldump -u root --password=rubestry030409 --compatible=ansi --compact --skip-add-locks --skip-comments base_data > {savePath}");
            pro.StandardInput.WriteLine();
            pro.StandardInput.Flush();

            System.IO.StreamReader err = pro.StandardError;
            string b = "";

            while (b == "")
            {
                await Task.Delay(1000);
                b = await err.ReadLineAsync();
            }

            pro.Close();

            // 재가공
            {
                string str = string.Empty;

                using (StreamReader reader = new StreamReader(savePath))
                {
                    str = await reader.ReadToEndAsync();
                }

                str.Trim();

                string[] split = str.Split('\n');

                for (int index = 0; index < split.Length; index++)
                {
                    while (split[index].StartsWith(" "))
                    {
                        split[index] = split[index].Remove(0, 1);
                    }

                    if (split[index].StartsWith("KEY") || split[index].StartsWith("CONSTRAINT"))
                    {
                        split[index] = string.Empty;

                        if (index > 0)
                        {
                            if (split[index - 1] != string.Empty && split[index - 1]?.Last() == ',')
                            {
                                split[index - 1] = split[index - 1].Remove(split[index - 1].Length - 2, 1);
                            }
                        }
                    }
                }

                // 쓰기
                string newString = string.Empty;

                for (int index = 0; index < split.Length; index++)
                {
                    newString = $"{newString}\n{split[index]}";
                }

                using (StreamWriter writer = new StreamWriter(savePath, false))
                {
                    await writer.WriteAsync(newString);
                }
            }
        }
    }
}
