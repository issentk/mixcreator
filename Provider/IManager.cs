namespace MixCreator.Provider
{
    public interface IManager
    {
        /// <summary>
        /// Starts checking online status of download links.
        /// </summary>
        void Check();

        /// <summary>
        /// Starts downloading the download links.
        /// </summary>
        void Download();

        /// <summary>
        /// Stats merging the downloaded mp3s to one mix.
        /// </summary>
        void Merge();
    }
}
