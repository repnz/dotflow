using System.IO;

namespace DotFlow.Example
{
    public class FileSystemSource : QueuedSource<FileObject>
    {
        private readonly string _directory;

        public FileSystemSource(string directory, int maxBuffer) : base(maxBuffer)
        {
            _directory = directory;
        }

        protected override void LoadLoop()
        {
            foreach (string fileName in Directory.EnumerateFiles(_directory))
            {
                if (TryCreateFileObject(fileName, out FileObject fileObject))
                {
                    QueueInput(fileObject);
                }
            }
        }

        private bool TryCreateFileObject(string fileName, out FileObject fileObject)
        {
            try
            {
                byte[] content = File.ReadAllBytes(fileName);
                File.Delete(fileName);
                fileName = Path.GetFileName(fileName);
                fileObject = new FileObject(fileName, content);
                return true;
            }
            catch (IOException)
            {
                fileObject = default(FileObject);
                return false;
            }
        }

    }
}
