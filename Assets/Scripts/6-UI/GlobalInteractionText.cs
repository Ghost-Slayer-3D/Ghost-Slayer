using UnityEngine;
using TMPro;

public class GlobalInteractionText : MonoBehaviour
{
    public static GlobalInteractionText Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI globalText; // Reference to the TMP text in the canvas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMessage(string message)
    {
        if (globalText == null)
        {
            Debug.LogError("Global text UI element is not assigned!");
            return;
        }

        // Set the new message and ensure it's visible
        globalText.text = message;
        globalText.gameObject.SetActive(true);
    }
}
