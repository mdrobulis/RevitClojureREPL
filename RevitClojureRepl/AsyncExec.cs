using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ClojureRepl;

namespace RevitClojureRepl
{
 

    public class RevitIdlingAsyncEventExecutor : IDisposable, IExecutor
    {
        private ConcurrentQueue<Task> queue = new ConcurrentQueue<Task>();
        private ExternalEvent externalEvent;
        private UIApplication app;

        public RevitIdlingAsyncEventExecutor(UIApplication app)
        {
            app.Idling += App_Idling;
            this.app = app;
        }


        private void App_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            if (fastExecutionEnabled) e.SetRaiseWithoutDelay();
            if (!queue.IsEmpty)
                using (Transaction t = new Transaction(this.app.ActiveUIDocument.Document))
                {
                    if (t.Start("Async Idling Transaction") == TransactionStatus.Started)
                    {
                        Task task;
                        while (queue.TryDequeue(out task))
                        {
                            task.RunSynchronously();
                        }
                        if (TransactionStatus.Committed != t.Commit())
                        {
                            t.RollBack();
                            throw new Exception("Something bad happened. Transaction failed. Rollbacked");
                        }
                    }
                }
        }

        private bool fastExecutionEnabled = false;

        /// <summary>
        /// Execute things under Revit API single thread syncronization context from any thread asynchroniously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task<T> Run<T>(Func<T> action)
        {
            var task = new Task<T>(action);
            queue.Enqueue(task);
            return task;
        }

        public void RunFast()
        {
            fastExecutionEnabled = true;
        }

        public Task Run(Action action)
        {
            var task = new Task(action);
            queue.Enqueue(task);

            return task;
        }



        public string GetName()
        {
            return "Revit Async Executor";
        }

        public void Dispose()
        {
            app.Idling -= App_Idling;
        }


        public void RunSlow()
        {
            fastExecutionEnabled = false;
        }
    }
}