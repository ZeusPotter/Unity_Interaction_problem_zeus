using UnityEngine;
using UnityEngine.UI;

public class ProximityDetector : MonoBehaviour
{
    public GameObject interactionButton; // Asigna el botón desde el Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionButton.SetActive(true); // Muestra el botón
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionButton.SetActive(false); // Oculta el botón
        }
    }
}
