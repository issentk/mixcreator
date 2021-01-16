using System;
using MixCreator.Event;
using MixCreator.Model;
using MixCreator.Provider;

namespace MixCreator.Command
{
    public class SaveCommand : BaseCommand
    {
        private IRepository<Song> Repository { get; set; }

        public IManager Manager { get; set; }

        public SaveCommand(IManager manager, IRepository<Song> repository)
        {
            this.Repository = repository;
            this.Manager = manager;
        }

        public override void Execute(object parameter)
        {
            Repository.AddOrUpdate((Song)parameter);
            Manager.Check();

            EventBus.Publish(new EventCloseSongDetail());
            EventBus.Publish(new EventRefreshData());
        }

        public override event EventHandler CanExecuteChanged;
    }
}
