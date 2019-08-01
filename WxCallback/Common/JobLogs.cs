using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace Common
{
    public class JobLogs
    {
        public static void Writer(string directory, string message)
        {
            try
            {
                return;
                string systemPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string dirPath = Path.Combine(systemPath, "Logs", directory);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                //写入日志
                string fileName = "log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                string filePath = Path.Combine(dirPath, fileName);
                using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "|" + message);
                    sw.Close();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}