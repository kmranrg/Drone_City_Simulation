
using UnityEngine;


public class DroneController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float ascendSpeed = 3f;
    public float rotationSpeed = 5f;
    public float fastRotationSpeed = 200f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Increase drag to ensure the drone stops moving when no keys are pressed
        rb.drag = 5f;
        rb.angularDrag = 5f;

        // Disable gravity if you don't want the drone to fall when not ascending/descending
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Reset velocities
        Vector3 movement = Vector3.zero;
        float ascend = 0f;
        float yaw = 0f;

        // Ascend/Descend
        if (Input.GetKey(KeyCode.W))
        {
            ascend = ascendSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ascend = -ascendSpeed;
        }

        // Movement (using arrow keys)
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x = -moveSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.x = moveSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement.z = moveSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movement.z = -moveSpeed;
        }

        // Rotation
        if (Input.GetKey(KeyCode.A))
        {
            yaw = -rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            yaw = rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            yaw = -fastRotationSpeed;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            yaw = fastRotationSpeed;
        }

        // Apply movement and rotation
        Vector3 velocity = transform.TransformDirection(movement) + new Vector3(0f, ascend, 0f);
        rb.velocity = velocity;

        rb.angularVelocity = new Vector3(0f, yaw * Mathf.Deg2Rad, 0f);
    }
    public bool IsDroneActive()
{
    // Check if any movement input is detected
    return Input.GetKey(KeyCode.W) ||
           Input.GetKey(KeyCode.S) ||
           Input.GetKey(KeyCode.A) ||
           Input.GetKey(KeyCode.D) ||
           Input.GetKey(KeyCode.Z) ||
           Input.GetKey(KeyCode.C) ||
           Input.GetKey(KeyCode.UpArrow) ||
           Input.GetKey(KeyCode.DownArrow) ||
           Input.GetKey(KeyCode.LeftArrow) ||
           Input.GetKey(KeyCode.RightArrow);
}
void OnCollisionEnter(Collision collision)
    {
        // Detect collision and apply bounce-back force
        // Calculate the reflection vector
        Vector3 incomingVelocity = rb.velocity;
        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal);

        // Apply the bounce-back force
            // Optional: Log collision information
        Debug.Log("Drone collided with " + collision.gameObject.name);
    }
}