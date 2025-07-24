using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public int menuSceneIndex = 0; // Índice de la escena del menú en Build Settings

    public GameObject player; // Arrastra aquí el GameObject del jugador en el Inspector

    public void GoToMainMenu()
    {
        // Si el jugador existe, lo desactivamos antes de cambiar de escena
        if (player != null)
        {
            player.SetActive(false); // Desactivar el jugador
            Destroy(player); // Destruye el jugador
        }

        Time.timeScale = 1f; // Asegurar que el tiempo esté activo
        SceneManager.LoadScene(menuSceneIndex); // Cargar la escena del menú usando el índice
    }
}
