using UnityEngine;
using UnityEngine.UI;

public class RaycastInteraction : MonoBehaviour
{
    public GameObject interactionButton; // Asigna el botón desde el Inspector
    public float rayDistance = 5f; // Distancia máxima del raycast
    public LayerMask interactableLayer; // Capa de objetos interactuables

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            interactionButton.SetActive(true);

            // Opcional: Detectar clic en el botón
            if (Input.GetKeyDown(KeyCode.E)) // Suponiendo que la tecla "E" es la de interacción
            {
                // Lógica de interacción con el objeto
                Debug.Log("Interaccionando con: " + hit.collider.gameObject.name);
                // Aquí puedes llamar a un método en el objeto impactado, por ejemplo:
                // hit.collider.gameObject.GetComponent<NombreDelScript>().MetodoDeInteraccion();
            }
        }
        else
        {
            interactionButton.SetActive(false);
        }
    }
}
