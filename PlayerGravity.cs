using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerGravity : MonoBehaviour
{
    public float gravity = 10f; // Valor predeterminado de gravedad
    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!controller.isGrounded) // Solo aplicar gravedad si no está en el suelo
        {
            velocity.y -= gravity * Time.deltaTime; // Aplicar gravedad
        }
        else
        {
            velocity.y = -0.1f; // Mantener al jugador en el suelo
        }

        controller.Move(velocity * Time.deltaTime); // Mover al jugador con la gravedad aplicada
    }
}
