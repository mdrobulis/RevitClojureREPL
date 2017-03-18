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

     
        public ConsoleRepl(IExecutor exe)
        {

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
                                return ClojureInit.ExecuteInNs.invoke(code,ClojureInit.NS);
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