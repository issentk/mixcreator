using System.Collections.Concurrent;
using MixCreator.Model;

namespace MixCreator.Provider
{
    public interface IMerger
    {
        /// <summary>
        /// starts merging all enqueued songs. If songs are added, while still running,
        /// they are processed as well. If no more songs are there, the method returns.
        /// </summary>
        void StartMerge();

        /// <summary>
        /// Gets the Song Queue, for enqueuing further songs to merge
        /// </summary>
        /// <returns>The song queue</returns>
        ConcurrentQueue<Song> GetSongQueue();
    }
}
