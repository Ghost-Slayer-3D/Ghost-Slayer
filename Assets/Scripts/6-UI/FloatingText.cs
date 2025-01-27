using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private Transform target; // The target the text should follow
    private Camera mainCamera;
    private Vector3 initialOffset = new Vector3(0f, 2f, 0f); // Initial position above the target
    private float duration = 2f; // Duration the text is shown
    private float upwardDistance = 0.5f; // Distance to move upward
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

        // Smooth upward movement over time
        elapsedTime += Time.deltaTime;
        float upwardOffset = Mathf.Lerp(0f, upwardDistance, elapsedTime / duration);

        // Position the text above the target with upward movement
        transform.position = target.position + initialOffset + new Vector3(0, upwardOffset, 0);

        // Ensure the text always faces the camera
        transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

        // Destroy the text after the duration
        if (elapsedTime >= duration)
        {
            Destroy(gameObject);
        }
    }
}
