using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Shamrock_WebSite.Services
{
    public static class SMSService
    {
        public static bool TrySendSMS(string text, string phoneNumber)
        {
            try
            {            
                var requestUriString = String.Format("http://smspilot.ru/api.php?send={0}&to={1}&from=Shamrock&apikey=Y2HMU0OM24ICN21WRQH10466585W3NMS4D48OFFD8RWPQ74HU6787NVS61YRX692", text, phoneNumber);
                var request = WebRequest.Create(requestUriString);

                var response = request.GetResponse();
                byte[] buffer = new byte[512];
                var responseStream = response.GetResponseStream();
                var responseBody = "";
                int count = 0;
                do
                {
                    count = responseStream.Read(buffer, 0, buffer.Length);
                    if (count != 0)
                        responseBody += Encoding.ASCII.GetString(buffer, 0, count);
                }
                while (count > 0);
                if (Regex.IsMatch(responseBody, "SUCCESS=SMS SENT"))
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}