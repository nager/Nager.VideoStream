# Nager.VideoStream
Get images from a video stream, requires [ffmpeg](https://www.ffmpeg.org/). You can download the [ffmpeg binary](https://github.com/BtbN/FFmpeg-Builds/releases) here, they are needed to access the video stream. Just copy the `ffmpeg.exe` into the execution directory.

## How can I use it?

The package is available on [nuget](https://www.nuget.org/packages/Nager.VideoStream)
```
PM> install-package Nager.VideoStream
```

## Code Example

### Network Camera (RTSP Stream)
```cs
var inputSource = new StreamInputSource("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");

var cancellationTokenSource = new CancellationTokenSource();

var client = new VideoStreamClient();
client.NewImageReceived += NewImageReceived;
var task = client.StartFrameReaderAsync(inputSource, OutputImageFormat.Bmp, cancellationTokenSource.Token);

//wait for exit
Console.ReadLine();

client.NewImageReceived -= NewImageReceived;

void NewImageReceived(byte[] imageData)
{
    File.WriteAllBytes($@"{DateTime.Now.Ticks}.bmp", imageData);
}
```

### Webcam
You can find out the name of your webcam in the `Windows Device Manager` in the section `Cameras`<br>
![Windows Device Manager](/doc/WindowsDeviceManager.png)

```cs
var inputSource = new WebcamInputSource("HP HD Camera");

var cancellationTokenSource = new CancellationTokenSource();

var client = new VideoStreamClient();
client.NewImageReceived += NewImageReceived;
var task = client.StartFrameReaderAsync(inputSource, OutputImageFormat.Bmp, cancellationTokenSource.Token);

//wait for exit
Console.ReadLine();

client.NewImageReceived -= NewImageReceived;

void NewImageReceived(byte[] imageData)
{
    File.WriteAllBytes($@"{DateTime.Now.Ticks}.bmp", imageData);
}
```
