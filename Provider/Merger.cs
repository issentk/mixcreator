using System;
using System.Collections.Concurrent;
using System.IO;
using System.Windows;
using MixCreator.Event;
using MixCreator.Model;
using NAudio.Wave;

namespace MixCreator.Provider
{
    class Merger : IMerger
    {
        private Song currentSong;

        public ConcurrentQueue<Song> SongQueue { get; set; }

        public IRepository<Song> Repository { get; set; }

        public IProviderFactory ProviderFactory { get; set; }

        public Merger(IRepository<Song> repository, IProviderFactory providerFactory)
        {
            SongQueue = new ConcurrentQueue<Song>();
            Repository = repository;
            ProviderFactory = providerFactory;
        }

        public void StartMerge()
        {
            using (var outputStream = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "final.mp3")))
            {
                while (SongQueue.Count > 0)
                {
                    if (!SongQueue.TryDequeue(out currentSong))
                    {
                        currentSong = null;
                        continue;
                    }

                    Process(outputStream);
                }
            }
        }

        private void Process(Stream output)
        {
            Mp3FileReader reader = new Mp3FileReader(currentSong.Path);
            if ((output.Position == 0) && (reader.Id3v2Tag != null))
            {
                output.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
            }
            Mp3Frame frame;
            while ((frame = reader.ReadNextFrame()) != null)
            {
                output.Write(frame.RawData, 0, frame.RawData.Length);
            }

            currentSong.Status = Song.SongStatus.Done;
            Repository.UpdateStatus(currentSong);
            PublishEvent();
        }

        public ConcurrentQueue<Song> GetSongQueue()
        {
            return SongQueue;
        }

        private void PublishEvent()
        {
            Application.Current.Dispatcher.Invoke(() => EventBus.Publish(new EventRefreshData()));
        }
    }
}
