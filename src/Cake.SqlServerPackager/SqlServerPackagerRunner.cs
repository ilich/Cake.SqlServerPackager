using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Core.Diagnostics;
using LibGit2Sharp;
using static System.Console;

namespace Cake.SqlServerPackager
{
    /// <summary>
    /// SQL server scripts packager.
    /// </summary>
    public class SqlServerPackagerRunner
    {
        /// <summary>
        /// Default SQL script extension.
        /// </summary>
        internal const string SqlExtension = ".sql";

        /// <summary>
        /// Packager settings.
        /// </summary>
        protected readonly SqlServerPackagerSettings _settings;

        /// <summary>
        /// Create an instance of SqlServerPackagerRunner class.
        /// </summary>
        /// <param name="settings">Packager settings.</param>
        /// <param name="logger">Cake logger.</param>
        public SqlServerPackagerRunner(SqlServerPackagerSettings settings, ICakeLog logger)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            Logger.LogEngine = logger;
        }

        /// <summary>
        /// Package SQL scripts into one file.
        /// </summary>
        public virtual void Package()
        {
            IFilesProvider provider;
            if (string.IsNullOrWhiteSpace(_settings.Tag) && string.IsNullOrWhiteSpace(_settings.Commit))
            {
                provider = new DiskFilesProvider();
            }
            else
            {
                provider = new GitFilesProvider();
            }

            var files = provider.GetFiles(_settings);
            if (files?.Count > 0)
            {
                files.Sort();
                CreateSqlScript(files);
            }
        }

        /// <summary>
        /// Merge SQL scripts into one file.
        /// </summary>
        /// <param name="files">The list of files.</param>
        protected virtual void CreateSqlScript(List<string> files)
        {
            if (File.Exists(_settings.TargetFilename))
            {
                if (_settings.OverwriteExistingScript)
                {
                    File.Delete(_settings.TargetFilename);
                }
                else
                {
                    throw new InvalidOperationException($"{_settings.TargetFilename} exists. Please remove it and try againg. You can also use OverwriteExistingScript option to avoid this exception.");
                }
            }

            foreach(var file in files)
            {
                if (!File.Exists(file))
                {
                    Logger.Log($"{file} is not found");
                    continue;
                }

                var content = File.ReadAllText(file);
                File.AppendAllText(_settings.TargetFilename, $"-- {file} {Environment.NewLine}");
                File.AppendAllText(_settings.TargetFilename, content);
                File.AppendAllText(_settings.TargetFilename, $"{Environment.NewLine}GO{Environment.NewLine}{Environment.NewLine}");
                Logger.Log($"{file} has been processed");
            }

            Logger.Log($"SQL script has been stored to {_settings.TargetFilename}");
        }
    }
}
