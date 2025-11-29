using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = GlobalVariables.playerSpeed; // Speed of movement


    public float rotationSpeed = 10f; 

    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 moveInput; // Stores player input

    private void Start()
    {
        GlobalVariables.playerSpeed = 5;
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure the Rigidbody is not affected by rotation
        rb.freezeRotation = true;
    }

    private void Update()
    {
        float moveSpeed = GlobalVariables.playerSpeed;
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down
        float moveY = Input.GetKey(KeyCode.Space);

        // Create a movement vector
        moveInput = new Vector3(moveX, moveY, moveZ).normalized; // Normalize to maintain consistent speed
    }

    private void FixedUpdate()
    {
        // Move the Rigidbody based on input
        rb.MovePosition(rb.position + moveInput * GlobalVariables.playerSpeed * Time.fixedDeltaTime);

        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
