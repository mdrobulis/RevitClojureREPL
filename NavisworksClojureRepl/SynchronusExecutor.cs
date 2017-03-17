using System;
using System.Threading.Tasks;
using ClojureRepl;

namespace NavisworksClojureRepl
{
    public class SynchronusExecutor : IExecutor
    {
        public Task Run(Action action)
        {
            var t = new Task(action);
            t.RunSynchronously();
            return t;
        }

        public Task<T> Run<T>(Func<T> action)
        {
            var t = new Task<T>(action);
            t.RunSynchronously();
            return t;
        }
    }
}