namespace Rabbit.Boot.Starter.IdentityServer.Client
{
    public class ClientOptions
    {
        private string _url;

        public string Url
        {
            get => _url;
            set => _url = value.TrimEnd('/');
        }

        public IdentityOptions Identity { get; set; }
    }

    public class IdentityOptions
    {
        private string _url;

        public string Url
        {
            get => _url;
            set => _url = value.TrimEnd('/');
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}