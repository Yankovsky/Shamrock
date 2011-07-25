using System.IO;
namespace Shamrock_WebSite.Infrastructure
{
    public static class PathExtensions
    {
        public static string GetFullPathWithoutExtension(string path)
        {
            return path.Remove(GetExtensionDotIndex(path));
        }

        public static string InsertStringBeforeExtension(string path, string insertString)
        {
            return path.Insert(GetExtensionDotIndex(path), insertString);
        }

        public static string GetDirectoryPath(string path)
        {
            var fileName = Path.GetFileName(path);
            return path.Replace(fileName , "");
        }

        private static int GetExtensionDotIndex(string path)
        {
            return path.LastIndexOf('.');
        }
    }
}