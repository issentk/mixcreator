using System;
using System.IO;
using System.Linq;
using MixCreator.Model;
using YoutubeExtractor;

namespace MixCreator.Provider
{
    public class Youtube : IProvider
    {
        private string _name;

        public event EventHandler<ProgressEventArgs> Progress;

        private Song Song { get; set; }

        public Youtube(Song song)
        {
            Song = song;
        }

        public void Download(string filename)
        {
            var video = GetVideoInfo();

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the audio downloader.
             * The first argument is the video where the audio should be extracted from.
             * The second argument is the path to save the audio file.
             */
            var audioDownloader = new AudioDownloader(video, filename);

            // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
            // because the download will take much longer than the audio extraction.
            audioDownloader.DownloadProgressChanged += (sender, args) => RaiseProgressEvent(args.ProgressPercentage * 0.85);
            audioDownloader.AudioExtractionProgressChanged += (sender, args) => RaiseProgressEvent(85 + args.ProgressPercentage * 0.15);
            audioDownloader.DownloadFinished += (sender, args) => RaiseProgressEvent(101.0);

            /*
             * Execute the audio downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            audioDownloader.Execute();
        }

        private void RaiseProgressEvent(double percent)
        {
            if (null != Progress)
            {
                Progress(this, new ProgressEventArgs(percent));
            }
        }

        private VideoInfo GetVideoInfo()
        {
            // Our test youtube link
            string link = Song.Url;

            /*
             * Get the available video formats.
             * We'll work with them in the video and audio download examples.
             */
            try
            {
                var videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
                /*
                 * We want the first extractable video with the highest audio quality.
                 */
                var video = videoInfos
                    .Where(info => info.CanExtractAudio)
                    .OrderByDescending(info => info.AudioBitrate)
                    .First();

                return video;
            }
            catch (VideoNotAvailableException)
            {
                return null;
            }
            catch (YoutubeParseException)
            {
                return null;
            }
        }

        public bool CheckOnline()
        {
            var video = GetVideoInfo();

            if (video == null) return false;

            _name = video.Title;
            return true;
        }

        public int GetSize()
        {
            return 0;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
