using System.IO;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace FluiTec.AppFx.AspNetCore.FileProviders
{
    internal static class PathUtils
    {
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars()
            .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar).ToArray();

        private static readonly char[] InvalidFilterChars = InvalidFileNameChars
            .Where(c => c != '*' && c != '|' && c != '?').ToArray();

        private static readonly char[] PathSeparators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

        internal static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(InvalidFileNameChars) != -1;
        }

        internal static bool HasInvalidFilterChars(string path)
        {
            return path.IndexOfAny(InvalidFilterChars) != -1;
        }

        internal static string EnsureTrailingSlash(string path)
        {
            if (!string.IsNullOrEmpty(path) &&
                path[path.Length - 1] != Path.DirectorySeparatorChar)
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        internal static bool PathNavigatesAboveRoot(string path)
        {
            var tokenizer = new StringTokenizer(path, PathSeparators);
            var depth = 0;

            foreach (var segment in tokenizer)
            {
                if (segment.Equals(".") || segment.Equals(""))
                {
                    continue;
                }

                if (segment.Equals(".."))
                {
                    depth--;

                    if (depth == -1)
                    {
                        return true;
                    }
                }
                else
                {
                    depth++;
                }
            }

            return false;
        }
    }
}