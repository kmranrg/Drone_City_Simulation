using UnityEngine;

public class DroneControllerMetaQuest : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float ascendSpeed = 3f;
    public float rotationSpeed = 5f;
    public float fastRotationSpeed = 200f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Increase drag to ensure the drone stops moving when no input is given
        rb.drag = 5f;
        rb.angularDrag = 5f;

        // Disable gravity if you don't want the drone to fall
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Reset velocities
        Vector3 movement = Vector3.zero;
        float ascend = 0f;
        float yaw = 0f;

        // Ascend/Descend (using controller buttons A and B)
        if (OVRInput.Get(OVRInput.Button.One)) // A button
        {
            Debug.Log("A Button Pressed");
            ascend = ascendSpeed;
        }
        else if (OVRInput.Get(OVRInput.Button.Two)) // B button
        {
            Debug.Log("B Button Pressed");
            ascend = -ascendSpeed;
        }

        // Movement (using the primary thumbstick)
        Vector2 primaryThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        movement.x = primaryThumbstick.x * moveSpeed;  // Left/Right movement
        movement.z = primaryThumbstick.y * moveSpeed;  // Forward/Backward movement

        // Rotation (using secondary thumbstick or triggers for finer control)
        Vector2 secondaryThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        yaw = secondaryThumbstick.x * rotationSpeed;

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            yaw = -fastRotationSpeed;
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            yaw = fastRotationSpeed;
        }

        // Apply movement and rotation
        Vector3 velocity = transform.TransformDirection(movement) + new Vector3(0f, ascend, 0f);
        rb.velocity = velocity;

        rb.angularVelocity = new Vector3(0f, yaw * Mathf.Deg2Rad, 0f);
    }

    // void FixedUpdate()
    // {
    //     // Detect active controller
    //     var activeController = OVRInput.GetActiveController();
    //     Debug.Log($"Active Controller: {activeController}");

    //     // Ascend/Descend (using controller buttons A and B)
    //     if (OVRInput.Get(OVRInput.Button.One)) // A button
    //     {
    //         Debug.Log("A Button Pressed");
    //         rb.velocity += Vector3.up * ascendSpeed * Time.deltaTime;
    //     }
    //     else if (OVRInput.Get(OVRInput.Button.Two)) // B button
    //     {
    //         Debug.Log("B Button Pressed");
    //         rb.velocity -= Vector3.up * ascendSpeed * Time.deltaTime;
    //     }

    //     // Thumbstick movement
    //     Vector2 primaryThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
    //     if (primaryThumbstick != Vector2.zero)
    //     {
    //         Debug.Log($"Thumbstick Input: {primaryThumbstick}");
    //         Vector3 move = new Vector3(primaryThumbstick.x, 0, primaryThumbstick.y);
    //         rb.velocity = transform.TransformDirection(move * moveSpeed * Time.deltaTime);
    //     }
    // }


    public bool IsDroneActive()
    {
        // Check if any controller input is detected
        return OVRInput.Get(OVRInput.Button.One) ||
               OVRInput.Get(OVRInput.Button.Two) ||
               OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ||
               OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) ||
               OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero ||
               OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick) != Vector2.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detect collision and apply bounce-back force
        Vector3 incomingVelocity = rb.velocity;
        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal);

        rb.velocity = reflectVelocity;

        // Log collision information
        Debug.Log("Drone collided with " + collision.gameObject.name);
    }
}
