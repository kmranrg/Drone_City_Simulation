using UnityEngine;

public class DroneFollower : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The drone's transform to follow

    [Header("Offset Settings")]
    public Vector3 offsetPosition = new Vector3(0f, 5f, -10f); // Camera position offset from the drone
    public Vector3 offsetRotation = Vector3.zero; // Camera rotation offset

    [Header("Smoothness Settings")]
    public float positionSmoothness = 0.125f; // Smoothness factor for position
    public float rotationSmoothness = 0.1f;   // Smoothness factor for rotation

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned. Please assign the drone's Transform in the Inspector.");
        }
    }

    void Update()
    {
        if (target == null) return; // Exit if target is not assigned

        // Calculate desired position and rotation
        Vector3 desiredPosition = target.position + target.TransformDirection(offsetPosition);
        Quaternion desiredRotation = Quaternion.Euler(target.eulerAngles + offsetRotation);

        // Smoothly interpolate the camera's position and rotation
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmoothness);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothness);

        // Apply the calculated position and rotation to the camera
        transform.position = smoothedPosition;
        transform.rotation = smoothedRotation;
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            // Draw a line in the Scene view to visualize the offset
            Gizmos.color = Color.green;
            Gizmos.DrawLine(target.position, target.position + target.TransformDirection(offsetPosition));
            Gizmos.DrawWireSphere(target.position + target.TransformDirection(offsetPosition), 0.3f);
        }
    }
}
