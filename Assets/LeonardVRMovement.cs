using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Animator))]
public class LeonardVRMovement : MonoBehaviour
{
    public Transform vrHeadset;           // Assign the VR headset transform here
    public float movementThreshold = 0.1f; // Threshold to start walking animation
    public float movementSpeed = 2.0f;     // Speed of Leonard's movement

    private Animator animator;
    private Vector3 lastPosition;          // To store last position of headset

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Store the initial position of the VR headset
        if (vrHeadset != null)
        {
            lastPosition = vrHeadset.position;
        }
    }

    void Update()
    {
        if (vrHeadset == null) return; // Exit if VR headset not assigned

        // Calculate movement distance of the VR headset
        Vector3 headsetMovement = vrHeadset.position - lastPosition;
        headsetMovement.y = 0; // Ignore vertical movement for walking

        float distanceMoved = headsetMovement.magnitude;

        // Check if the distance exceeds the movement threshold
        if (distanceMoved > movementThreshold)
        {
            // Move Leonard in the direction of VR headset movement
            Vector3 moveDirection = headsetMovement.normalized;
            transform.position += moveDirection * movementSpeed * Time.deltaTime;

            // Set the walking animation
            animator.SetFloat("Speed", movementSpeed);

            // Rotate Leonard to face the movement direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        else
        {
            // Set to idle animation if below threshold
            animator.SetFloat("Speed", 0);
        }

        // Update last position for the next frame
        lastPosition = vrHeadset.position;
    }
}
