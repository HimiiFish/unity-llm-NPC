using System.Collections.Generic;
using Newtonsoft.Json;

public class LlmChatResponse
{
    [JsonProperty("choices")]
    public List<LlmChatChoice> Choices { get; set; }
}

public class LlmChatChoice
{
    [JsonProperty("message")]
    public ChatMessage Message { get; set; }
}