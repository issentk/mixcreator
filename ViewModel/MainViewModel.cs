using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MixCreator.Command;
using MixCreator.Event;
using MixCreator.Model;
using MixCreator.Provider;

namespace MixCreator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Song> Songs { get; set; }

        private int _selectedSong;
        public int SelectedSong
        {
            get { return _selectedSong; }
            set
            {
                _selectedSong = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedSong"));
            }
        }

        public AddCommand AddCommand { get; set; }

        public RemoveCommand RemoveCommand { get; set; }

        public EditCommand EditCommand { get; set; }

        public MoveCommand DownCommand { get; set; }

        public MoveCommand UpCommand { get; set; }

        public StartCommand StartCommand { get; set; }

        public IRepository<Song> Repository { get; set; }

        public IManager Manager { get; set; }

        public MainViewModel(IRepository<Song> repository, IManager manager)
        {
            // Data
            Songs = new ObservableCollection<Song>();
            Repository = repository;
            Manager = manager;
            RefreshData();

            // Commands
            AddCommand = new AddCommand(Repository, Manager);
            RemoveCommand = new RemoveCommand(Repository);
            EditCommand = new EditCommand(Repository, Manager);
            DownCommand = new MoveCommand(Repository, true);
            UpCommand = new MoveCommand(Repository, false);
            StartCommand = new StartCommand(Manager);

            // Events
            EventBus.Subscribe<EventRefreshData>(RefreshDataHandler);
            EventBus.Subscribe<EventSelectionChanged>(SelectionChangedHandler);
            EventBus.Subscribe<EventDownloadComplete>(DownloadCompleteHandler);
        }

        private void RefreshData()
        {
            int selection = SelectedSong;

            Songs.Clear();

            foreach (var item in Repository.LoadAll())
            {
                Songs.Add(item);
            }

            SelectedSong = selection;
        }

        #region Events and INotifyChanged Implementation

        private void SelectionChangedHandler(EventSelectionChanged e)
        {
            SelectedSong += e.DirectionDown ? 1 : -1;
        }

        private void RefreshDataHandler(EventRefreshData e)
        {
            RefreshData();
        }


        private void DownloadCompleteHandler(EventDownloadComplete e)
        {
            Manager.Merge();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
