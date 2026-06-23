using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class LlmSecretsLoader
{
    private const string SecretsFileName = "LlmSecrets.json";

    public static LlmSecrets LoadFromProjectRoot()
    {
        string secretsFilePath = Path.GetFullPath(
            Path.Combine(Application.dataPath, "..", SecretsFileName));

        if (!File.Exists(secretsFilePath))
        {
            throw new FileNotFoundException(
                $"未找到配置文件: {secretsFilePath}。请复制 LlmSecrets.json.example 并填写真实 Key。");
        }

        string jsonText = File.ReadAllText(secretsFilePath);
        LlmSecrets secrets = JsonConvert.DeserializeObject<LlmSecrets>(jsonText);

        if (secrets == null)
        {
            throw new JsonException("LlmSecrets.json 解析结果为空。");
        }

        if (string.IsNullOrWhiteSpace(secrets.ApiKey))
        {
            throw new JsonException("LlmSecrets.json 中 apiKey 为空。");
        }

        if (string.IsNullOrWhiteSpace(secrets.BaseUrl))
        {
            throw new JsonException("LlmSecrets.json 中 baseUrl 为空。");
        }

        if (string.IsNullOrWhiteSpace(secrets.Model))
        {
            throw new JsonException("LlmSecrets.json 中 model 为空。");
        }

        return secrets;
    }
}