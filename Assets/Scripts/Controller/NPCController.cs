using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public static bool IsPlayerInRange { get; private set; }

    [SerializeField] private TextMeshPro promptText;

    private void Awake()
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        IsPlayerInRange = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        IsPlayerInRange = false;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseDialogue();
        }
    }
}