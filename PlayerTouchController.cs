using UnityEngine;

public class PlayerTouchController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocidad de movimiento del jugador
    public float rotationSpeed = 700f; // Sensibilidad de la cámara
    public Joystick movementJoystick;  // Joystick de movimiento
    public Joystick cameraJoystick;  // Joystick de cámara
    public Transform cameraTransform;  // Cámara que sigue al jugador

    private CharacterController characterController;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector3 velocity; // Para la gravedad
    public float gravity = 9.81f; // Intensidad de la gravedad

    void Start()
    {
        characterController = GetComponent<CharacterController>();  // Obtén el CharacterController del jugador

        // Cargar la sensibilidad de la cámara desde PlayerPrefs
        rotationSpeed = PlayerPrefs.GetFloat("CameraSensitivity", 300f); // Valor predeterminado de 300f si no está guardado
    }

    void Update()
    {
        MovePlayer();  // Mover al jugador con el Joystick
        RotateCamera();  // Rotar la cámara con el joystick de la cámara
        ApplyGravity(); // Aplicar gravedad
    }

    // Método para mover al jugador usando el Joystick de movimiento
    void MovePlayer()
    {
        float moveX = movementJoystick.Horizontal;  // Movimiento horizontal (A/D o izquierda/derecha)
        float moveZ = movementJoystick.Vertical;    // Movimiento vertical (W/S o adelante/atrás)

        // Dirección de movimiento basada en la cámara
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        
        // Asegurar que el movimiento es solo en el plano X-Z (eliminar Y)
        moveDirection.y = 0f;

        // Aplicar movimiento si hay entrada del joystick
        if (moveDirection.magnitude >= 0.1f)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    // Método para rotar la cámara según el Joystick o la pantalla
    void RotateCamera()
    {
        rotationX += cameraJoystick.Horizontal * rotationSpeed * Time.deltaTime;  // Movimiento de cámara en el eje X
        rotationY -= cameraJoystick.Vertical * rotationSpeed * Time.deltaTime;    // Movimiento de cámara en el eje Y

        rotationY = Mathf.Clamp(rotationY, -90f, 90f);  // Limitar la rotación vertical

        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);  // Rotar solo en X
        transform.rotation = Quaternion.Euler(0, rotationX, 0);  // Rotar el jugador solo en Y
    }

    // Método para aplicar gravedad al jugador
    void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;  // Aplicar gravedad solo si no está en el suelo
        }
        else
        {
            velocity.y = -2f; // Pequeño valor negativo para mantener contacto con el suelo
        }

        characterController.Move(velocity * Time.deltaTime);  // Aplicar la gravedad al CharacterController
    }
}
