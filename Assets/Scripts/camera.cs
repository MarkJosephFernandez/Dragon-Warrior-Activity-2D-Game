using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -10); // Default offset

    [Header("Movement Settings")]
    [Range(0, 1)]
    public float smoothSpeed = 0.125f; // Adjusts how "heavy" the camera feels

    [Header("Boundary Settings")]
    public bool useBoundaries = false;
    public float minX, maxX, minY, maxY; // Limits so the camera doesn't leave the forest

    void LateUpdate()
    {
        // 1. Safety check: Exit if the player was destroyed/not assigned
        if (target == null) return;

        // 2. Calculate the position we want to be at
        Vector3 desiredPosition = target.position + offset;

        // 3. (Optional) Clamp the position so the camera stays within the level
        if (useBoundaries)
        {
            float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);
            desiredPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        }

        // 4. Smoothly move to that position
        // Using SmoothDamp or Lerp—Lerp is great for that "floaty" feel
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}