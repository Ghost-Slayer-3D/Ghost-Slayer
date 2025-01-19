using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuMessageHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Display Settings")]
    [SerializeField] private float displayDuration = 5f;

    private void Start()
    {
        // Retrieve the saved message from PlayerPrefs
        string message = PlayerPrefs.GetString("MainMenuMessage", "");
        PlayerPrefs.DeleteKey("MainMenuMessage"); // Clear the key after retrieving

        if (!string.IsNullOrEmpty(message))
        {
            ShowMessage(message);
        }
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;

            // Set the color based on the message content
            if (message.ToLower().Contains("won"))
            {
                messageText.color = Color.green; // Green for winning
            }
            else if (message.ToLower().Contains("lost"))
            {
                messageText.color = Color.red; // Red for losing
            }

            messageText.gameObject.SetActive(true);

            // Hide the message after the specified duration
            Invoke(nameof(HideMessage), displayDuration);
        }
    }

    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}
