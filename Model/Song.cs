using System.ComponentModel.DataAnnotations;

namespace MixCreator.Model
{
    public class Song
    {
        [Key]
        public string Guid { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Progress { get; set; }

        public string Size { get; set; }

        public string Downloaded { get; set; }

        public SongStatus Status { get; set; }

        public string Path { get; set; }

        public Song()
        {
            Guid = System.Guid.NewGuid().ToString();
            Status = SongStatus.Idle;
        }

        public void CopyValues(Song other)
        {
            this.Guid = other.Guid;
            this.Order = other.Order;
            this.Name = other.Name;
            this.Url = other.Url;
            this.Progress = other.Progress;
            this.Size = other.Size;
            this.Downloaded = other.Downloaded;
            this.Status = other.Status;
            this.Path = other.Path;
        }

        public void ResetValues()
        {
            // Guid stays the same
            // Order stays the same
            this.Name = null;
            this.Url = null;
            this.Progress = null;
            this.Size = null;
            this.Downloaded = null;
            this.Status = SongStatus.Idle;
            this.Path = null;
        }

        public enum SongStatus
        {
            Idle,
            WaitCheck,
            Checking,
            Online,
            WaitDownload,
            Downloading,
            Complete,
            WaitConvert,
            Done,
            Offline
        }
    }
}
