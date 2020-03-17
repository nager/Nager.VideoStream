using System.IO;

namespace Nager.VideoStream
{
    public class FileInputSource : InputSource
    {
        internal override string InputCommand { get; }

        public FileInputSource(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Cannot found file", filePath);
            }

            this.InputCommand = $"-i {filePath}";
        }
    }
}
