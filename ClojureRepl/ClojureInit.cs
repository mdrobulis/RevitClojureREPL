using clojure.clr.api;
using clojure.lang;

namespace ClojureRepl
{
    public static class ClojureInit
    {
        public static IFn nsGen;
        public static IFn ExecuteInNs;

        public static IFn ReplExe;
        public static object NS;

        public static IFn DefinerFn;

        static ClojureInit()
        {
            IFn Eval = Clojure.var("clojure.core", "eval");

            object execFn = Clojure.read(@"
(defn execute  
  ""evaluates s-forms""
  ([request](execute request *ns*))
  ([request user-ns]    
      (try
        (binding [*ns* user-ns]
            (eval (read-string request)))
        (catch Exception e (str (. e Message) "" "" (. e StackTrace )))
)))");

            object nsGenFn = Clojure.read(@"
(defn generate-ns  
  ""generates ns for client connection""
  [] (let [user-ns (create-ns (symbol(str ""user"" )))]
    (execute(str ""(clojure.core/refer 'clojure.core)"") user-ns)
    user-ns)) ");


            //object definer = Clojure.read("(fn [name data] (def (symbol name) data ) ) ");
            //DefinerFn = Eval.invoke(definer) as IFn;
            

            ExecuteInNs = Eval.invoke(execFn) as IFn;
            nsGen = Eval.invoke(nsGenFn) as IFn;

            NS = nsGen.invoke();

            ExecuteInNs.invoke("(use 'clojure.reflect 'clojure.pprint 'clojure.repl)", NS);

            //ReplExe = ExecuteInNs.invoke(" (defn print-exe [r] (with-out-str (clojure.pprint/pprint (clojure.core/execute r)))") as IFn;

        }

    }
}