using System;
using System.Linq;
using MixCreator.Event;
using MixCreator.Model;
using MixCreator.ViewModel;

namespace MixCreator.Command
{
    public class MoveCommand : BaseCommand
    {
        public bool DirectionDown { get; set; }

        private IRepository<Song> Repository { get; set; }

        public MoveCommand(IRepository<Song> repository, bool directionDown)
        {
            this.DirectionDown = directionDown;
            this.Repository = repository;
        }

        public override void Execute(object parameter)
        {
            if (null == parameter)
            {
                return;
            }

            Repository.ChangeOrder((Song)parameter, DirectionDown);

            EventBus.Publish(new EventRefreshData());
            EventBus.Publish(new EventSelectionChanged(DirectionDown));
        }

        public override event EventHandler CanExecuteChanged;
    }
}
