using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cake.SqlServerPackager
{
    /// <summary>
    /// Disk files provider.
    /// </summary>
    public class DiskFilesProvider : IFilesProvider
    {
        /// <summary>
        /// Returns the list of SQL scripts for packaging.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <returns>The list of files.</returns>
        public virtual List<string> GetFiles(SqlServerPackagerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.ScriptsFolder))
            {
                throw new InvalidOperationException("ScriptsFolder is not set");
            }

            if (!Directory.Exists(settings.ScriptsFolder))
            {
                throw new InvalidOperationException($"{settings.ScriptsFolder} is not found");
            }

            Logger.Log($"Looking for SQL scripts at {settings.ScriptsFolder}");

            var results = new List<string>();
            var folders = new Queue<DirectoryInfo>();
            folders.Enqueue(new DirectoryInfo(settings.ScriptsFolder));
            while (folders.Count > 0)
            {
                var working = folders.Dequeue();
                if (!working.Exists)
                {
                    continue;
                }

                results.AddRange(
                    working.GetFiles()
                        .Where(f => f.Extension.Equals(settings.Extension, StringComparison.InvariantCultureIgnoreCase))
                        .Select(f => f.FullName));

                foreach (var folder in working.GetDirectories())
                {
                    folders.Enqueue(folder);
                }
            }

            return results;
        }
    }
}
