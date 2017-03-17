using System;
using System.Windows.Input;

namespace ClojureRepl
{
    public class GenericCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged;


        public string Name { get; set; }

        public GenericCommand(Action<object> execute, string name = "Execute")
            : this(execute, (o) => true, name)
        {
        }

        public GenericCommand(Action<object> execute, Predicate<object> canExecute, string name = "Execute")
        {
            _execute = execute;
            _canExecute = canExecute;
            this.Name = name;
        }

        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            try
            {
                _execute(parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw ex;
            }
        }

    }
}