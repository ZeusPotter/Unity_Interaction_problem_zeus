using UnityEngine;
using UnityEngine.UI;

public class PauseMenuTouch : MonoBehaviour
{
    public GameObject pauseCanvas; // Arrastra el Canvas del menú de pausa
    public Button pauseButton; // Botón de pausa
    public Button resumeButton; // Botón de reanudar
    private bool isPaused = false;

    void Start()
    {
        // Asegurar que el menú de pausa inicie oculto
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;

        // Asignar funciones a los botones
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    void Update()
    {
        // Permitir pausar con la tecla Escape
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
        pauseCanvas.SetActive(true); // Mostrar menú de pausa
        pauseButton.gameObject.SetActive(false); // Ocultar botón de pausa
        Time.timeScale = 0f; // Pausar el juego
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false); // Ocultar menú de pausa
        pauseButton.gameObject.SetActive(true); // Mostrar botón de pausa nuevamente
        Time.timeScale = 1f; // Reanudar el juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
