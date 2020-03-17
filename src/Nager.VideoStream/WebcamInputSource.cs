namespace Nager.VideoStream
{
    public class WebcamInputSource : InputSource
    {
        internal override string InputCommand { get; }

        public WebcamInputSource(string busDeviceName)
        {
            this.InputCommand = $"-f dshow -i video=\"{busDeviceName}\"";
        }
    }
}
