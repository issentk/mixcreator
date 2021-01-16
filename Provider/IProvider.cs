using System;
using YoutubeExtractor;

namespace MixCreator.Provider
{
    public interface IProvider
    {
        /// <summary>
        /// Downloads the file
        /// </summary>
        /// <param name="filename"></param>
        void Download(string filename);

        /// <summary>
        /// check online status
        /// </summary>
        /// <returns>true if online</returns>
        bool CheckOnline();

        /// <summary>
        /// get name of link
        /// </summary>
        /// <returns>string name</returns>
        string GetName();

        /// <summary>
        /// size of video/song in bytes which needs to be downloaded
        /// </summary>
        /// <returns>bytes</returns>
        int GetSize();

        /// <summary>
        /// progress event
        /// </summary>
        event EventHandler<ProgressEventArgs> Progress;
    }
}
