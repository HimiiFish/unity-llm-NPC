using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;

    private LlmApiClient llmApiClient;
    private bool isWaitingForResponse;
    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        llmApiClient = new LlmApiClient();

        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnSendButtonClicked);
        }
    }

    private void OnDestroy()
    {
        if (sendButton != null)
        {
            sendButton.onClick.RemoveListener(OnSendButtonClicked);
        }

        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }

    public void OnDialogueOpened()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();
        SetInputEnabled(true);
        inputField?.ActivateInputField();
    }

    public void OnDialogueClosed()
    {
        cancellationTokenSource?.Cancel();
        isWaitingForResponse = false;
        SetInputEnabled(false);
    }

    private void Update()
    {
        if (!isActiveAndEnabled || isWaitingForResponse)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessageAsync();
        }
    }

    private void OnSendButtonClicked()
    {
        SendMessageAsync();
    }

    private async void SendMessageAsync()
    {
        if (isWaitingForResponse || inputField == null)
        {
            return;
        }

        string userMessage = inputField.text.Trim();
        if (string.IsNullOrEmpty(userMessage))
        {
            return;
        }

        isWaitingForResponse = true;
        SetInputEnabled(false);
        ShowDialog("思考中…");

        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            string reply = await llmApiClient.SendChatCompletionAsync(userMessage);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            inputField.text = string.Empty;
            ShowDialog(reply);
        }
        catch (Exception exception)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                ShowDialog($"请求失败: {exception.Message}");
            }
        }
        finally
        {
            isWaitingForResponse = false;

            if (isActiveAndEnabled && !cancellationToken.IsCancellationRequested)
            {
                SetInputEnabled(true);
                inputField.ActivateInputField();
            }
        }
    }

    private void SetInputEnabled(bool enabled)
    {
        if (inputField != null)
        {
            inputField.interactable = enabled;
        }

        if (sendButton != null)
        {
            sendButton.interactable = enabled;
        }
    }

    public void ShowDialog(string message)
    {
        if (dialogText != null)
        {
            dialogText.text = message;
        }
    }
}