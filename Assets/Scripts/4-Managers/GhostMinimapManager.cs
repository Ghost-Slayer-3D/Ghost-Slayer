using UnityEngine;

public class GhostMinimapManager : MonoBehaviour
{
    public GameObject ghostIconPrefab; // Assign the prefab in the Inspector
    public float iconHeightOffset = 2f; // Height above each ghost

    void Start()
    {
        // Initialize existing enemies at the start
        AddIconsToAllEnemies();
    }

    void Update()
    {
        // Continuously check for new enemies and add icons
        AddIconsToAllEnemies();
        LockIconRotation(); // Ensure icons do not rotate
    }

    void AddIconsToAllEnemies()
    {
        // Find all GameObjects tagged as "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Check if the enemy already has an icon to avoid duplicates
            if (enemy.transform.Find("MinimapIcon") == null)
            {
                // Instantiate the icon as a child of the enemy
                GameObject icon = Instantiate(ghostIconPrefab, enemy.transform);

                // Name it to prevent duplicates
                icon.name = "MinimapIcon";

                // Set local position above the enemy
                icon.transform.localPosition = new Vector3(0, iconHeightOffset, 0);

                // Lock the rotation so it doesn't tilt with the enemy
                icon.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }

    void LockIconRotation()
    {
        // Find all minimap icons and lock their rotation
        GameObject[] icons = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in icons)
        {
            Transform icon = enemy.transform.Find("MinimapIcon");
            if (icon != null)
            {
                // Keep the rotation fixed (locked to 90 degrees X-axis)
                icon.rotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }
}
