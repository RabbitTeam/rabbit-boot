namespace Rabbit.Boot.Starter.IdentityServer.Client
{
    public class IdentityClientOptions
    {
        private string _identityUrl;

        public string IdentityUrl
        {
            get => _identityUrl;
            set => _identityUrl = value?.TrimEnd('/');
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        private string _baseUrl;

        public string BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = value?.TrimEnd('/');
        }
    }
}