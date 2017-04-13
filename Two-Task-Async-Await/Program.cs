using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using One_BasicConcepts;

namespace Two_Task_Async_Await
{
    class Program
    {
        protected static DateTime timestampStart;

        static void TimeStampStart()
        {
            timestampStart = DateTime.Now;
        }

        static void TimeStamp(string temp)
        {
            long elapsed = (long)(DateTime.Now - timestampStart).TotalMilliseconds;
            Console.WriteLine("{0} : {1}", elapsed ,temp);
        }

        static async void StartConnectionListenner(HttpListener listener)//StartConnectionListener StartConnectionListenner
        {
            TimeStamp("StartConnectionListenner Thread ID: " + Thread.CurrentThread.ManagedThreadId);
           
            HttpListenerContext context = await listener.GetContextAsync();

            semaforo.Release();

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;


            string path = request.RawUrl.LeftOf("?").RightOf("/");
            Console.WriteLine(path);

            try
            {
                string text = File.ReadAllText(path);
                byte[] data = Encoding.UTF8.GetBytes(text);
                response.ContentType = "text/html";
                response.ContentLength64 = data.Length;
                response.OutputStream.Write(data, 0, data.Length);
                response.ContentEncoding = Encoding.UTF8;
                response.StatusCode = 200;
                response.OutputStream.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            

        }
        static Semaphore semaforo;

        static void Main(string[] args)
        {
            semaforo = new Semaphore(20, 20);
            HttpListener listener = new HttpListener();
            string url = "http://localhost/";
            listener.Prefixes.Add(url);
            listener.Start();

            Task.Run(() =>
            {
                while (true)
                {
                    semaforo.WaitOne();
                    StartConnectionListenner(listener);
                }
            });

            Console.WriteLine();
            Console.ReadLine();
        }

      


    }
}
