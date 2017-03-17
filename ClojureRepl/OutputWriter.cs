using System.IO;
using System.Text;


namespace RevitClojureRepl
{
   
    public class TextBoxStreamWriter : TextWriter
    {
        System.Windows.Forms.ListBox _output = null;

        public TextBoxStreamWriter(System.Windows.Forms.ListBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.Items.Add(value.ToString()); // When character data is written, append it to the text box.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    

}