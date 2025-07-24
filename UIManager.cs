using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject interactPrompt; // Asigna el Text en el Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowPrompt(bool state)
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(state);
    }
}
