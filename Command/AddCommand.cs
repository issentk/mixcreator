using System;
using MixCreator.Model;
using MixCreator.Provider;
using MixCreator.ViewModel;

namespace MixCreator.Command
{
    public class AddCommand : BaseCommand
    {
        private IRepository<Song> Repository { get; set; }

        public IManager Manager { get; set; }

        public AddCommand(IRepository<Song> repository, IManager manager)
        {
            this.Repository = repository;
            this.Manager = manager;
        }

        public override void Execute(object parameter)
        {
            new Window.SongDetail(Repository, Manager).Show();
        }

        public override event EventHandler CanExecuteChanged;
    }
}