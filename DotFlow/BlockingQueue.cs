using System.Collections.Concurrent;
using System.Threading;

namespace DotFlow
{
    public class BlockingQueue<T> : IQueue<T>
    {
        private readonly BlockingCollection<T> _collection;

        public BlockingQueue(int max)
        {
            _collection = new BlockingCollection<T>(max);
        }

        public bool Any()
        {
            return _collection.Count > 0;
        }

        public T Get(CancellationToken token)
        {
            return _collection.Take(token);
        }

        public void Put(T item)
        {
            _collection.Add(item);
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
