using System;

namespace DotFlow
{

    public class FlowBuilderContext<SourceType>
    {
        private readonly FlowBuilder _builder;
        private readonly ISource<SourceType> _currentSource;
        private readonly ILink _lastLink;

        public FlowBuilderContext(FlowBuilder builder, ISource<SourceType> currentSource, ILink link)
        {
            _currentSource = currentSource;
            _builder = builder;
            _lastLink = link;
        }

        public FlowBuilderContext<TargetType> AddActor<TargetType>(Func<SourceType, TargetType> func, string name)
        {
            var nextSource = new BlockingQueue<TargetType>(_builder.MaxBufferSize);
            var link = new Link<SourceType, TargetType>(_currentSource, nextSource, func);
            var actor = new Actor<SourceType, TargetType>(link, name, 
                _builder.ErrorListener, _builder.NumberOfTasks);
            _builder.Add(actor);
            return new FlowBuilderContext<TargetType>(_builder, nextSource, link);
        }

        public FlowBuilder Target(ITarget<SourceType> target)
        {
            _lastLink.SetTarget(target);
            return _builder;
        }
    }
}
