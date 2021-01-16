using MixCreator.Exception;
using MixCreator.Model;

namespace MixCreator.Provider
{
    public class DefaultProviderFactory : IProviderFactory
    {
        public IProvider GetProvider(Song song)
        {
            // Find appropriate provider
            if (song.Url.Contains("youtube.com"))
            {
                return new Youtube(song);
            }

            throw new ProviderNotFoundException();
        }
    }
}
