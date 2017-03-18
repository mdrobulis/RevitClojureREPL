using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using clojure.clr.api;
using clojure.lang;
using ClojureRepl;

namespace RevitClojureRepl
{
    public partial class ConsoleRepl : UserControl
    {

        public static readonly IFn Eval = Clojure.var("clojure.core", "eval");

        public static readonly object execFn = Clojure.read(@"
(defn execute  
  ""evaluates s-forms""
  ([request](execute request *ns*))
  ([request user-ns]
    (str
      (try
        (binding [*ns* user-ns]
            (eval (read-string request)))
        (catch Exception e (. e Message))))))");

        public static readonly object nsGenFn = Clojure.read(@"
(defn generate-ns  
  ""generates ns for client connection""
  [] (let [user-ns (create-ns (symbol(str ""user"" )))]
    (execute(str ""(clojure.core/refer 'clojure.core)"") user-ns)
    user-ns)) ");

        public static IFn nsGen;
        public static IFn ExecuteInNs;
        public static object NS;

        public ConsoleRepl(IExecutor exe)
        {
            InitializeComponent();

            IFn Eval = Clojure.var("clojure.core", "eval");

            ExecuteInNs = Eval.invoke(execFn) as IFn;
            nsGen = Eval.invoke(nsGenFn) as IFn;
            NS = nsGen.invoke();



            

            ExecuteInNs.invoke("(def App RevitClojureRepl.GuiReplPlugin/app)", NS);
            ExecuteInNs.invoke("(def CommandData RevitClojureRepl.GuiCommand/CommandData)", NS);

            CommandInput.KeyDown += async (sender, args) =>
            {
                if (args.KeyData == Keys.Enter)
                {
                    try
                    {
                        args.SuppressKeyPress = true;
                        string code = CommandInput.Text.Trim();
                        listBox1.Items.Add(code);
                        var res = await exe.Run(() =>
                        {
                            try
                            {
                                return ExecuteInNs.invoke(code);
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }).ConfigureAwait(false);

                        listBox1.Items.Add(res.ToString());
                        CommandInput.Text = String.Empty;
                    }
                    catch (Exception ex)
                    {
                        listBox1.Items.Add(ex.Message);
                        listBox1.Items.Add(ex.StackTrace);
                        CommandInput.Text = String.Empty;
                    }
                }
            };

            listBox1.SelectedValueChanged +=
                (sender, args) =>
                {
                    if(listBox1.SelectedItem!=null)
                        CommandInput.Text = listBox1.SelectedItem.ToString().Trim();
                };
        }
    }
}