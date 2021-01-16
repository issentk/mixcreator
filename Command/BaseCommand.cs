using System;
using System.Windows.Input;
using MixCreator.ViewModel;

namespace MixCreator.Command
{
    public abstract class BaseCommand : ICommand
    {
        public bool Executable { get; set; }
        
        protected BaseCommand()
        {
            Executable = true;
        }

        public bool CanExecute(object parameter)
        {
            return Executable;
        }

        public abstract void Execute(object parameter);
        public abstract event EventHandler CanExecuteChanged;
    }
}