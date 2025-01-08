using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    public GameObject player;
    public float heightAbovePlayer = 40f;

    private void LateUpdate()
    {
        if (player != null)
        {
            // Keep the camera's position directly above the player, maintaining the same y position
            transform.position = new Vector3(player.transform.position.x, heightAbovePlayer, player.transform.position.z);
        }
    }
}