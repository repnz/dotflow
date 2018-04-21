using System.Collections.Generic;

namespace DotFlow
{
    public class FlowBuilder
    {
        
        private readonly List<IActor> _actors;
        internal IErrorListener ErrorListener { get; }
        internal int NumberOfTasks { get; }
        internal int MaxBufferSize { get; }

        public FlowBuilder(IErrorListener errorListener, int numberOfTasksPerActor, int maxBufferSize)
        {
            NumberOfTasks = numberOfTasksPerActor;
            MaxBufferSize = maxBufferSize;
            ErrorListener = errorListener;
            _actors = new List<IActor>();
        }

        public FlowBuilder(IErrorListener errorListener) : this(errorListener, 10, 100)
        {
        }

        public FlowBuilderContext<T> Source<T>(ISource<T> source)
        {
            return new FlowBuilderContext<T>(this, source, null);
        }

        public Flow Create(string flowName)
        {
            return new Flow(_actors, flowName);
        }

        internal void Add(IActor actor)
        {
            this._actors.Add(actor);
        }
    }
}
