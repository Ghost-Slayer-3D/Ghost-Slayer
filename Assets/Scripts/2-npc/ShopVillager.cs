using UnityEngine;

/**
 * Extends the Villager script to handle shop functionality.
 * Displays the shop panel when the player interacts with this villager.
 */
public class ShopVillager : Villager
{
    [SerializeField] private GameObject shopPanel; // Reference to the shop UI panel

    private void Start()
    {
        // Ensure the shop panel is hidden at the start
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Shop panel not assigned for " + gameObject.name);
        }
    }

    public override void Interact()
    {
        base.Interact(); // Optionally call base interaction (e.g., show message)
        OpenShop();
    }

    private void OpenShop()
    {
        if (shopPanel == null)
        {
            Debug.LogError("Shop panel is not assigned for " + gameObject.name);
            return;
        }

        Debug.Log("Opening shop panel for " + gameObject.name);
        shopPanel.SetActive(true);

        // Pause the game and show the cursor
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseShop()
    {
        if (shopPanel == null)
        {
            Debug.LogError("Shop panel is not assigned for " + gameObject.name);
            return;
        }

        Debug.Log("Closing shop panel for " + gameObject.name);
        shopPanel.SetActive(false);

        // Resume the game and hide the cursor
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
