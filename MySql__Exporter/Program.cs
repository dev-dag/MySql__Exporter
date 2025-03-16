using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            await exporter.Command();
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

        public async Task Command()
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
            pro.StandardInput.Write(@"mysqldump -u root --password=rubestry030409 --databases base_data > C:\Users\pppol\backup.sql");
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

            return;
        }
    }
}
