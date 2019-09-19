using Newtonsoft.Json;

namespace RssReader
{
    public class RssLinks
    {
        [JsonProperty("link")]
        public string[] Uri { get; set; }
    }
}
