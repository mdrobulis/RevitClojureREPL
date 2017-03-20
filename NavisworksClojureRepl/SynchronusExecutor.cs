using System;
using System.Threading.Tasks;
using clojure.clr.api;
using clojure.lang;
using ClojureRepl;

namespace NavisworksClojureRepl
{
    public class SynchronusExecutor : IExecutor
    {
        private object document;

        public SynchronusExecutor(object doc)
        {
            document = doc;
        }

        public Task Run(Action action)
        {
            var t = new Task(action);
            t.RunSynchronously();
            return t;
        }

        public object Run(IFn func)
        {
            var p =ClojureInit.Eval.invoke(Clojure.read("(promise)"));
            return ClojureInit.Sync.invoke(func, document,p);
            
        }

        public void Run(string code)
        {
            throw new NotImplementedException();
        }

        public Task<T> Run<T>(Func<T> action)
        {
            var t = new Task<T>(action);
            t.RunSynchronously();
            return t;
        }
    }
}