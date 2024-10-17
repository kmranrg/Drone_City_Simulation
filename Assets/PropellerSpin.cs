
using UnityEngine;
public class PropellerSpin : MonoBehaviour
{
    public float spinSpeed = 2000f; // Adjust as needed
    public DroneController droneController;
    public Vector3 rotationAxis = Vector3.up; // Default to Y-axis

    void Update()
    {
        // Normalize the rotation axis to ensure consistent rotation speed
        Vector3 normalizedAxis = rotationAxis.normalized;

        // Check if the drone is moving or ascending/descending
        if (droneController != null && droneController.IsDroneActive())
        {
            // Spin the propeller
            transform.Rotate(normalizedAxis, spinSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            // Optionally, slow down or stop the propeller when idle
            // For now, we'll keep spinning even when idle
            transform.Rotate(normalizedAxis, spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}

