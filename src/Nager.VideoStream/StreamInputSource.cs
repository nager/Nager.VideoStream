using System;

namespace Nager.VideoStream
{
    public class StreamInputSource : InputSource
    {
        internal override string InputCommand { get; }

        public StreamInputSource(Uri streamUri)
        {
            this.InputCommand = $"-i {streamUri}";
        }

        public StreamInputSource(string streamUri)
        {
            this.InputCommand = $"-i {streamUri}";
        }
    }
}
