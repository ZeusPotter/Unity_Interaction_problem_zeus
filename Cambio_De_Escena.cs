using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambio_De_Escena : MonoBehaviour
{
    // Método que se llama desde el OnClick() de tu botón
    public void LoadSceneByName(string EscenarioPrincipal)
    {
        SceneManager.LoadScene(0);
    }
}