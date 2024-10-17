
using UnityEngine;

public class Dronefollower : MonoBehaviour
{
     [Header("Target Settings")]
    public Transform target;            // The drone's transform to follow

    [Header("Offset Settings")]
    public Vector3 offsetPosition = new Vector3(0f, 5f, -10f); // Camera position offset from the drone
    public Vector3 offsetRotation = Vector3.zero;              // Camera rotation offset

    [Header("Smoothness Settings")]
    public float positionSmoothness = 0.125f;  // Smoothness factor for position
    public float rotationSmoothness = 0.1f;    // Smoothness factor for rotation

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not assigned in SmoothCameraFollow script.");
            return;
        }

        // Desired position and rotation
        Vector3 desiredPosition = target.position + target.TransformDirection(offsetPosition);
        Quaternion desiredRotation = Quaternion.Euler(target.eulerAngles + offsetRotation);

        // Smoothly interpolate position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmoothness);

        // Smoothly interpolate rotation
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothness);

        // Apply the smoothed position and rotation to the camera
        transform.position = smoothedPosition;
        transform.rotation = smoothedRotation;
    }
    
}

