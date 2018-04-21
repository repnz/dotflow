using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DotFlow
{
    public abstract class QueuedSource<T> : ISource<T>
    {
        private readonly BlockingCollection<T> _inputQueue;
        private readonly Task _workerTask;
        private bool _isRunning;

        protected abstract void LoadLoop();

        public QueuedSource(int maxBufferSize)
        {
            _inputQueue = new BlockingCollection<T>(maxBufferSize);
            _workerTask = new Task(Run);
        }

        protected void QueueInput(T inputItem)
        {
            _inputQueue.Add(inputItem);
        }

        public bool Any()
        {
            return _inputQueue.Count > 0;
        }

        public T Get(CancellationToken token)
        {
            return _inputQueue.Take(token);
        }

        public void Start()
        {
            _isRunning = true;
            _workerTask.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Wait()
        {
            _workerTask.Wait();
        }

        private void Run()
        {
            while (_isRunning)
            {
                LoadLoop();
            }
        }
    }
}
