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

        List<string> CommandHistory = new List<string>() {""};
        private int index = 0;
     
        public ConsoleRepl(IExecutor exe)
        {
            InitializeComponent();

            var inpoutStream = new BlockingStream(new MemoryStream());
            var outputStream = new BlockingStream(new MemoryStream());

            var replThread = new Thread(() => ClojureInit.REPL2(inpoutStream, outputStream,outputStream,exe));
            replThread.Start();
            var inputWriter = new StreamWriter(inpoutStream);
            var outputReader = new StreamReader(outputStream);
            outputReader.InitializeLifetimeService();
            inputWriter.InitializeLifetimeService();

            TextBoxStreamWriter textBoxStreamWriter = new TextBoxStreamWriter( this.richTextBox1);

            new Thread(()=> {
                
               
                while (true)
                {
                    var output = outputReader.Read();
                    if((char)output != '\r' )
                     textBoxStreamWriter.Write((char) output);
                    
                }
            }).Start();


            CommandInput.KeyDown += (sender, args) =>
            {
                if (args.KeyData == Keys.Enter)
                {
                    try
                    {
                        args.SuppressKeyPress = true;
                        
                        string code = CommandInput.Text.Trim('\r', '\n', ' ');
                        CommandHistory.Add(code);
                        index = CommandHistory.Count-1;
                        inputWriter.WriteLine(code);
                        textBoxStreamWriter.WriteLine(code);
                        inputWriter.Flush();
                        
                    }
                    catch (Exception ex)
                    {
                        CommandInput.Text = String.Empty;
                    }
                }
                if (args.KeyData == Keys.Up)
                {
                    try
                    {
                        args.SuppressKeyPress = true;
                        index--;
                        CommandInput.Text = CommandHistory[index];
                    }
                    catch (Exception ex)
                    {
                        CommandInput.Text = String.Empty;
                    }
                }
                if (args.KeyData == Keys.Down)
                {
                    try
                    {
                        args.SuppressKeyPress = true;
                        index++;
                        CommandInput.Text = CommandHistory[index];
                    }
                    catch (Exception ex)
                    {
                        CommandInput.Text = String.Empty;
                    }
                }


            };

            
        }
    }
}