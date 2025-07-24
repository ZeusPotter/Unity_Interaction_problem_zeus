using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giroscopio : MonoBehaviour
{
   public GameObject VRCamaras;
	private float PosicionInicialY = 0f;
	private float PosicionDelGyroEnY = 0f;
	private float CalibrarEnLaPosicionY = 0f;
	public bool SeInicioElJuego;
	// Start is called before the first frame update
    void Start()
    {
       Input.gyro.enabled = true; // Accede al giroscopio del dispositivo.
       PosicionInicialY = VRCamaras.transform.eulerAngles.y; //eulerAngles es una propiedad de Transform que devuelve la rotación de un objeto en formato de grados de Euler, es decir, tres valores (uno para cada eje: X, Y y Z) que indican la rotación en esos ejes.   
    }

    // Update is called once per frame
    void Update()
    {
        AplicarRotacionDelGyroscopio();
		AplicarCalibracion();
		
		if (SeInicioElJuego == true)
		   {
		     Invoke("CalibrarEnLaPosicionY", 3f); //Invoke es útil cuando necesitas ejecutar una función con un retraso
			 SeInicioElJuego = false;
		   }
	}
	void AplicarRotacionDelGyroscopio()
	{
	   VRCamaras.transform.rotation = Input.gyro.attitude; //orientación del espacio en el dispositivo
	   VRCamaras.transform.Rotate(0f, 0f, 180f, Space.Self); //Intercambia el giroscopio 
	   VRCamaras.transform.Rotate(90f, 180f, 0f, Space.World); // para que gire y la camara apunte en la parte posterior del dispositivo
	   PosicionDelGyroEnY = VRCamaras.transform.eulerAngles.y; // Guarda el angulo al redor del eje para el uso en la calibración
	}
	
	void CalibrarEnPosicionY()
	{
	  CalibrarEnLaPosicionY = PosicionDelGyroEnY - PosicionInicialY;
	}
	
	void AplicarCalibracion()
	{
	   VRCamaras.transform.Rotate(0f, -CalibrarEnLaPosicionY, 0f, Space.World);
	}
}