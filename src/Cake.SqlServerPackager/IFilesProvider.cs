using System.Collections.Generic;

namespace Cake.SqlServerPackager
{
    /// <summary>
    /// Files provider interface.
    /// </summary>
    public interface IFilesProvider
    {
        /// <summary>
        /// Returns the list of SQL scripts for packaging.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <returns>The list of files.</returns>
        List<string> GetFiles(SqlServerPackagerSettings settings);
    }
}
