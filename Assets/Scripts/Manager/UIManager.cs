using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("引用")]
    [SerializeField] private GameObject dialoguePanelRoot;
    [SerializeField] private DialogUIController dialogUIController;
    [SerializeField] private PlayerController playerController;

    public bool IsDialogueOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (dialoguePanelRoot == null && dialogUIController != null)
        {
            dialoguePanelRoot = dialogUIController.gameObject;
        }

        dialogUIController = dialoguePanelRoot.GetComponent<DialogUIController>();

        CloseDialogueImmediate();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void OpenDialogue()
    {
        if (IsDialogueOpen)
        {
            return;
        }

        IsDialogueOpen = true;
        dialoguePanelRoot.SetActive(true);

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        dialogUIController?.OnDialogueOpened();
    }

    public void CloseDialogue()
    {
        if (!IsDialogueOpen)
        {
            return;
        }

        CloseDialogueImmediate();
    }

    public void ToggleDialogue()
    {
        if (IsDialogueOpen)
        {
            CloseDialogue();
        }
        else
        {
            OpenDialogue();
        }
    }

    private void CloseDialogueImmediate()
    {
        IsDialogueOpen = false;

        dialogUIController?.OnDialogueClosed();

        if (dialoguePanelRoot != null)
        {
            dialoguePanelRoot.SetActive(false);
        }

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }
    }
}