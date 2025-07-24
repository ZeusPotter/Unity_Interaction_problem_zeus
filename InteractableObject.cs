using UnityEngine;

public class InteractableObject : MonoBehaviour {
    public DialogueTrigger dialogueTrigger;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            dialogueTrigger.TriggerDialogue();
        }
    }
}
