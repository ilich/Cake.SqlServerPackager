# Cake.SqlServerPackager
Cake AddIn that packages multiple SQL Scripts into one file to speed up deployment and minimize the probability of errors.

## Example usage

```cs
#addin Cake.SqlServerPackager

SqlServerPackager(new SqlServerPackagerSettings
{
    ScriptsFolder = "C:\project\sample",
    TargetFilename = "C:\project\sample\out-sql\script.sql",
    OverwriteExistingScript = true,
    Tag = "1.0.0"
});
```

SQL scripts are ordered by their path in the same way as in Windows explorer.

## Options

**ScriptsFolder** - Gets or sets SQL scripts folder. It has to be inside Git repository if Git-based packaging is used.

**TargetFilename** - Gets or sets target SQL script full filename.

**Tag** - Gets or sets Git Tag. Only SQL scripts added or modified after the tagged version will be packaged.

**Commit** - Gets or sets Git commit. Only SQL scripts added or modified after the tagged version will be packaged. This option has higher priority than the tag.

**OverwriteExistingScript** - Gets or sets whether user wants to overwrite existing SQL script or not.

**Extension** - Gets or sets SQL scripts extensions. Default value: '.sql'.

**ExcludedChagesets** - Gets or sets excluded commits.