namespace MixCreator.Exception
{
    public class SongNotFoundException : System.Exception
    {
        public SongNotFoundException()
        {
        }

        public SongNotFoundException(string message)
        : base(message)
        {
        }

        public SongNotFoundException(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}