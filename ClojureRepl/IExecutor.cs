using System;
using System.Threading.Tasks;
using clojure.lang;

namespace ClojureRepl
{
    public interface IExecutor
    {
        /// <summary>
        /// Execute things under Revit API single thread syncronization context from any thread asynchroniously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<T> Run<T>(Func<T> action);

        Task Run(Action action);

        object Run(IFn func);

    }
}