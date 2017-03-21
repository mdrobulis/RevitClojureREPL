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


        private StringBuilder sb = new StringBuilder(100);

        public override void Write(char value)
        {
            if (value != '\n')
            {
                sb.Append(value);
            }
            else
            {
                
            

            if (_output.IsHandleCreated)
            {


                _output.Invoke(new Action(() =>
                {
                    _output.AppendText(sb.ToString());
                    sb = new StringBuilder();
                }

                ));
            }
          }
            // When character data is written, append it to the text box.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    

}