using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml;
using System.IO;

namespace Shamrock_WebSite.Services
{
    public static class SMSService
    {
        public static bool TryToLogin()
        {
            var requestUriString = String.Format("http://api.sms24×7.ru/?method=login&email={0}&password={1}");
            return true;
        }

        public static bool TrySendSMS(string text, string phoneNumber)
        {
            var requestUriString = String.Format("http://api.sms24x7.ru/?method=push_msg&text={0}&phone={1}&sender_name=Shamrock", text, phoneNumber);
            var request = WebRequest.Create(requestUriString);
            var response = request.GetResponse();
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var xml = new XmlDocument();
                xml.Load(stream);
                return true;
            }
        }
    }
}