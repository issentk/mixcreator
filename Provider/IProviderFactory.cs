using MixCreator.Provider;

namespace MixCreator.Model
{
    public interface IProviderFactory
    {
        IProvider GetProvider(Song song);
    }
}
