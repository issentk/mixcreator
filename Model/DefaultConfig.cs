namespace MixCreator.Model
{
    public class DefaultConfig : IConfig
    {
        public string GetDatabaseName()
        {
            return "songs.db";
        }
    }
}