using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AutoCaminar : MonoBehaviour
{
    public GameObject visioncam;
    public const int AnguloRecto = 90;
    public bool EstaCaminando;
    public float speed;
    public bool CaminarPulsando;
    public bool CaminarMirando;
    public float AnguloUmbral;
    public bool CongelarPosicionY;
    public float CompensarY;
    public float jumpForce = 5.0f;
    public float gravity = 9.81f;
    
    private float verticalVelocity = 0.0f;
    private CharacterController controlador;
    private Vector3 moveDirection = Vector3.zero; // Inicializamos moveDirection

    void Start()
    {
        controlador = GetComponent<CharacterController>();

        // 🔴 Desactivar el movimiento al inicio para evitar problemas con el spawn
        EstaCaminando = false;

        // 🕐 Esperar 1 segundo antes de permitir caminar
        StartCoroutine(ActivarAutoCaminar());
    }

    IEnumerator ActivarAutoCaminar()
    {
        yield return new WaitForSeconds(1.0f); // Esperar 1 segundo
        EstaCaminando = true; // Ahora sí puede caminar
    }

    void Update()
    {
        // Obtener ángulo corregido de la cámara
        float anguloCamara = GetCameraAngle();

        // Lógica para detectar si debe caminar
        if (CaminarMirando && !CaminarPulsando)
        {
            if (!EstaCaminando && anguloCamara >= AnguloUmbral && anguloCamara <= AnguloRecto)
            {
                EstaCaminando = true;
            }
            else if (EstaCaminando && (anguloCamara < AnguloUmbral || anguloCamara > AnguloRecto))
            {
                EstaCaminando = false;
            }
        }

        // Movimiento solo si está activado
        if (EstaCaminando)
        {
            Caminar();
        }

        // Ajustar la posición en Y si es necesario
        if (CongelarPosicionY)
        {
            AjustarAltura();
        }
        
        // Salto
        if (Input.GetButtonDown("Jump") && controlador.isGrounded)
        {
            verticalVelocity = jumpForce;
        }

        // Aplicar gravedad
        if (!controlador.isGrounded) // Si no está tocando el suelo
        {
            verticalVelocity -= gravity * Time.deltaTime; // Aplicar gravedad
        }
        else
        {
            verticalVelocity = 0; // Mantener al personaje pegado al suelo
        }

        // Asignar la gravedad al movimiento en Y
        moveDirection.y = verticalVelocity;
    }

    // Método para mover al personaje usando CharacterController
    public void Caminar()
    {
        Vector3 direccion = new Vector3(visioncam.transform.forward.x, 0, visioncam.transform.forward.z).normalized;
        moveDirection.x = direccion.x * speed * Time.deltaTime;  // Movimiento en X
        moveDirection.z = direccion.z * speed * Time.deltaTime;  // Movimiento en Z

        controlador.Move(moveDirection); // Aplicar el movimiento con gravedad
    }

    // Método para normalizar ángulos y evitar problemas con valores cíclicos
    private float GetCameraAngle()
    {
        float angle = visioncam.transform.eulerAngles.x;
        return (angle > 180) ? angle - 360 : angle; // Convertir a un rango de -180 a 180
    }

    // Método para ajustar la altura sin atravesar el suelo
    private void AjustarAltura()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            controlador.Move(new Vector3(0, hit.point.y + CompensarY - transform.position.y, 0));
        }
    }

    // Visualizar con Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 start = transform.position;
        Vector3 direction = new Vector3(visioncam.transform.forward.x, 0, visioncam.transform.forward.z).normalized;
        Gizmos.DrawRay(start, direction * 2f);
    }
}
