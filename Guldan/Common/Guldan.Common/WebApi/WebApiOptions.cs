namespace Guldan.Common
{

    public class WebApiOptions
    {
        public WebApiOptions(string apiPrefix = null, string httpVerb = null)
        {
            ApiPrefix = apiPrefix;
            HttpVerb = httpVerb;
        }

        public string ApiPrefix { get; }

        public string HttpVerb { get; }
    }
}