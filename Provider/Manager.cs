using System.Threading.Tasks;
using MixCreator.Model;

namespace MixCreator.Provider
{
    public sealed class Manager : IManager
    {
        /* TODO: Maybe use Thread-Pool */
        private Task downloadTask;
        private Task checkerTask;
        private Task mergeTask;
        private readonly IDownloader downloader;
        private readonly IChecker checker;
        private readonly IMerger merger;
        private readonly IRepository<Song> repository;
        
        public Manager(IDownloader downloader, IChecker checker, IMerger merger, IRepository<Song> repository)
        {
            this.downloader = downloader;
            this.checker = checker;
            this.merger = merger;
            this.repository = repository;
        }

        public void Check()
        {
            var songs = repository.LoadByStatus(Song.SongStatus.Idle);

            foreach (var song in songs)
            {
                // Update status before adding
                song.Status = Song.SongStatus.WaitCheck;
                repository.Update(song);

                // Add song to Download Queue
                checker.GetSongQueue().Enqueue(song);
            }

            if (isTaskActive(checkerTask))
            {
                // already running
                return;
            }

            checkerTask = new Task(() => checker.StartCheck());
            checkerTask.Start();
        }

        public void Download()
        {
            var songs = repository.LoadByStatus(Song.SongStatus.Online);

            foreach (var song in songs)
            {
                // Update status before adding
                song.Status = Song.SongStatus.WaitDownload;
                repository.Update(song);

                // Add song to Download Queue
                downloader.GetSongQueue().Enqueue(song);
            }

            if (isTaskActive(downloadTask))
            {
                // already running
                return;
            }

            downloadTask = new Task(() => downloader.StartDownload());
            downloadTask.Start();
        }

        public void Merge()
        {
            var songs = repository.LoadByStatus(Song.SongStatus.Complete);

            foreach (var song in songs)
            {
                // Update status before adding
                song.Status = Song.SongStatus.WaitConvert;
                repository.Update(song);

                // Add song to Download Queue
                merger.GetSongQueue().Enqueue(song);
            }

            if (isTaskActive(mergeTask))
            {
                // already running
                return;
            }

            mergeTask = new Task(() => merger.StartMerge());
            mergeTask.Start();
        }

        private bool isTaskActive(Task task)
        {
            return task != null && (task.IsCompleted == false ||
                                    task.Status == TaskStatus.Running ||
                                    task.Status == TaskStatus.WaitingToRun ||
                                    task.Status == TaskStatus.WaitingForActivation);
        }
    }
}
