using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controlador_VRPlayer : MonoBehaviour
{
    private CharacterController ControladorPersonaje;
    private Vector3 MovimientoEnDireccion = Vector3.zero;
    /// <summary>
    /// Stores the current input values as a 2D vector, typically representing horizontal and vertical movement axes.
    /// </summary>
    private Vector2 Entrada;
    private CollisionFlags BanderasDeCollision;
    public float FuerzaAlTocalSuelo = 0.2f;
    public float MultiplicarGravedad = 1.5f;
    public Vector3 jumpforce;

    private bool puedeMoverse = false; // 🔴 Desactivar movimiento al inicio

    void Start()
    {
        ControladorPersonaje = GetComponent<CharacterController>();

        // Si hay un Rigidbody, desactívalo para evitar problemas
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // 🕐 Esperar 1 segundo antes de activar el movimiento
        StartCoroutine(ActivarMovimiento());
    }

    IEnumerator ActivarMovimiento()
    {
        yield return new WaitForSeconds(1.0f); // Esperar 1 segundo antes de moverse
        puedeMoverse = true; // 🔵 Ahora sí se mueve
    }

    void FixedUpdate()
    {
        if (!puedeMoverse) return; // 🔴 Bloquear movimiento hasta que pase 1 segundo

        Vector3 MovimientoDeseado = transform.forward * Entrada.y + transform.right * Entrada.x;

        // Usar el centro del CharacterController en lugar de transform.position
        Vector3 origen = ControladorPersonaje.bounds.center;
        RaycastHit hitInfo;

        if (Physics.SphereCast(origen, ControladorPersonaje.radius, Vector3.down, out hitInfo,
            ControladorPersonaje.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            MovimientoDeseado = Vector3.ProjectOnPlane(MovimientoDeseado, hitInfo.normal).normalized;
        }
        else
        {
            MovimientoDeseado = MovimientoDeseado.normalized;
        }

        if (ControladorPersonaje.isGrounded)
        {
            MovimientoEnDireccion.y = -FuerzaAlTocalSuelo;
        }
        else
        {
            MovimientoEnDireccion += Physics.gravity * MultiplicarGravedad * Time.fixedDeltaTime;
        }

        // Agregamos MovimientoDeseado a MovimientoEnDireccion
        MovimientoEnDireccion.x = MovimientoDeseado.x;
        MovimientoEnDireccion.z = MovimientoDeseado.z;

        BanderasDeCollision = ControladorPersonaje.Move(MovimientoEnDireccion * Time.fixedDeltaTime);
    }
}
