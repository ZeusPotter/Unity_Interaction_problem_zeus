using UnityEngine;
using UnityEngine.UI; // Para trabajar con botones y UI

public class ExitGameManager : MonoBehaviour
{
    public GameObject confirmacionPanel; // Arrastra el panel de confirmación
    public Button salirButton; // Arrastra el botón de salir
    public Button confirmarButton; // Arrastra el botón de confirmar
    public Button cancelarButton; // Arrastra el botón de cancelar

    void Start()
    {
        // Asignar eventos a los botones
        salirButton.onClick.AddListener(MostrarConfirmacion);
        confirmarButton.onClick.AddListener(SalirJuego);
        cancelarButton.onClick.AddListener(CerrarConfirmacion);
    }

    // Muestra el panel de confirmación
    void MostrarConfirmacion()
    {
        confirmacionPanel.SetActive(true); // Activa el panel de confirmación
    }

    // Cierra la aplicación
    void SalirJuego()
    {
        // Si está en el Editor de Unity, termina la ejecución
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // Cierra la aplicación en build
        #endif
    }

    // Cierra el panel de confirmación
    void CerrarConfirmacion()
    {
        confirmacionPanel.SetActive(false); // Desactiva el panel de confirmación
    }
}

