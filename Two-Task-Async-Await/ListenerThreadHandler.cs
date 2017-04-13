using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Two_Task_Async_Await
{
    class ListenerThreadHandler  //CommonHandler, IRequestHandler
    {
        public void Process(HttpListenerContext context)
        {
            Program.TimeStamp("Process Thread ID:" + Thread.CurrentThread.ManagedThreadId);
            CommonResponse(context);
        }

        public void CommonResponse(HttpListenerContext context)
        {
            //delay
            Thread.Sleep(1000);
            //Get request
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string path = request.RawUrl.LeftOf("?").RightOf("/");

            string text = File.ReadAllText(path);
            byte[] data = Encoding.UTF8.GetBytes(text);
            response.ContentType = "text/html";
            response.ContentLength64 = data.Length;
            response.OutputStream.Write(data,0,data.Length);
            response.ContentEncoding = Encoding.UTF8;
            response.StatusCode = 200;
            response.OutputStream.Close();

        }
    }
}
