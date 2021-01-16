using System;
using MixCreator.Model;
using MixCreator.Provider;
using MixCreator.ViewModel;

namespace MixCreator.Command
{
    public class EditCommand : BaseCommand
    {
        private IRepository<Song> Repository { get; set; }

        public IManager Manager { get; set; }

        public EditCommand(IRepository<Song> repository, IManager manager)
        {
            this.Repository = repository;
            this.Manager = manager;
        }
        
        public override void Execute(object parameter)
        {
            if (null == parameter)
            {
                return;
            }

            new Window.SongDetail(Repository, Manager, (Song) parameter).Show();
        }

        public override event EventHandler CanExecuteChanged;

        }
}
