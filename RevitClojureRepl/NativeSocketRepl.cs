using System;
using System.Diagnostics;
using Autodesk.Revit.UI;
using clojure.clr.api;
using clojure.lang;

namespace RevitClojureRepl
{
    public class NativeSocketRepl:IExternalApplication
    {
   
        private static IFn eval = Clojure.var("clojure.core", "eval");

        private object execFn = Clojure.read(@"
(defn execute  
  ""evaluates s-forms""
  ([request](execute request *ns*))
  ([request user-ns]
    (str
      (try
        (binding [*ns* user-ns]
            (eval (read-string request)))
        (catch Exception e (. e Message))))))");

        private object nsGenFn = Clojure.read(@"
(defn generate-ns  
  ""generates ns for client connection""
  [] (let [user-ns (create-ns (symbol(str ""user"" )))]
    (execute(str ""(clojure.core/refer 'clojure.core)"") user-ns)
    user-ns)) ");

        private IFn nsGen;
        private IFn execute;
        private object ns;

        private object server;

        public Result OnStartup(UIControlledApplication application)
        {

            execute = eval.invoke(execFn) as IFn;
            nsGen = eval.invoke(nsGenFn) as IFn;
            ns = nsGen.invoke();

            application.ControlledApplication.DocumentOpened += (sender, args) =>
            {
                execute.invoke(
                  Clojure.read(" (def config {:port 9998 :name (name (gensym)) :accept 'clojure.core.server/repl }) "),
                  ns);
                execute.invoke(Clojure.read(" (def server (clojure.core.server/start-server config)) "), ns);


            };

            execute.invoke(
                    Clojure.read(" (def config {:port 9998 :name (name (gensym)) :accept 'clojure.core.server/repl }) "),
                    ns);
            server = execute.invoke(Clojure.read(" (def server (clojure.core.server/start-server config)) "), ns);



            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }

    public class ConsloleRepl:IExternalApplication
    {
        
        public ConsloleRepl()
        {
        }



   


        public Result OnStartup(UIControlledApplication application)
        {



            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }


}