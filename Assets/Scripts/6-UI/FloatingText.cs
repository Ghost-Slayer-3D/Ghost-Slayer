using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private Transform target; // The target the text should follow
    private Camera mainCamera;
    private Vector3 initialOffset = new Vector3(0, 2, 0); // Initial position above the target
    private float duration = 2f; // Duration the text is shown
    private float downwardDistance = 0.5f; // Distance to move downward
    private float elapsedTime = 0f;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        mainCamera = Camera.main;
        transform.position = target.position + initialOffset; // Set initial position
    }

    private void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        // Smooth downward movement over time
        elapsedTime += Time.deltaTime;
        float downwardOffset = Mathf.Lerp(0, -downwardDistance, elapsedTime / duration);

        // Position the text above the target with downward movement
        transform.position = target.position + initialOffset + new Vector3(0, downwardOffset, 0);

        // Ensure the text always faces the camera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);

        // Destroy the text after the duration
        if (elapsedTime >= duration)
        {
            Destroy(gameObject);
        }
    }
}
