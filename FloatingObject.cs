using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatSpeed = 1f;  // Velocidad del movimiento de flotación
    public float floatAmplitude = 0.2f;  // Amplitud del movimiento en Y

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;  // Guardamos la posición inicial
    }

    void Update()
    {
        // Movimiento senoidal para dar el efecto de flotación
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
