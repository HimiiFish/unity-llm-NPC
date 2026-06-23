using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class LlmApiClient
{
    private const int RequestTimeoutSeconds = 30;
    private const string ChatCompletionsPath = "/chat/completions";

    private readonly LlmSecrets secrets;

    public LlmApiClient()
    {
        secrets = LlmSecretsLoader.LoadFromProjectRoot();
    }

    // 发送聊天完成请求, 返回assistant的回复
    public async Task<string> SendChatCompletionAsync(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            throw new ArgumentException("userMessage 不能为空。", nameof(userMessage));
        }

        string requestUrl = BuildChatCompletionsUrl(secrets.BaseUrl);

        LlmChatRequest chatRequest = new LlmChatRequest
        {
            Model = secrets.Model,
            Messages = new List<ChatMessage>
            {
                new ChatMessage("user", userMessage)
            }
        };

        string requestBodyJson = JsonConvert.SerializeObject(chatRequest);
        byte[] requestBodyBytes = Encoding.UTF8.GetBytes(requestBodyJson);

        using UnityWebRequest webRequest = new UnityWebRequest(requestUrl, UnityWebRequest.kHttpVerbPOST);
        webRequest.uploadHandler = new UploadHandlerRaw(requestBodyBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", $"Bearer {secrets.ApiKey}");
        webRequest.timeout = RequestTimeoutSeconds;

        await SendWebRequestAsync(webRequest);

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            string responseText = webRequest.downloadHandler?.text ?? string.Empty;
            throw new InvalidOperationException(
                $"HTTP 请求失败: {webRequest.responseCode} {webRequest.error}\n响应体: {responseText}");
        }

        string responseJson = webRequest.downloadHandler.text;
        LlmChatResponse chatResponse = JsonConvert.DeserializeObject<LlmChatResponse>(responseJson);

        if (chatResponse?.Choices == null || chatResponse.Choices.Count == 0)
        {
            throw new InvalidOperationException($"响应中没有 choices 字段。\n原始响应: {responseJson}");
        }

        string assistantReply = chatResponse.Choices[0].Message?.Content;

        if (string.IsNullOrWhiteSpace(assistantReply))
        {
            throw new InvalidOperationException($"assistant 回复为空。\n原始响应: {responseJson}");
        }

        return assistantReply;
    }

    private static string BuildChatCompletionsUrl(string baseUrl)
    {
        return baseUrl.TrimEnd('/') + ChatCompletionsPath;
    }

    private static Task SendWebRequestAsync(UnityWebRequest webRequest)
    {
        TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
        UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();
        operation.completed += _ =>
        {
            if (operation.webRequest.result == UnityWebRequest.Result.ConnectionError ||
                operation.webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                completionSource.TrySetResult(null);
            }
            else
            {
                completionSource.TrySetResult(null);
            }
        };

        return completionSource.Task;
    }
}