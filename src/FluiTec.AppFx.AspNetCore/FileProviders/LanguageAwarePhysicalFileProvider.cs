using System;
using System.IO;
using FluiTec.AppFx.AspNetCore.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using PhysicalFileInfo = Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo;

namespace FluiTec.AppFx.AspNetCore.FileProviders
{
    /// <summary>A language aware physical file provider.</summary>
    public class LanguageAwarePhysicalFileProvider : IFileProvider
    {
        /// <summary>The polling environment key.</summary>
        private const string PollingEnvironmentKey = "DOTNET_USE_POLLING_FILE_WATCHER";

        /// <summary>The path separators.</summary>
        private static readonly char[] PathSeparators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

        /// <summary>The files watcher.</summary>
        private readonly PhysicalFilesWatcher _filesWatcher;

        /// <summary>The filters.</summary>
        private readonly ExclusionFilters _filters;

        /// <summary>Gets or sets options for controlling the culture.</summary>
        /// <value>Options that control the culture.</value>
        public CultureOptions CultureOptions { get; set; }

        /// <summary>
        /// Initializes a new instance of a PhysicalFileProvider at the given root directory.
        /// </summary>
        /// <param name="root">The root directory. This should be an absolute path.</param>
        public LanguageAwarePhysicalFileProvider(string root)
            : this(root, CreateFileWatcher(root, ExclusionFilters.Sensitive), ExclusionFilters.Sensitive)
        {
        }

        /// <summary>
        /// Initializes a new instance of a PhysicalFileProvider at the given root directory.
        /// </summary>
        /// <param name="root">The root directory. This should be an absolute path.</param>
        /// <param name="filters">Specifies which files or directories are excluded.</param>
        public LanguageAwarePhysicalFileProvider(string root, ExclusionFilters filters)
            : this(root, CreateFileWatcher(root, filters), filters)
        { }

        // for testing
        internal LanguageAwarePhysicalFileProvider(string root, PhysicalFilesWatcher physicalFilesWatcher)
            : this(root, physicalFilesWatcher, ExclusionFilters.Sensitive)
        { }

        /// <summary>Constructor.</summary>
        /// <exception cref="ArgumentException">            Thrown when one or more arguments have
        ///                                                 unsupported or illegal values. </exception>
        /// <exception cref="DirectoryNotFoundException">   Thrown when the requested directory is not
        ///                                                 present. </exception>
        /// <param name="root">                 The root directory. This should be an absolute path. </param>
        /// <param name="physicalFilesWatcher"> The physical files watcher. </param>
        /// <param name="filters">              Specifies which files or directories are excluded. </param>
        private LanguageAwarePhysicalFileProvider(string root, PhysicalFilesWatcher physicalFilesWatcher, ExclusionFilters filters)
        {
            if (!Path.IsPathRooted(root))
            {
                throw new ArgumentException("The path must be absolute.", nameof(root));
            }
            var fullRoot = Path.GetFullPath(root);
            // When we do matches in GetFullPath, we want to only match full directory names.
            Root = PathUtils.EnsureTrailingSlash(fullRoot);
            if (!Directory.Exists(Root))
            {
                throw new DirectoryNotFoundException(Root);
            }

            _filesWatcher = physicalFilesWatcher;
            _filters = filters;
        }

        /// <summary>Creates file watcher.</summary>
        /// <param name="root">     The root directory. This should be an absolute path. </param>
        /// <param name="filters">  Specifies which files or directories are excluded. </param>
        /// <returns>The new file watcher.</returns>
        private static PhysicalFilesWatcher CreateFileWatcher(string root, ExclusionFilters filters)
        {
            var environmentValue = Environment.GetEnvironmentVariable(PollingEnvironmentKey);
            var pollForChanges = string.Equals(environmentValue, "1", StringComparison.Ordinal) ||
                                 string.Equals(environmentValue, "true", StringComparison.OrdinalIgnoreCase);

            root = PathUtils.EnsureTrailingSlash(Path.GetFullPath(root));
            return new PhysicalFilesWatcher(root, new FileSystemWatcher(root), pollForChanges, filters);
        }

        /// <summary>
        /// Disposes the provider. Change tokens may not trigger after the provider is disposed.
        /// </summary>
        public void Dispose()
        {
            _filesWatcher.Dispose();
        }

        /// <summary>
        /// The root directory for this instance.
        /// </summary>
        public string Root { get; }

        /// <summary>Gets full path.</summary>
        /// <param name="path"> Full pathname of the file. </param>
        /// <returns>The full path.</returns>
        private string GetFullPath(string path)
        {
            if (PathUtils.PathNavigatesAboveRoot(path))
            {
                return null;
            }

            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(Path.Combine(Root, path));
            }
            catch
            {
                return null;
            }

            if (!IsUnderneathRoot(fullPath))
            {
                return null;
            }

            return fullPath;
        }

        /// <summary>Query if 'fullPath' is underneath root.</summary>
        /// <param name="fullPath"> Full pathname of the full file. </param>
        /// <returns>True if underneath root, false if not.</returns>
        private bool IsUnderneathRoot(string fullPath)
        {
            return fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Locate a file at the given path by directly mapping path segments to physical directories.
        /// </summary>
        /// <param name="subpath">A path under the root directory</param>
        /// <returns>The file information. Caller must check <see cref="IFileInfo.Exists"/> property. </returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            subpath = AdjustSubpath(subpath);

            if (string.IsNullOrEmpty(subpath) || PathUtils.HasInvalidPathChars(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            // Relative paths starting with leading slashes are okay
            subpath = subpath.TrimStart(PathSeparators);

            // Absolute paths not permitted.
            if (Path.IsPathRooted(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            var fullPath = GetFullPath(subpath);
            if (fullPath == null)
            {
                return new NotFoundFileInfo(subpath);
            }

            var fileInfo = new FileInfo(fullPath);
            if (FileSystemInfoHelper.IsExcluded(fileInfo, _filters))
            {
                return new NotFoundFileInfo(subpath);
            }

            var info = new PhysicalFileInfo(fileInfo);
            return info;
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// </summary>
        /// <param name="subpath">A path under the root directory. Leading slashes are ignored.</param>
        /// <returns>
        /// Contents of the directory. Caller must check <see cref="IDirectoryContents.Exists"/> property. <see cref="NotFoundDirectoryContents" /> if
        /// <paramref name="subpath" /> is absolute, if the directory does not exist, or <paramref name="subpath" /> has invalid
        /// characters.
        /// </returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            subpath = AdjustSubpath(subpath);
            try
            {
                if (subpath == null || PathUtils.HasInvalidPathChars(subpath))
                {
                    return NotFoundDirectoryContents.Singleton;
                }

                // Relative paths starting with leading slashes are okay
                subpath = subpath.TrimStart(PathSeparators);

                // Absolute paths not permitted.
                if (Path.IsPathRooted(subpath))
                {
                    return NotFoundDirectoryContents.Singleton;
                }

                var fullPath = GetFullPath(subpath);
                if (fullPath == null || !Directory.Exists(fullPath))
                {
                    return NotFoundDirectoryContents.Singleton;
                }

                return new PhysicalDirectoryContents(fullPath, _filters);
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (IOException)
            {
            }
            return NotFoundDirectoryContents.Singleton;
        }

        /// <summary>
        ///     <para>Creates a <see cref="IChangeToken" /> for the specified <paramref name="filter" />.</para>
        ///     <para>Globbing patterns are interpreted by <seealso cref="Microsoft.Extensions.FileSystemGlobbing.Matcher" />.</para>
        /// </summary>
        /// <param name="filter">
        /// Filter string used to determine what files or folders to monitor. Example: **/*.cs, *.*,
        /// subFolder/**/*.cshtml.
        /// </param>
        /// <returns>
        /// An <see cref="IChangeToken" /> that is notified when a file matching <paramref name="filter" /> is added,
        /// modified or deleted. Returns a <see cref="NullChangeToken" /> if <paramref name="filter" /> has invalid filter
        /// characters or if <paramref name="filter" /> is an absolute path or outside the root directory specified in the
        /// constructor <seealso cref="PhysicalFileProvider(string)" />.
        /// </returns>
        public IChangeToken Watch(string filter)
        {
            if (filter == null || PathUtils.HasInvalidFilterChars(filter))
            {
                return NullChangeToken.Singleton;
            }

            // Relative paths starting with leading slashes are okay
            filter = filter.TrimStart(PathSeparators);

            return _filesWatcher.CreateFileChangeToken(filter);
        }

        /// <summary>Adjust subpath.</summary>
        /// <param name="subPath">  Full pathname of the sub file. </param>
        /// <returns>A string.</returns>
        private string AdjustSubpath(string subPath)
        {
            if (string.IsNullOrWhiteSpace(subPath))
                return subPath;

            foreach (var culture in CultureOptions.SupportedCultures)
            {
                var c1 = $"{culture}/";
                var c2 = $"/{culture}/";

                if (subPath.StartsWith(c1))
                {
                    var adjusted = subPath.Substring(culture.Length + 1);
                    return adjusted;
                }
                if (subPath.StartsWith(c2))
                {
                    var adjusted = subPath.Substring(culture.Length + 1);
                    return adjusted;
                }
            }

            return subPath;
        }
    }

    /// <summary>
    /// Specifies filtering behavior for files or directories.
    /// </summary>
    [Flags]
    public enum ExclusionFilters
    {
        /// <summary>
        /// Equivalent to <c>DotPrefixed | Hidden | System</c>. Exclude files and directories when the name begins with a period, or has either <see cref="FileAttributes.Hidden"/> or <see cref="FileAttributes.System"/> is set on <see cref="FileSystemInfo.Attributes"/>.
        /// </summary>
        Sensitive = DotPrefixed | Hidden | System,

        /// <summary>
        /// Exclude files and directories when the name begins with period.
        /// </summary>
        DotPrefixed = 0x0001,

        /// <summary>
        /// Exclude files and directories when <see cref="FileAttributes.Hidden"/> is set on <see cref="FileSystemInfo.Attributes"/>.
        /// </summary>
        Hidden = 0x0002,

        /// <summary>
        /// Exclude files and directories when <see cref="FileAttributes.System"/> is set on <see cref="FileSystemInfo.Attributes"/>.
        /// </summary>
        System = 0x0004,

        /// <summary>
        /// Do not exclude any files.
        /// </summary>
        None = 0
    }
}
