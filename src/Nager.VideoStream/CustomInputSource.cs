namespace Nager.VideoStream
{
    public class CustomInputSource : InputSource
    {
        internal override string InputCommand { get; }

        public CustomInputSource(string customCommand)
        {
            this.InputCommand = customCommand;
        }
    }
}
