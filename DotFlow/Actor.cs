using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotFlow
{
    class Actor<SourceType, TargetType> : IActor
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly Link<SourceType, TargetType> _link;
        private readonly List<Task> _tasks;
        private readonly IErrorListener _errorListener;

        public string Name { get; }

        public Actor(Link<SourceType, TargetType> link, string name, 
            IErrorListener errorListener, int numberOfTasks)
        {
            Name = name;
            _tasks = new List<Task>();

            for (int i=0; i<numberOfTasks; ++i)
            {
                _tasks.Add(new Task(Work));
            }

            _link = link;
            _errorListener = errorListener;
            _tokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _link.Source.Start();
            _tasks.ForEach(t => t.Start());
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            _link.Source.Stop();
        }

        public void Wait() { Task.WaitAll(_tasks.ToArray()); }

        private void Work()
        {
            while (!_tokenSource.IsCancellationRequested || _link.Source.Any())
            {

                if (!HandleRead(out SourceType sourceItem))
                    return;

                TargetType targetItem;

                try
                {
                    targetItem = _link.Action(sourceItem);
                }
                catch (Exception e)
                {
                    _errorListener.OnError(ActorComponent.Action, Name, e);
                    continue;
                }

                HandleWrite(targetItem);
            }
        }

        private void HandleWrite(TargetType targetItem)
        {
            bool errorLogged = true;

            while (true)
            {
                try
                {
                    _link.Target.Put(targetItem);
                    break; 
                }
                catch (Exception e) when (!errorLogged)
                {
                    _errorListener.OnError(ActorComponent.Target, Name, e);
                    errorLogged = true;
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                    continue;
                }
            }
        }

        private bool HandleRead(out SourceType sourceItem)
        {
            bool errorLogged = false;

            while (true)
            {
                try
                {
                    sourceItem = _link.Source.Get(_tokenSource.Token);
                    break;
                }
                catch (OperationCanceledException)
                {
                    sourceItem = default(SourceType);
                    return false;
                }
                catch (Exception e) when (!errorLogged)
                {
                    _errorListener.OnError(ActorComponent.Source, Name, e);
                    Thread.Sleep(5000);
                    errorLogged = true;
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                }
            }

            return true;
        }
        
    }
}
