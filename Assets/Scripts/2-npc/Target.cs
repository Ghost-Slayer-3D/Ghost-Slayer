using UnityEngine;

/**
 * This component denotes a target that can be shown on the scene editor with a red sphere.
 * It is used by TargetRunner.
 */
public class Target : MonoBehaviour
{
    [SerializeField] private float sphereRadius = 0.3f;
    [SerializeField] private Color sphereColor = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }
}
