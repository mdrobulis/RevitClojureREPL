using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autodesk.Revit.UI;
using clojure.clr.api;
using clojure.lang;

namespace RevitClojureRepl
{
    public class ConsoleViewModel
    {

        public ConsoleViewModel()
        {
            CommandHistory = new ObservableCollection<string>(new string[0]);
            ExecuteSexpression = new ExecuteClojureCode(CommandHistory);
        }
        public ICommand ExecuteSexpression { get; set; }

        public ObservableCollection<string> CommandHistory { get; set; }

        public string ClojureCode { get; set; }

    }


    public class ExecuteClojureCode:ICommand
    {
        private ObservableCollection<string> Hist;

        private IFn eval;

        private IFn nsGen;
        private IFn execute;

        private object ns;

        public ExecuteClojureCode(ObservableCollection<string> hist)
        {

            eval = Clojure.var("clojure.core", "eval");


            var execFn = Clojure.read(@"
(defn execute  
  ""evaluates s-forms""
  ([request](execute request *ns*))
  ([request user-ns]
    (str
      (try
        (binding [*ns* user-ns] (eval (read-string request)))
        (catch Exception e (.getLocalizedMessage e))))))");

            var NsGen = Clojure.read(@"
(defn generate-ns  
  ""generates ns for client connection""
  [](let [user-ns (create-ns (symbol(str ""user"" )))]
    (execute(str ""(clojure.core/refer 'clojure.core)"") user-ns)
    user-ns))  
");

            execute= eval.invoke(execFn) as IFn;
            nsGen =eval.invoke(NsGen) as IFn;

            ns = nsGen.invoke();

            Hist = hist;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {

                
                

                
                //var form = Clojure.read(parameter.ToString());

                var res = execute.invoke(parameter.ToString(), ns);  //eval.invoke(form);

                //var result = print.invoke().ToString();
                Hist.Add(parameter.ToString());
                Hist.Add(res.ToString());
            }
            catch (Exception ex)
            {
                Hist.Add(ex.Message);
                Hist.Add(ex.StackTrace);
            }

            // var e = new mySafeCodeExecutor(parameter.ToString(),Hist);
            //  var x =ExternalEvent.Create(e);
            // x.Raise();
        }

        public event EventHandler CanExecuteChanged;
}
}
