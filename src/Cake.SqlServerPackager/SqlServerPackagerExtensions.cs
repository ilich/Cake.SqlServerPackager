using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.SqlServerPackager
{
    /// <summary>
    /// Cake AddIn that packages multiple SQL Scripts into one file to speed up deployment and minimize the probability of errors.
    /// </summary>
    [CakeAliasCategory("Database")]
    public static class SqlServerPackagerExtensions
    {
        /// <summary>
        /// Merge multiple SQL scripts into one SQL script.
        /// </summary>
        /// <param name="context">Cake context.</param>
        /// <param name="settings">Settings.</param>
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
