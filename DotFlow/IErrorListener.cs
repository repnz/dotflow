using System;

namespace DotFlow
{
    

    public interface IErrorListener
    {
        void OnError(ActorComponent component, string actorName, Exception e);
    }
}
