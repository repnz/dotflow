using System;

namespace DotFlow
{
    class Link<SourceType, TargetType> : ILink
    {
        public ISource<SourceType> Source { get; private set; }
        public ITarget<TargetType> Target { get; private set; }
        public Func<SourceType, TargetType> Action { get; private set; }

        public Link(ISource<SourceType> source, ITarget<TargetType> target, Func<SourceType, TargetType> action)
        {
            Source = source;
            Target = target;
            Action = action;
        }

        ISource<T> ILink.GetSource<T>()
        {
            return (ISource<T>)Source;
        }

        ITarget<T> ILink.GetTarget<T>()
        {
            return (ITarget<T>)Target;
        }

        void ILink.SetTarget<T>(ITarget<T> target)
        {
            Target = (ITarget<TargetType>)target;
        }

        void ILink.SetSource<T>(ISource<T> source)
        {
            Source = (ISource<SourceType>)source;
        }
    }
}
