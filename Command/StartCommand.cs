using System;
using MixCreator.Provider;

namespace MixCreator.Command
{
    public class StartCommand : BaseCommand
    {
        public IManager Manager { get; set; }

        public StartCommand(IManager manager)
        {
            Manager = manager;
        }

        public override void Execute(object parameter)
        {
            Manager.Download();
        }

        public override event EventHandler CanExecuteChanged;
    }
}
