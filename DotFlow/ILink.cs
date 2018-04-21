namespace DotFlow
{
    public interface ILink
    {
        ISource<T> GetSource<T>();
        ITarget<T> GetTarget<T>();

        void SetTarget<T>(ITarget<T> target);
        void SetSource<T>(ISource<T> source);
    }
}
