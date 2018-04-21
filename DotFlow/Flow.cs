using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DotFlow
{
    public class Flow : IActor
    {
        private readonly List<IActor> _actors;
        private readonly Task _completionTask;

        public string Name { get; }

        internal Flow(IEnumerable<IActor> actors, string flowName)
        {
            _actors = actors.ToList();
            _completionTask = new Task(CompletionWorker);
        }

        public void Start()
        {
            _actors.ForEach(a => a.Start());
        }

        public void Stop()
        {
            _completionTask.Start();
        }

        public void Wait()
        {
            _completionTask.Wait();
        }

        private void CompletionWorker()
        {
            foreach (IActor actor in _actors)
            {
                actor.Stop();
                actor.Wait();
            }
        }
    }
}