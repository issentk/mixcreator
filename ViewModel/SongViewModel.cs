using MixCreator.Command;
using MixCreator.Model;
using MixCreator.Provider;

namespace MixCreator.ViewModel
{
    public class SongViewModel
    {
        public Song Song { get; set; }

        public SaveCommand SaveCommand { get; set; }

        public SongViewModel(IRepository<Song> repository, IManager manager, Song song)
        {
            Song = song ?? new Song();
            SaveCommand = new SaveCommand(manager, repository);
        }
    }
}
