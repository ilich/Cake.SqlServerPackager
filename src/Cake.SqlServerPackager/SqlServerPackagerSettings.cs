namespace Cake.SqlServerPackager
{
    /// <summary>
    /// SQL Server packager settings
    /// </summary>
    public class SqlServerPackagerSettings
    {
        /// <summary>
        /// Gets or sets SQL scripts folder. It has to be inside Git repository
        /// if Git-based packaging is used.
        /// </summary>
        public string ScriptsFolder { get; set; }

        /// <summary>
        /// Gets or sets target SQL script full filename.
        /// </summary>
        public string TargetFilename { get; set; }

        /// <summary>
        /// Gets or sets Git Tag. Only SQL scripts added or modified after the tagged version
        /// will be packaged.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets Git commit. Only SQL scripts added or modified after the tagged version
        /// will be packaged. This option has higher priority than the tag.
        /// </summary>
        public string Commit { get; set; }

        /// <summary>
        /// Gets or sets whether user wants to overwrite existing SQL script or not.
        /// </summary>
        public bool OverwriteExistingScript { get; set; }

        /// <summary>
        /// Gets or sets SQL scripts extensions, e.g. '.sql'
        /// </summary>
        public string Extension { get; set; } = SqlServerPackagerRunner.SqlExtension;

        /// <summary>
        /// Gets or sets excluded commits
        /// </summary>
        public string[] ExcludedChagesets { get; set; } = new string[0];
    }
}
