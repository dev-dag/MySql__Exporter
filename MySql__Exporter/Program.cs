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
        public const string PASSWORD = "your database password";
        public const string DB_SAVE_PATH = "sqlite save path";
        public const string DB_NAME = "your database name";
        public const string USER_NAME = "your user name";

        static async Task Main(string[] args)
        {
            Converter converter = new Converter(DB_SAVE_PATH, PASSWORD, DB_NAME, USER_NAME);

            await converter.Convert();
        }
    }

    public class Converter
    {
        private string dbSavePath;
        private string password;
        private string dbName;
        private string userName;

        public Converter(string dbPath, string password, string dbName, string userName)
        {
            this.dbSavePath = dbPath;
            this.password = password;
            this.dbName = dbName;
            this.userName = userName;
        }

        public async Task Convert()
        {
            ProcessStartInfo pri = new ProcessStartInfo()
            {
                FileName = @"cmd.exe",
                WorkingDirectory = @"C:\",
                CreateNoWindow = true,
                UseShellExecute = false,

                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Process pro = new Process();

            pro.StartInfo = pri;

            pro.Start();   // 어플리케이션 실행

            pro.StandardInput.AutoFlush = false;

            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            pro.StandardInput.WriteLine($"cd {localPath}\\Programs\\Python\\Python313\\Scripts");
            pro.StandardInput.Flush();

            pro.StandardInput.WriteLine($"mysql2sqlite -d {dbName} -u {userName} -f {dbSavePath}  --mysql-password {password}");
            pro.StandardInput.Flush();

            pro.StandardInput.Close();

            var output = await pro.StandardOutput.ReadToEndAsync();
            var err = await pro.StandardError.ReadToEndAsync();

            return;
        }
    }
}
