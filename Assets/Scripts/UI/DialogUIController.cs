using System;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TMP_InputField inputField;

    public void ShowDialog(string message)
    {
        dialogText.text = message;
    }

    private void Update()
    {
        SendMessage();
    }
    
    private async void SendMessage()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LlmApiClient apiClient = new LlmApiClient();
            string reply = await apiClient.SendChatCompletionAsync(inputField.text);
            ShowDialog(reply);
        }
    }
}