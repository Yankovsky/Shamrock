using System.IO;
using System.Threading;

namespace Shamrock_WebSite.Infrastructure
{
    //HACK
    public static class DirectoryExtensions
    {
        public static void DeleteDirectoryAndAllFilesInIt(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Thread.Sleep(50);
                DeleteDirectoryAndAllFilesInIt(path);
            }
        }
    }
}