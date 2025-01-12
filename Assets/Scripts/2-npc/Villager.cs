using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class Villager : MonoBehaviour
{
    [SerializeField] private TextMeshPro interactionText; // TMP 3D Text object
    [SerializeField] private float displayDuration = 3f; // Duration to show the text

    private bool isInteracting = false;

    private void Start()
    {
        // Hide text initially
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Make sure it's inactive at the start
        }
        else
        {
            Debug.LogWarning("Interaction text not assigned for " + gameObject.name);
        }
    }

    // Called by Player Interaction script
    public virtual void Interact()
    {
        if (!isInteracting)
        {
            ShowMessage();
        }
    }

    private void ShowMessage()
    {
        // Display the pre-set message above the villager
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true); // Show the text
        }

        isInteracting = true;

        // Hide the text after display duration
        Invoke(nameof(HideMessage), displayDuration);
    }

    private void HideMessage()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Hide the text
            Debug.Log($"Hiding text for {gameObject.name}");
        }

        isInteracting = false;
    }
}
