using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace One_BasicConcepts
{
    class Program
    {
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
                    StartConnectionListener(listener);
                }
            });

            Console.WriteLine();
            Console.ReadLine();
        }

        static async void StartConnectionListener(HttpListener listener)
        {
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
    }
}
