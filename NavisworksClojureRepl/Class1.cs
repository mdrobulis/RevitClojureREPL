using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;
using System.Windows.Forms;
using ClojureRepl;
using RevitClojureRepl;

namespace NavisworksClojureRepl
{
    [Plugin("Clojure REPL", //Plugin name
       "Martynas Drobulis", //Developer ID or GUID         
       DisplayName = "Clojure REPL panel",
       ToolTip = "Clojure REPL panel")]
    [DockPanePlugin(200, 300, FixedSize = false, MinimumHeight = 300, MinimumWidth = 200)]
    public class NavisWorksClojurePlugin : DockPanePlugin
    {
        public override Control CreateControlPane()
        {
            var host = new ConsoleRepl(new SynchronusExecutor(Autodesk.Navisworks.Api.Application.ActiveDocument));


            //ClojureInit.ExecuteInNs.invoke(@" (load-string (slurp ""C:\\dev\\RevitClojureRepl\\NavisworksClojureRepl\\wrapper.clj"")) ");

            host.Dock = DockStyle.Fill;
            host.CreateControl();
            return host;

        }

        public override void DestroyControlPane(Control pane)
        {
            pane.Dispose();
        }
    }


    //[Plugin("Clojure REPL", //Plugin name
    //  "Martynas Drobulis", //Developer ID or GUID         
    //  DisplayName = "Clojure REPL window",
    //  ToolTip = "Clojure REPL window")]


    //[DockPanePlugin(200, 300, FixedSize = false, MinimumHeight = 300, MinimumWidth = 200)]
    //public class so : DockPanePlugin
    //{
    //    public override Control CreateControlPane()
    //    {
    //        var wpfView = new ReplConsole();
    //        var viewModel = new ConsoleViewModel(new SynchronusExecutor());

    //        wpfView.DataContext = viewModel;

    //        Window w = new Window();
    //        w.Content = wpfView;

    //       // w.Show();

    //        ConsoleRepl r = new ConsoleRepl(new SynchronusExecutor());
    //        r.Show();

    //        return new Panel();
    //    }
    //}


}
