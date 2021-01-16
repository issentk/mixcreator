using System.Collections.Concurrent;
using System.IO;
using System.Windows;
using MixCreator.Event;
using MixCreator.Model;

namespace MixCreator.Provider
{
    public class Checker : IChecker
    {
        private Song currentSong;

        public ConcurrentQueue<Song> SongQueue { get; set; }

        public IRepository<Song> Repository { get; set; }

        public IProviderFactory ProviderFactory { get; set; }

        public Checker(IRepository<Song> repository, IProviderFactory providerFactory)
        {
            SongQueue = new ConcurrentQueue<Song>();
            Repository = repository;
            ProviderFactory = providerFactory;
        }

        public void StartCheck()
        {
            while (SongQueue.Count > 0)
            {
                if (!SongQueue.TryDequeue(out currentSong))
                {
                    currentSong = null;
                    continue;
                }

                Process();
            }
        }

        public ConcurrentQueue<Song> GetSongQueue()
        {
            return SongQueue;
        }

        private void Process()
        {
            IProvider provider = ProviderFactory.GetProvider(currentSong);

            currentSong.Status = Song.SongStatus.Checking;
            Repository.UpdateStatus(currentSong);

            if (!provider.CheckOnline())
            {
                currentSong.Status = Song.SongStatus.Offline;
                Repository.UpdateStatus(currentSong);

                PublishEvent();
                return;
            }

            currentSong.Status = Song.SongStatus.Online;
            currentSong.Name = provider.GetName();
            currentSong.Size = $"{provider.GetSize() / 1000} KB";
            currentSong.Path = Path.Combine(Directory.GetCurrentDirectory(), currentSong.Name + ".mp3");
            Repository.UpdateStatusNamePathSize(currentSong);
            PublishEvent();
        }

        private void PublishEvent()
        {
            Application.Current.Dispatcher.Invoke(() => EventBus.Publish(new EventRefreshData()));
        }
    }
}
