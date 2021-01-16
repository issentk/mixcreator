namespace MixCreator.Exception
{
    public class ProviderNotFoundException : System.Exception
    {
        public ProviderNotFoundException()
        {
        }

        public ProviderNotFoundException(string message)
        : base(message)
        {
        }

        public ProviderNotFoundException(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}