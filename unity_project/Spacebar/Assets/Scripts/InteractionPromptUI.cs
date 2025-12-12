using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject promptPanel;

    private static InteractionPromptUI instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HidePrompt();
    }

    public static void ShowPrompt(string message)
    {
        if (instance == null) return;

        instance.promptText.text = message;
        instance.promptPanel.SetActive(true);
    }

    public static void HidePrompt()
    {
        if (instance == null) return;

        instance.promptPanel.SetActive(false);
    }
}
