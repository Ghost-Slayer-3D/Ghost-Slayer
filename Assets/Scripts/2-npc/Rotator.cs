using UnityEngine;

/**
 * This component just rotates its object between angular bounds.
 */
public class Rotator : MonoBehaviour
{
    [SerializeField] private float minAngle = -90;
    [SerializeField] private float maxAngle = 90;
    [SerializeField] private float angularSpeed = 30;

    [SerializeField] private int direction = 1;

    private void Update()
    {
        transform.Rotate(new Vector3(0, direction * angularSpeed * Time.deltaTime, 0));

        float angle = transform.rotation.eulerAngles.y;

        if (angle > 180)
        {
            angle -= 360;
        }

        if (angle <= minAngle)
        {
            direction = 1;
        }

        if (angle >= maxAngle)
        {
            direction = -1;
        }
    }
}
