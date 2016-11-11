using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.SqlServerPackager
{
    /// <summary>
    /// Cake AddIn that packages multiple SQL Scripts into one file to speed up deployment and minimize the probability of errors.
    /// <code>
    /// #addin Cake.SqlServerPackager
    /// </code>
    /// </summary>
    [CakeAliasCategory("Database")]
    public static class SqlServerPackagerExtensions
    {
        /// <summary>
        /// Merge multiple SQL scripts into one SQL script.
        /// </summary>
        /// <param name="context">Cake context.</param>
        /// <param name="settings">Settings.</param>
        /// <example>
        /// <code>
        /// SqlServerPackager(new SqlServerPackagerSettings
        /// {
        ///     ScriptsFolder = "C:\project\sample",
        ///     TargetFilename = "C:\project\sample\out-sql\script.sql",
        ///     OverwriteExistingScript = true,
        ///     Tag = "1.0.0"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void SqlServerPackager(this ICakeContext context, SqlServerPackagerSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new SqlServerPackagerRunner(settings, context.Log);
            runner.Package();
        }
    }
}
