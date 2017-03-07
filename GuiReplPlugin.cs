using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Runtime.CompilerServices;
using Autodesk.Revit.Attributes;
using clojure.clr.api;


namespace RevitClojureRepl
{
    public class GuiReplPlugin : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("Clojure");
            var panel =application.CreateRibbonPanel("Clojure", "Clojure");

            var assembly = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var button = new PushButtonData("console", "Repl Console", assembly, "RevitClojureRepl.GuiCommand");

            panel.AddItem(button);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class mySafeCodeExecutor : IExternalEventHandler
    {
        private string code;

        public mySafeCodeExecutor(string code, ObservableCollection<string> history)
        {
            this.code = code;
            History = history;
        }

        private ObservableCollection<string> History;



        public void Execute(UIApplication app)
        {
            History.Add(code);
            var eval = Clojure.var("clojure.core", "eval");
            var print = Clojure.var("clojure.pprint", "pprint");
            var form = Clojure.read(code);
            var result =print.invoke(eval.invoke(form)).ToString();
            History.Add(result);

           
        }

        public string GetName()
        {
            return "ClojureExecutor";
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class GuiCommand : IExternalCommand,IExternalEventHandler {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            
            ConsoleViewModel vm = new ConsoleViewModel();

            ReplConsole view = new ReplConsole();
            view.DataContext = vm;

            view.Show();

            return Result.Succeeded;
        }

        public void Execute(UIApplication app)
        {
            
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }
    }

    

}