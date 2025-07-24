using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject VR_Player_C_Cascos; // Prefab del personaje 1
    public GameObject VR_Player_S_Cascos; // Prefab del personaje 2
    public GameObject PlayerPC;  // Prefab del personaje 3
    public GameObject PlayerPhone; // Prefab del personaje 4
    public Camera mainCamera; // Referencia a la cámara principal

    private static GameObject currentPlayer; // El jugador persistente

    void Awake()
    {
        // Si el jugador ya existe, aseguramos que se mantenga entre escenas
        if (currentPlayer != null)
        {
            // Si estamos en el menú, destruye el jugador
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                Destroy(currentPlayer);  // Destruye al jugador al regresar al menú
                if (mainCamera != null)
                {
                    mainCamera.gameObject.SetActive(false); // Desactiva la cámara en el menú
                }
            }
            else
            {
                // De lo contrario, activamos el jugador y la cámara en la escena
                currentPlayer.SetActive(true);
                if (mainCamera != null)
                {
                    mainCamera.gameObject.SetActive(true); // Activa la cámara
                }
                MovePlayerToSpawn();
            }
            return;
        }

        // Recuperamos el jugador seleccionado desde PlayerPrefs
        int personajeSeleccionado = PlayerPrefs.GetInt("PersonajeSeleccionado", 1);
        GameObject playerToSpawn = null;

        // Seleccionamos el prefab del jugador según la elección
        switch (personajeSeleccionado)
        {
            case 1: playerToSpawn = VR_Player_C_Cascos; break;
            case 2: playerToSpawn = VR_Player_S_Cascos; break;
            case 3: playerToSpawn = PlayerPC; break;
            case 4: playerToSpawn = PlayerPhone; break;
        }

        // Instanciamos el jugador
        if (playerToSpawn != null)
        {
            currentPlayer = Instantiate(playerToSpawn);  // Instancia el jugador
            DontDestroyOnLoad(currentPlayer); // El jugador no se destruye al cambiar de escena
        }

        // Escuchar cuando se haya cargado la escena para mover al jugador
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            // Si estamos en el menú, destruye el jugador
            if (currentPlayer != null)
            {
                Destroy(currentPlayer);  // Destruye al jugador
            }
            if (mainCamera != null)
            {
                mainCamera.gameObject.SetActive(false); // Desactiva la cámara en el menú
            }
        }
        else
        {
            // Mueve al jugador a su punto de inicio si no estamos en el menú
            if (currentPlayer != null)
            {
                currentPlayer.SetActive(true); // Reactiva al jugador
            }
            if (mainCamera != null)
            {
                mainCamera.gameObject.SetActive(true); // Reactiva la cámara
            }
            MovePlayerToSpawn();
        }
    }

    void MovePlayerToSpawn()
    {
        // Asumimos que hay un objeto "SpawnPoint" en la escena
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null && currentPlayer != null)
        {
            currentPlayer.transform.position = spawnPoint.transform.position;  // Mueve al jugador al punto de spawn
        }
    }
}
