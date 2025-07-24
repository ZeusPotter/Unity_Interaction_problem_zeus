using UnityEngine;
using MyGame.Dialogues;
using DialogueSystem;

public class DialogueTrigger : MonoBehaviour
{
    public string interactionID;
    private static DialogueScene dialogueScene;

    private void Awake()
    {
        if (dialogueScene == null)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("dialogues");
            if (jsonFile != null)
                dialogueScene = JsonUtility.FromJson<DialogueScene>(jsonFile.text);
            else
                Debug.LogError("dialogues.json no encontrado en Resources.");
        }
    }

    public void TriggerDialogue()
    {
        if (DialogueManager.Instance.IsDialoguePlaying()) return;

        var interaction = dialogueScene.interactions.Find(i => i.id == interactionID);
        if (interaction != null)
        {
            // Use the interaction ID as the NPC name since npcName is not in the JSON
            string npcName = interaction.npcName;
            if (string.IsNullOrEmpty(npcName))
            {
                npcName = interaction.id; // Fallback to using the ID as the NPC name
            }
            Debug.Log("NPC Name: " + npcName);
            DialogueManager.Instance.StartDialogue(interaction.lines, npcName);
        }
        else
        {
            Debug.LogWarning("No se encontró el diálogo con ID: " + interactionID);
        }
    }
}
