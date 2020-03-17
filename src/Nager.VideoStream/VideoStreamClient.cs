using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nager.VideoStream
{
    public class VideoStreamClient
    {
        public event Action<byte[]> NewImageReceived;
        public event Action<string> FFmpegInfoReceived;

        private readonly string _ffmpegPath;

        public VideoStreamClient(string ffmpegPath = "ffmpeg.exe")
        {
            if (!File.Exists(ffmpegPath))
            {
                throw new FileNotFoundException("Cannot found ffmpeg", ffmpegPath);
            }

            this._ffmpegPath = ffmpegPath;
        }

        public async Task StartFrameReaderAsync(InputSource inputSource, OutputImageFormat outputImageFormat, CancellationToken cancellationToken)
        {
            var inputArgs = $"-y {inputSource.InputCommand}";
            var outputArgs = $"-c:v {outputImageFormat.ToString().ToLower()} -f image2pipe -";

            var startInfo = new ProcessStartInfo
            {
                FileName = this._ffmpegPath,
                Arguments = $"{inputArgs} {outputArgs}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };

            using (var ffmpegProcess = new Process { StartInfo = startInfo })
            {
                ffmpegProcess.ErrorDataReceived += this.ProcessDataReceived;

                ffmpegProcess.Start();
                ffmpegProcess.BeginErrorReadLine();

                using (var frameOutputStream = ffmpegProcess.StandardOutput.BaseStream)
                {
                    var index = 0;
                    var buffer = new byte[32768];
                    var imageData = new List<byte>();
                    byte[] imageHeader = null;

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var length = await frameOutputStream.ReadAsync(buffer, 0, buffer.Length);
                        if (length == 0)
                        {
                            break;
                        }

                        if (imageHeader == null)
                        {
                            imageHeader = buffer.Take(5).ToArray();
                        }

                        if (buffer.Take(5).SequenceEqual(imageHeader))
                        {
                            if (imageData.Count > 0)
                            {
                                this.NewImageReceived?.Invoke(imageData.ToArray());
                                imageData.Clear();
                                index++;
                            }
                        }

                        imageData.AddRange(buffer.Take(length));
                    }

                    frameOutputStream.Close();
                }

                ffmpegProcess.ErrorDataReceived -= this.ProcessDataReceived;

                ffmpegProcess.WaitForExit(1000);

                if (!ffmpegProcess.HasExited)
                {
                    ffmpegProcess.Kill();
                }
            }
        }

        private void ProcessDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.FFmpegInfoReceived?.Invoke(e.Data);
        }
    }
}
