namespace SensorHubModule
{
    //    using Microsoft.Azure.Devices.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    class Program
    {
//        static ModuleClient client = null;

        static public async void Configure(IApplicationBuilder app)
        {
//            AmqpTransportSettings amqpSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
//            ITransportSettings[] settings = { amqpSetting };

            // Open a connection to the Edge runtime
//            client = await ModuleClient.CreateFromEnvironmentAsync(settings);
//            await client.OpenAsync();

            await Task.Delay(50);
            Console.WriteLine("IoT Hub module client initialized.");

            app.Run(async (context) => {
                Console.WriteLine(context.Request.Path);

                if (context.Request.ContentType != null)
                {
                    Console.WriteLine("Content-Type: {0}", context.Request.ContentType);
                }
                Console.WriteLine("Content-Length: {0}", context.Request.ContentLength);

                string inputBody;
                using (var reader = new StreamReader(context.Request.Body, System.Text.Encoding.UTF8))
                {
                    inputBody = await reader.ReadToEndAsync();
                }

                Console.WriteLine("-----START-BODY-CONTENT-----");
                Console.WriteLine(inputBody);
                Console.WriteLine("-----END-BODY-CONTENT-----");

//            byte[] messageBytes = Encoding.UTF8.GetBytes(s);
//            Message msg = new Message(messageBytes);
//            client.SendEventAsync("output1", msg).Wait();
//            Console.WriteLine("message sent");

                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("OK");
            });
        }

        static void Main(string[] args)
        {
            new WebHostBuilder().UseKestrel().Configure(Configure).UseUrls("http://+:80").Build().Run();
        }
    }
}
