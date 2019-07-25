namespace SensorModule
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;

    class Program
    {
        static int counter = 0;
        static Timer timer = null;
        static HttpClient client = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Creating CancellationToken");
            var cts = new CancellationTokenSource();

            Console.WriteLine("Calling Init");
            Init(cts);

            // Wait until the app unloads or is cancelled
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();

            Console.WriteLine("Waiting for Cancel");
            WhenCancelled(cts.Token).Wait();

            Console.WriteLine("Exiting Main");
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Initializes the ModuleClient and sets up the callback to receive
        /// messages containing temperature information
        /// </summary>
        static void Init(CancellationTokenSource cts)
        {
            client = new HttpClient();

            timer = new System.Threading.Timer(async (e) =>
            {
                if (!cts.Token.IsCancellationRequested)
                {
                    string data = string.Format("{{\"key\": \"{0}\"}}", counter);
                    counter += 1;

                    HttpResponseMessage response = await client.PostAsync("http://localhost:8080/data/", new StringContent(data, Encoding.UTF8, "application/json"));

                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("{0} {1}", response.StatusCode, content);

                }
                else
                {
                    Console.WriteLine("Cancellation Detected, disposing timer.");
                    timer.Dispose();
                }
            }, null, 0, 5001);
        }
    }
}
