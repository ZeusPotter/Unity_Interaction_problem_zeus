using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Controller_PC : MonoBehaviour
{
    public float speed = 6.0f;
    public float mouseSensitivity = 2.0f;
    public float jumpForce = 5.0f;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Vector3 velocity = Vector3.zero;
    private float rotationX = 0.0f;
    public bool canMove = true;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
{
    if (!canMove) return; // ⛔ Detener entrada si no se permite mover

    // --- ROTACIÓN DE CÁMARA ---
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

    rotationX -= mouseY;
    rotationX = Mathf.Clamp(rotationX, -90f, 90f);

    transform.Rotate(0, mouseX, 0);
    Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

    // --- MOVIMIENTO ---
    float moveX = Input.GetAxis("Horizontal");
    float moveZ = Input.GetAxis("Vertical");

    Vector3 move = transform.right * moveX + transform.forward * moveZ;

    if (controller.isGrounded)
    {
        velocity = move * speed;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
        }
    }
    else
    {
        Vector3 horizontalVelocity = move * speed;
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;
    }

    velocity.y -= gravity * Time.deltaTime;

    controller.Move(velocity * Time.deltaTime);
}


    public void UpdateMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}
