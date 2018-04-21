using System.Threading;

namespace DotFlow
{
    public interface ISource<T>
    {
        T Get(CancellationToken token);

        bool Any();

        void Start();

        void Stop();
    }
}
