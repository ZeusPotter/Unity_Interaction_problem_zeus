using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_Menu : MonoBehaviour
{    public void SeleccionarPersonaje(int personajeID)
    {
        PlayerPrefs.SetInt("PersonajeSeleccionado", personajeID);
        PlayerPrefs.Save(); // Guarda el valor en memoria
        SceneManager.LoadScene(1); // Carga la escena del juego
    }
}
