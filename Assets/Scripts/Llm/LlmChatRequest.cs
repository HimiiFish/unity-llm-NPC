using System.Collections.Generic;
using Newtonsoft.Json;

public class LlmChatRequest
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("messages")]
    public List<ChatMessage> Messages { get; set; }
}