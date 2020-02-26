# Nager.VideoStream
Get images from a video stream, requires [ffmpeg](https://www.ffmpeg.org/)

```cs
var streamUrl = "rtsp://192.168.0.10/stream1";
var cancellationTokenSource = new CancellationTokenSource();

var client = new VideoStreamClient();
client.NewImageReceived += NewImageReceived;
var task = client.StartFrameReaderAsync(streamUrl, OutputImageFormat.Bmp, cancellationTokenSource.Token);
client.NewImageReceived -= NewImageReceived;
cancellationTokenSource.Cancel();
```
