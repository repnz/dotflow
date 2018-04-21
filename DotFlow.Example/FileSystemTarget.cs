using System.IO;

namespace DotFlow.Example
{
    public class FileSystemTarget : ITarget<FileObject>
    {
        private string _directory;

        public FileSystemTarget(string directory)
        {
            _directory = directory;
        }

        public void Put(FileObject item)
        {
            string fileName = Path.Combine(_directory, item.Filename);
            File.WriteAllBytes(fileName, item.Contents);
        }
    }
}