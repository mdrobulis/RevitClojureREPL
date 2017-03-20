using System.IO;
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

        public static IFn Eval;

        static ClojureInit()
        {
            Eval = Clojure.var("clojure.core", "eval");

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

            var SyncCode =Clojure.read("(fn [handler document prom] (deliver prom (try (handler document)  (catch Exception e e)) )  ) ");

            Eval.invoke(Clojure.read("(def ^:dynamic sync)"));
            Sync = Eval.invoke(SyncCode) as IFn;
            var replCode = Clojure.read("(fn [in out err synchronizer] (binding [*in* in, *out* out *err* err sync synchronizer]  (clojure.main/with-bindings  (loop [] (try (clojure.main/repl ) (catch Exception e (clojure.main/repl-caught e) (set! *e e))) (flush) (recur)) ) ))");
            
            repl = Eval.invoke(replCode) as IFn;
            
            ExecuteInNs = Eval.invoke(execFn) as IFn;
            nsGen = Eval.invoke(nsGenFn) as IFn;

            NS = nsGen.invoke();

            ExecuteInNs.invoke("(use \'clojure.reflect \'clojure.pprint \'clojure.repl)", NS);

            

            //ReplExe = ExecuteInNs.invoke(" (defn print-exe [r] (with-out-str (clojure.pprint/pprint (clojure.core/execute r)))") as IFn;

        }

        public static IFn repl;
        

        public static IFn Sync;



        public static void REPL2(Stream input, Stream output, Stream err,IExecutor ex)
        {
            
            var codeReader = new LineNumberingTextReader(new StreamReader (new PushbackInputStream(input)));
            var outputWriter = TextWriter.Synchronized(new StreamWriter(output));
            var errorWriter = TextWriter.Synchronized(new StreamWriter(err));
          
            repl.invoke(codeReader, outputWriter, errorWriter,ex);
          
        }


    }
}