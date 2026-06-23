using Newtonsoft.Json;

public class LlmSecrets
{
    [JsonProperty("apiKey")]
    public string ApiKey { get; set; }

    [JsonProperty("baseUrl")]
    public string BaseUrl { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }
}