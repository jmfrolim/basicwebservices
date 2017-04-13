using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Two_Task_Async_Await
{
    public class RequestIssuer
    {
        public static async Task<string> HttpGet(string url)
        {
            string ret;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        ret = await reader.ReadToEndAsync();
                    }
                }

            }
            catch (Exception exception)
            {
                ret = exception.Message;
            }

            return ret;
        }
    }
}
