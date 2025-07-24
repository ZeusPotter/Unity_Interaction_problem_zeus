using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas; // Arrastra aquí el Canvas del menú de pausa
    private bool isPaused = false;

    void Start()
    {
        // Asegurar que el menú esté oculto al inicio
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f; // Garantiza que el tiempo inicie normal
    }

    void Update()
    {
        // Activar/desactivar pausa con Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseCanvas.SetActive(true); // Mostrar el menú
        Time.timeScale = 0f; // Pausar el tiempo del juego
        Cursor.lockState = CursorLockMode.None; // Liberar el cursor
        Cursor.visible = true; // Hacer visible el cursor
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false); // Ocultar el menú
        Time.timeScale = 1f; // Restaurar el tiempo del juego
        Cursor.lockState = CursorLockMode.Locked; // Ocultar y bloquear el cursor
        Cursor.visible = false;
    }
}
