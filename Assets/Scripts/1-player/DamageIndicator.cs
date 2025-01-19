using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject damageTextPrefab; // Prefab for floating text
    [SerializeField] private Vector3 offset = new Vector3(0, 1, 0); // Offset above the player
    [SerializeField] private Transform targetPart; // Transform for the specific part (e.g., head or custom target)

    public void ShowDamage(string text, Color color)
    {
        if (damageTextPrefab == null || targetPart == null)
        {
            Debug.LogError("DamageTextPrefab or TargetPart is not assigned.");
            return;
        }

        // Instantiate the damage text prefab at the target position
        GameObject damageText = Instantiate(
            damageTextPrefab,
            targetPart.position + offset, // Position relative to the target
            Quaternion.identity
        );

        // Configure the text
        TextMeshPro tmp = damageText.GetComponent<TextMeshPro>();
        if (tmp != null)
        {
            tmp.text = text;
            tmp.color = color;
        }

        // Add movement and camera-facing behavior
        FloatingText floatingText = damageText.GetComponent<FloatingText>();
        if (floatingText != null)
        {
            floatingText.SetTarget(targetPart); // Ensure the text follows the correct part
        }

        // Destroy the text after 2 seconds
        Destroy(damageText, 2f);
    }
}
