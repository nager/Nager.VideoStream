# Nager.VideoStream
Get images from a video stream, requires [ffmpeg](https://www.ffmpeg.org/)

## How can I use it?

The package is available on [nuget](https://www.nuget.org/packages/Nager.VideoStream)
```
PM> install-package Nager.VideoStream
```

## Code Example
```cs
var streamUrl = "rtsp://192.168.0.10/stream1";
var cancellationTokenSource = new CancellationTokenSource();

var client = new VideoStreamClient();
client.NewImageReceived += NewImageReceived;
var task = client.StartFrameReaderAsync(streamUrl, OutputImageFormat.Bmp, cancellationTokenSource.Token);
client.NewImageReceived -= NewImageReceived;
cancellationTokenSource.Cancel();

private static void NewImageReceived(byte[] imageData)
{
    File.WriteAllBytes($@"{DateTime.Now.Ticks}.bmp", imageData);
}
```
