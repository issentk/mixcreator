using System.Collections.Concurrent;
using MixCreator.Model;

namespace MixCreator.Provider
{
    public interface IDownloader
    {
        /// <summary>
        /// starts downloading all enqueued songs. If songs are added, while still running,
        /// they are processed as well. If no more songs are there, the method returns.
        /// </summary>
        void StartDownload();

        /// <summary>
        /// Gets the Song Queue, for enqueuing further songs to check
        /// </summary>
        /// <returns>The song queue</returns>
        ConcurrentQueue<Song> GetSongQueue();
    }
}
