using System;
using System.Collections.Concurrent;
using System.IO;
using System.Windows;
using MixCreator.Event;
using MixCreator.Model;
using YoutubeExtractor;

namespace MixCreator.Provider
{
    public class Downloader : IDownloader
    {
        private Song currentSong;

        public ConcurrentQueue<Song> SongQueue { get; set; }

        public IRepository<Song> Repository { get; set; }

        public IProviderFactory ProviderFactory { get; set; }

        public Downloader(IRepository<Song> repository, IProviderFactory providerFactory)
        {
            SongQueue = new ConcurrentQueue<Song>();
            Repository = repository;
            ProviderFactory = providerFactory;
        }

        public void StartDownload()
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

            //EventBus.Publish(new EventDownloadComplete());
            Application.Current.Dispatcher.Invoke(() => EventBus.Publish(new EventDownloadComplete()));
        }

        public ConcurrentQueue<Song> GetSongQueue()
        {
            return SongQueue;
        }

        private void Process()
        {
            IProvider provider = ProviderFactory.GetProvider(currentSong);

            currentSong.Status = Song.SongStatus.Downloading;
            Repository.UpdateStatus(currentSong);

            //PublishEvent(new EventRefreshData());
            Application.Current.Dispatcher.Invoke(() => EventBus.Publish(new EventRefreshData()));

            provider.Progress += ProviderOnProgress;

            provider.Download(currentSong.Path);
        }

        private void PublishEvent(IEvent e)
        {
            Application.Current.Dispatcher.Invoke(() => EventBus.Publish(e));
        }

        private void ProviderOnProgress(object sender, ProgressEventArgs eventArgs)
        {
            currentSong.Status = Song.SongStatus.Downloading;
            currentSong.Progress = $"{eventArgs.ProgressPercentage:0}%";
            if (eventArgs.ProgressPercentage > 100.0)
            {
                currentSong.Status = Song.SongStatus.Complete;
            }

            Repository.UpdateStatusProgress(currentSong);

            //PublishEvent(new EventRefreshData());
            Application.Current.Dispatcher.Invoke(() => EventBus.Publish(new EventRefreshData()));
        }
    }
}
