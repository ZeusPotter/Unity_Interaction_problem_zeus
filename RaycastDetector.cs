using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    public float rayDistance = 20f;
    public LayerMask interactLayer;

    private bool isLookingAtInteractable = false;
    private DialogueTrigger currentTrigger;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer))
        {
            DialogueTrigger trigger = hit.collider.GetComponent<DialogueTrigger>();
            if (trigger != null)
            {
                if (!isLookingAtInteractable || currentTrigger != trigger)
                {
                    UIManager.Instance.ShowPrompt(true); // Mostrar el mensaje
                    isLookingAtInteractable = true;
                    currentTrigger = trigger;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    trigger.TriggerDialogue(); // Iniciar el diálogo desde el trigger
                }
            }
        }
        else
        {
            if (isLookingAtInteractable)
            {
                UIManager.Instance.ShowPrompt(false); // Ocultar el mensaje
                isLookingAtInteractable = false;
                currentTrigger = null;
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
    }
}
