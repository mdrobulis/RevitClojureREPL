using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using clojure.clr.api;
using clojure.lang;

namespace ClojureRepl
{
    public class ConsoleViewModel
    {

      

        public ConsoleViewModel(IExecutor exe)
        {
            CommandHistory = new ObservableCollection<string>(new string[100]);
            FullHistory = new ObservableCollection<string>(new string[1000]);


           
            

            ExecuteSexpression = new GenericCommand(async p  =>  
            {
                string result = await exe.Run(() =>
                {
                   var res= ClojureInit.ExecuteInNs.invoke(p.ToString(), ClojureInit.NS);
                   return res.ToString();
                   return String.Empty;
                });

                CommandHistory.Add(p.ToString());

                FullHistory.Add(p.ToString());
                FullHistory.Add(result);

            });
        }

        public int CommandIndex { get; set; }


        public ICommand ExecuteSexpression { get; set; }

        public ObservableCollection<string> CommandHistory { get; set; }

        public ObservableCollection<string> FullHistory { get; set; }



        public string ClojureCode { get; set; }

    }
}
