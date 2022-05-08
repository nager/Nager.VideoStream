using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Nager.VideoStream.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists("frames"))
            {
                Directory.CreateDirectory("frames");
            }

            //var inputSource = new WebcamInputSource("Microsoft® LifeCam HD-3000");
            //var inputSource = new FileInputSource("myvideo.mp4");
            //var inputSource = new StreamInputSource("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4");
            var inputSource = new CustomInputSource("-rtsp_transport tcp -i rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4 -vf transpose=dir=1");
            

            var cancellationTokenSource = new CancellationTokenSource();

            _ = Task.Run(async () => await StartStreamProcessingAsync(inputSource, cancellationTokenSource.Token));
            Console.WriteLine("Press any key for stop");
            Console.ReadKey();
            cancellationTokenSource.Cancel();

            Console.WriteLine("Press any key for quit");
            Console.ReadKey();
        }

        private static async Task StartStreamProcessingAsync(InputSource inputSource, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Start Stream Processing");
            var client = new VideoStreamClient();
            client.NewImageReceived += NewImageReceived;
            client.FFmpegInfoReceived += Client_FFmpegInfoReceived;
            await client.StartFrameReaderAsync(inputSource, OutputImageFormat.Bmp, cancellationToken: cancellationToken);
            //client.NewImageReceived -= NewImageReceived;
            Console.WriteLine("End Stream Processing");
        }

        private static void Client_FFmpegInfoReceived(string obj)
        {
            Console.WriteLine(obj);
        }

        private static void NewImageReceived(byte[] imageData)
        {
            Console.WriteLine($"New image received, bytes:{imageData.Length}");
            File.WriteAllBytes($@"frames\{DateTime.Now.Ticks}.bmp", imageData);
        }
    }
}
