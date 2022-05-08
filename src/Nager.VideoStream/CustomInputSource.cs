namespace Nager.VideoStream
{
    public class CustomInputSource : InputSource
    {

        internal override string InputCommand { get; }

        public CustomInputSource(string CustomCommand)
        {
            InputCommand = CustomCommand;
        }
    }
}
