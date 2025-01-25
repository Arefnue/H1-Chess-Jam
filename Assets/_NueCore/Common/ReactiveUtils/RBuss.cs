using System;
using UniRx;

namespace _NueCore.Common.ReactiveUtils
{
    /// <summary>
    /// Proxy for UniRx event system.
    /// </summary>
    public static class RBuss
    {

        /// <summary>
        /// Publishes a new event of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="evnt"></param>
        public static void Publish<T>(T evnt) where T : REvent
        {
            //evnt.ToString().NLog(new Color(1f, 0.53f, 0.04f));
            //typeof(T).ToString().NLog(new Color(1f, 0.53f, 0.04f));
            MessageBroker.Default.Publish(evnt);
        }

        /// <summary>
        /// Creates and subscribable to an event of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObservable<T> OnEvent<T>() {
            return MessageBroker.Default.Receive<T>();
        }


        /// <summary>
        /// Clears event subscriptions.
        /// Note that this method called when a level is removed.
        /// So it is not possible to have inter-level events at the moment.
        /// </summary>
        public static void ClearSubs()
        {
            MessageBroker.Default.ClearSubs();
        }

    }
    
    /// <summary>
    /// Base class for custom game events.
    /// Extend this class to implement your own events.
    /// </summary>
    public abstract class REvent
    {
    }

}