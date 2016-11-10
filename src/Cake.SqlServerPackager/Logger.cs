using System;
using Cake.Core.Diagnostics;

namespace Cake.SqlServerPackager
{
    /// <summary>
    /// Add-in logger.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Gets or sets logger engine.
        /// </summary>
        public static ICakeLog LogEngine { get; set; }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">Message.</param>
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
