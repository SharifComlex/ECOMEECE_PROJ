using System;
using System.IO;

namespace Microflake.Core.Utilities.Logger
{
    public class Logger : ILogger
    {
        public void Log(Exception ex)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "/Public/ErrorLog.txt"))
                {
                    sw.WriteLine("DateTime : " + DateTime.Now + Environment.NewLine);
                    if (ex.Message != null)
                    {
                        sw.WriteLine(Environment.NewLine + "Message" + ex.Message);
                        sw.WriteLine(Environment.NewLine + "StackTrace" + ex.StackTrace);
                    }
                again: if (ex.InnerException != null)
                    {
                        sw.WriteLine(Environment.NewLine + "Inner Exception : " + ex.InnerException.Message);
                    }
                    if (ex.InnerException.InnerException != null)
                    {
                        ex = ex.InnerException;
                        goto again;
                    }

                    sw.WriteLine("------******------");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
