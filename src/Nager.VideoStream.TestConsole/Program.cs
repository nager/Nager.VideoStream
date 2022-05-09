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

            // RTSP Test
            //https://github.com/aler9/rtsp-simple-server
            // 1. Start an own RTSP Test Server
            //  docker run --rm -it -e RTSP_PROTOCOLS=tcp -p 8554:8554 -p 1935:1935 -p 8888:8888 aler9/rtsp-simple-server
            // 2. Stream an own test video to the rtsp server via ffmpeg
            //  ffmpeg -re -stream_loop -1 -i MY_TEST_VIDEO.MP4 -c copy -f rtsp rtsp://localhost:8554/mystream
            // 3. Try stream the video
            //  rtsp://localhost:8554/mystream


            //var inputSource = new WebcamInputSource("Microsoft® LifeCam HD-3000");
            //var inputSource = new FileInputSource("myvideo.mp4");
            //var inputSource = new StreamInputSource("rtsp://localhost:8554/mystream");
            var inputSource = new CustomInputSource("-rtsp_transport tcp -i rtsp://localhost:8554/mystream -vf transpose=dir=1");
            
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
            try
            {
                var client = new VideoStreamClient();
                client.NewImageReceived += NewImageReceived;
                client.FFmpegInfoReceived += FFmpegInfoReceived;
                await client.StartFrameReaderAsync(inputSource, OutputImageFormat.Bmp, cancellationToken: cancellationToken);
                client.NewImageReceived -= NewImageReceived;
                client.FFmpegInfoReceived -= FFmpegInfoReceived;
                Console.WriteLine("End Stream Processing");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{exception}");
            }
        }

        private static void FFmpegInfoReceived(string ffmpegStreamInfo)
        {
            //frame=   77 fps=6.4 q=-0.0 size=  467779kB time=00:00:02.56 bitrate=1493004.8kbits/s dup=16 drop=0 speed=0.214x
            Console.WriteLine(ffmpegStreamInfo);
        }

        private static void NewImageReceived(byte[] imageData)
        {
            Console.WriteLine($"New image received, bytes:{imageData.Length}");
            File.WriteAllBytes($@"frames\{DateTime.Now.Ticks}.bmp", imageData);
        }
    }
}
