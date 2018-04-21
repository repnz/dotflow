namespace DotFlow
{
    public interface IActor
    {
        string Name { get; }

        void Start();
        void Stop();
        void Wait();
    }
}
