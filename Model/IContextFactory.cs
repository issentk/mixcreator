namespace MixCreator.Model
{
    public interface IContextFactory<T> where T : class
    {
        IContext<T> CreateContext(IConfig config);
    }
}