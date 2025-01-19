using UnityEngine;
using System.Collections.Generic;

public class CameraOcclusionHandler : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private LayerMask obstacleMask; // Layer mask for objects that can block the view

    private List<GameObject> hiddenObjects = new List<GameObject>(); // Track currently hidden objects

    void Update()
    {
        HandleOcclusion();
    }

    private void HandleOcclusion()
    {
        // Clear previous hidden objects
        foreach (GameObject obj in hiddenObjects)
        {
            SetObjectVisibility(obj, true);
        }
        hiddenObjects.Clear();

        // Cast a ray from the camera to the player
        Vector3 direction = player.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, direction.magnitude, obstacleMask);

        // Hide all objects hit by the ray
        foreach (var hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            SetObjectVisibility(hitObject, false);
            hiddenObjects.Add(hitObject);
        }
    }

    private void SetObjectVisibility(GameObject obj, bool isVisible)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            objRenderer.enabled = isVisible;
        }
    }
}
