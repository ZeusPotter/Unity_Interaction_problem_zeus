using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra es el jugador
        {
            SceneManager.LoadScene(1); // Usa el nombre real de la escena
        }
    }
}