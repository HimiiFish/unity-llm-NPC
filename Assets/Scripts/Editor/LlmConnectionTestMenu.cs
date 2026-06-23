using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class LlmConnectionTestMenu
{
    private const string MenuPath = "Tools/LLM/测试连接";
    private const string TestUserMessage = "你好，请用一句话自我介绍。";

    [MenuItem(MenuPath)]
    private static void RunConnectionTest()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("[LLM] 请先进入 Play Mode，再点击「测试连接」。");
            return;
        }

        RunConnectionTestAsync();
    }

    private static async void RunConnectionTestAsync()
    {
        Debug.Log("[LLM] 开始连通性测试…");

        try
        {
            LlmApiClient apiClient = new LlmApiClient();
            string reply = await apiClient.SendChatCompletionAsync(TestUserMessage);
            Debug.Log($"[LLM] 连接成功。回复: {reply}");
        }
        catch (Exception exception)
        {
            Debug.LogError($"[LLM] 连接失败: {exception}");
        }
    }
}