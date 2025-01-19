using UnityEngine;
using TMPro;

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
            interactionText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Interaction text not assigned for " + gameObject.name);
        }
    }

    public virtual void Interact()
    {
        if (!isInteracting)
        {
            ShowMessage();
        }
    }

    private void ShowMessage()
    {
        // Display 3D text above the villager
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true);
        }

        // Update the global canvas text
        if (interactionText != null)
        {
            GlobalInteractionText.Instance?.UpdateMessage(interactionText.text);
        }

        isInteracting = true;

        // Hide the 3D text after display duration
        Invoke(nameof(HideMessage), displayDuration);
    }

    private void HideMessage()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        isInteracting = false;
    }
}
