namespace DotFlow.Example
{
    public class FileObject
    {
        public string Filename { get; }
        public byte[] Contents { get; }

        public FileObject(string filename, byte[] contents)
        {
            Filename = filename;
            Contents = contents;
        }
    }
}