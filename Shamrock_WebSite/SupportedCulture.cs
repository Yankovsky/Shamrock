using System.Collections.Generic;
using System.Threading;

namespace Shamrock_WebSite
{
    public static class SupportedCulture
    {
        public static string Ru = "ru";
        public static string En = "en";
        public static List<string> GetList()
        {
            var list = new List<string>();
            list.Add(En);
            list.Add(Ru);
            return list;
        }

        public static string Current
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }
    }
}