using System;
using System.IO;
using System.Text;


namespace RevitClojureRepl
{
   
    public class TextBoxStreamWriter : TextWriter
    {
        System.Windows.Forms.RichTextBox _output = null;

        public TextBoxStreamWriter(System.Windows.Forms.RichTextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            if (_output.IsHandleCreated)
                _output.Invoke(new Action(() => {
                    
                                     _output.AppendText(value.ToString());
                }
                )); 
            // When character data is written, append it to the text box.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    

}