using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autodesk.Revit.UI;

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
        public ExecuteClojureCode(ObservableCollection<string> hist)
        {
            Hist = hist;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var e = new mySafeCodeExecutor(parameter.ToString(),Hist);
            var x =ExternalEvent.Create(e);
            x.Raise();
        }

        public event EventHandler CanExecuteChanged;
}
}
