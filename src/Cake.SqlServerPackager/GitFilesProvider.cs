using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace Cake.SqlServerPackager
{
    /// <summary>
    /// Git files provider.
    /// </summary>
    public class GitFilesProvider : IFilesProvider
    {
        /// <summary>
        /// Returns the list of SQL scripts for packaging.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <returns>The list of files.</returns>
        public virtual List<string> GetFiles(SqlServerPackagerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Tag) && string.IsNullOrWhiteSpace(settings.Commit))
            {
                throw new InvalidOperationException("You should provide Tag or Commit to load changes from Git");
            }

            using (var git = new Repository(settings.ScriptsFolder))
            {
                var files = new List<string>();
                var lastCommit = FindCommit(git, settings);
                if (string.IsNullOrWhiteSpace(lastCommit))
                {
                    throw new InvalidOperationException("Tag and Commit are not found");
                }

                Logger.Log($"Processing changes till {lastCommit}");
                var filter = new CommitFilter { ExcludeReachableFrom = lastCommit };
                var commits = git.Commits.QueryBy(filter).ToArray();
                if (commits.Length == 0)
                {
                    return files;
                }
                else if (commits.Length == 1)
                {
                    var newCommit = commits.First();
                    var changes = git.Diff.Compare<TreeChanges>(newCommit.Parents.First().Tree, newCommit.Tree);
                    files.AddRange(ProcessTreeChanges(changes, settings));
                }
                else
                {
                    var newCommit = commits[0];
                    for (int i = 1; i < commits.Length; i++)
                    {
                        var oldCommit = commits[i];
                        if (settings.ExcludedChagesets.Contains(oldCommit.Sha))
                        {
                            // Skip the change completely
                            i++;
                            if (i == commits.Length)
                            {
                                // It was the last commit which we're going to skip.
                                break;
                            }

                            newCommit = commits[i];
                            continue;
                        }

                        var changes = git.Diff.Compare<TreeChanges>(oldCommit.Tree, newCommit.Tree);
                        files.AddRange(ProcessTreeChanges(changes, settings));
                        newCommit = oldCommit;
                    }

                }

                return files.Distinct().ToList();
            }
        }

        /// <summary>
        /// Extract filenames from Git changes.
        /// </summary>
        /// <param name="changes">Git changes.</param>
        /// <param name="settings">Settings.</param>
        /// <returns>The list of files.</returns>
        protected virtual List<string> ProcessTreeChanges(TreeChanges changes, SqlServerPackagerSettings settings)
        {
            var files = new List<string>();

            foreach (var change in changes)
            {
                switch (change.Status)
                {
                    case ChangeKind.Added:
                    case ChangeKind.Modified:
                    case ChangeKind.Renamed:
                    case ChangeKind.Copied:
                        var ext = Path.GetExtension(change.Path);
                        if (ext.Equals(settings.Extension, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var filename = Path.Combine(settings.ScriptsFolder, change.Path);
                            files.Add(filename);
                        }

                        break;
                }
            }

            return files;
        }

        /// <summary>
        /// Find target Git changest.
        /// </summary>
        /// <param name="git">Repository.</param>
        /// <param name="settings">Settings.</param>
        /// <returns>Target changeset hash.</returns>
        protected virtual string FindCommit(Repository git, SqlServerPackagerSettings settings)
        {
            string commit = null;

            if (!string.IsNullOrWhiteSpace(settings.Tag))
            {
                var tag = git.Tags[settings.Tag];
                commit = tag.Target.Sha;
            }

            if (!string.IsNullOrWhiteSpace(settings.Commit))
            {
                commit = settings.Commit;
            }

            return commit;
        }
    }
}
