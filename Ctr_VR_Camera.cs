using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ctr_VR_Camera : MonoBehaviour
{
    public float Velocidad = 50f;
    public float SencibilidaddeArrastre = 2f;
    public bool RotaciondelMouse = true;
    public bool RotaciondeArrastre = true;

    private float x = 0.0f;
    private float y = 0.0f;
    void Update()
    {
        if (RotaciondelMouse && Input.GetMouseButton(0))
        {
            x += Input.GetAxis("Mouse X") * Velocidad * Time.deltaTime * SencibilidaddeArrastre; //GetAxist se usa para acceder a un eje virtual tanto para eje horizontal o vertical
            y -= Input.GetAxis("Mouse Y") * Velocidad * Time.deltaTime * SencibilidaddeArrastre;

        }
        y = ClampAngle(y, -85, 85f);
        Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
        transform.rotation = rotation;
    }
    public static float ClampAngle(float angle, float min, float max) // angle es el ángulo entre dos puntos dados
    {
        if (angle < -360.0f)
            angle += 360.0f;

        if (angle > 360.0f)
            angle -= 360.0f;

        return Mathf.Clamp(angle, min, max); // El valor de un punto flotante restringido dentro de un rango definido por valores Max y min.
    }
}