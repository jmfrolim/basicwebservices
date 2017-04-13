using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Two_Task_Async_Await
{
     class Program
    {
        protected static DateTime timestampStart;

        static void TimeStampStart()
        {
            timestampStart = DateTime.Now;
        }

        public static void TimeStamp(string temp)
        {
            long elapsed = (long)(DateTime.Now - timestampStart).TotalMilliseconds;
            Console.WriteLine("{0} : {1}", elapsed ,temp);
        }
        static Semaphore semaforo;
        static ListenerThreadHandler handler;

        static void Main(string[] args)
        {
            semaforo = new Semaphore(20, 20);
            handler = new ListenerThreadHandler();

            TimeStampStart();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Resquest #" + i);
                MakeRequest(i);
            }

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

        static async void MakeRequest(int i)
        {
            TimeStamp("MakeReqquest "+ i + "start thread ID: "+ Thread.CurrentThread.ManagedThreadId);
            string ret = await RequestIssuer.HttpGet("http://localhost/index.html");
            TimeStamp("MakeRequest" + i + " end Thread ID" + Thread.CurrentThread.ManagedThreadId);
        }
        static async void StartConnectionListenner(HttpListener listener)
        {
            TimeStamp("StartConnectionListenner Thread ID: "+ Thread.CurrentThread.ManagedThreadId);

            HttpListenerContext context = await listener.GetContextAsync();

            semaforo.Release();
            handler.Process(context);

        }
    }
}
