using System;
using Cake.Core.Diagnostics;

namespace Cake.SqlServerPackager
{
    public static class Logger
    {
        public static ICakeLog LogEngine { get; set; }

        public static void Log(string message)
        {
            var text = $"Cake.SqlServerPackager: {message}";

            if (LogEngine == null)
            {
                Console.WriteLine(text);
            }
            else
            {
                LogEngine.Write(Verbosity.Normal, LogLevel.Information, text);
            }
            
        }
    }
}
