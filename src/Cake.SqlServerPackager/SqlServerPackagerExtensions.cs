using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.SqlServerPackager
{
    [CakeAliasCategory("Database")]
    public static class SqlServerPackagerExtensions
    {
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
