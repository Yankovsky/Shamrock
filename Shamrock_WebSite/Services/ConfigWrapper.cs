using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Shamrock_WebSite.Services
{
    public static class ConfigWrapper
    {
        private const string _configFile = "App_Data/Config.xml";
        private static string _configPath;

        public static void SetServerPath(string serverPath)
        {
            _configPath = Path.Combine(serverPath, _configFile);
        }

        public static XDocument Config
        {
            get
            {
                return XDocument.Load(_configPath);
            }
        }
    }
}