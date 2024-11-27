using UnityEngine;

public class MapHeadsetToLeonard : MonoBehaviour
{
    public Transform Leonard; // Leonard's Transform
    public Transform XRHeadset; // XR Headset Transform
    public Animator animator; // Leonard's Animator
    public float groundLevel = 0f; // Ground height (Y-axis)
    public float animationThreshold = 0.1f; // Minimum movement threshold for walking

    private Vector3 previousHeadsetPosition; // Track previous headset position

    void Start()
    {
        if (XRHeadset == null || Leonard == null || animator == null)
        {
            Debug.LogError("Missing references in MapHeadsetToLeonard script!");
            return;
        }

        // Initialize previous headset position
        previousHeadsetPosition = XRHeadset.position;
    }

    void Update()
    {
        // Calculate movement in XZ plane only (ignore Y-axis changes)
        Vector3 currentHeadsetPosition = XRHeadset.position;
        Vector3 headsetDelta = currentHeadsetPosition - previousHeadsetPosition;

        // Ignore small movements (head wobble, drift) by adding a threshold
        if (headsetDelta.magnitude > 0.001f)
        {
            // Update Leonard's position based on the delta in XZ plane
            Leonard.position += new Vector3(headsetDelta.x, 0, headsetDelta.z);

            // Calculate movement speed (adjust for frame time)
            float movementSpeed = headsetDelta.magnitude / Time.deltaTime;

            // Cap movement speed to avoid unrealistic values
            movementSpeed = Mathf.Clamp(movementSpeed, 0, 5); // Adjust the max speed (5) as necessary

            // Update Animator parameters
            animator.SetFloat("Speed", movementSpeed);
            animator.SetBool("isWalking", movementSpeed > animationThreshold);

            // Debug logs for monitoring
            Debug.Log($"Headset Position: {currentHeadsetPosition}");
            Debug.Log($"Leonard Position: {Leonard.position}");
            Debug.Log($"Movement Speed: {movementSpeed}");
        }

        // Update previous headset position
        previousHeadsetPosition = currentHeadsetPosition;
    }
}
