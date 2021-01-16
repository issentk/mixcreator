namespace MixCreator.Model
{
    public class SongContextFactory : IContextFactory<Song>
    {
        public IContext<Song> CreateContext(IConfig config)
        {
            return new SongContext(config);
        }
    }
}
