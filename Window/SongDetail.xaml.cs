using MixCreator.Event;
using MixCreator.Model;
using MixCreator.Provider;
using MixCreator.ViewModel;

namespace MixCreator.Window
{
    /// <summary>
    /// Interaktionslogik für SongDetail.xaml
    /// </summary>
    public partial class SongDetail : System.Windows.Window
    {
        public SongDetail(IRepository<Song> repository, IManager manager, Song song = null)
        {
            InitializeComponent();
            DataContext = new SongViewModel(repository, manager, song);
            EventBus.Subscribe<EventCloseSongDetail>(CloseSongDetailHandler);
        }

        private void CloseSongDetailHandler(EventCloseSongDetail e)
        {
            Close();
        }
    }
}
