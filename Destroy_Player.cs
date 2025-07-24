using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para usar SceneManager

public class DestroyPlayer : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        // Buscar el jugador en la escena por su nombre (ajusta "Player" por el nombre real del objeto)
        player = GameObject.Find("Player");
    }

    public void DestroyPlayerObject()
    {
        if (player != null)
        {
            Destroy(player);  // Destruye el jugador
            SceneManager.LoadScene("Menu");  // Carga la escena del menú
        }
    }
}
