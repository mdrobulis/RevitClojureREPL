using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using clojure.clr.api;
using clojure.lang;
using ClojureRepl;


namespace RevitClojureRepl
{
    public class GuiReplPlugin : IExternalApplication
    {
       
        public static UIControlledApplication app;

        public Result OnStartup(UIControlledApplication application)
        {
            app = application;

            application.CreateRibbonTab("Clojure");
            var panel = application.CreateRibbonPanel("Clojure", "Clojure");
            var assembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var button = new PushButtonData("console", "Clojure REPL", assembly, "RevitClojureRepl.GuiCommand");
            panel.AddItem(button);
            
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }

    public class JtWindowHandle : IWin32Window
    {
        IntPtr _hwnd;

        public JtWindowHandle(IntPtr h)
        {
            Debug.Assert(IntPtr.Zero != h, "expected non-null window handle");
            _hwnd = h;
        }

        public IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }
    }


    [Transaction(TransactionMode.Manual)]
    public class GuiCommand : IExternalCommand,IExternalEventHandler
    {

        public static ExternalCommandData CommandData;


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            CommandData = commandData;


            var exe = new RevitIdlingAsyncEventExecutor(commandData.Application);

            ClojureInit.ExecuteInNs.invoke("(load-string (slurp \"revitWrapper.clj\"))");

            var form = new System.Windows.Forms.Form();
            ConsoleRepl r = new ConsoleRepl(exe);
            r.Dock=DockStyle.Fill;
            form.Controls.Add(r);

            form.Show(new JtWindowHandle( Process.GetCurrentProcess().MainWindowHandle ));

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