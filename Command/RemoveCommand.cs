using System;
using System.Linq;
using System.Windows;
using MixCreator.Event;
using MixCreator.Model;
using MessageBox = System.Windows.MessageBox;

namespace MixCreator.Command
{
    public class RemoveCommand : BaseCommand
    {
        private IRepository<Song> Repository { get; set; }

        public RemoveCommand(IRepository<Song> repository)
        {
            this.Repository = repository;
        }

        public override void Execute(object parameter)
        {
            if (null == parameter)
            {
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete the selected item?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            Repository.Remove((Song) parameter);

            EventBus.Publish(new EventRefreshData());
        }

        public override event EventHandler CanExecuteChanged;
    }
}
