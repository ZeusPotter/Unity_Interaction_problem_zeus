using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Networking;
using System.Collections;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI englishText;
    public TextMeshProUGUI japaneseText;
    public TextMeshProUGUI romajiText;

    public TextMeshProUGUI micStatusText; 

    [Header("Audio")]
    public AudioSource audioSource;

    public Player_Controller_PC playerController; // Asignar desde el Inspector

    public TextMeshProUGUI serverFeedbackText;

    public BoxCollider blockingCollider;

    public string currentNPC = "ticket_machine"; // O la forma que identifiques con quién hablas




    private Queue<DialogueLine> lines;
    private bool isPlaying = false;
    private string lastAudioClipName;
    private bool isMicrophoneOn = false;
    private AudioClip micClip;
    private DialogueLine currentLine;
    private float lastSimilarity = 0f;
private bool canAdvance = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        lines = new Queue<DialogueLine>();
        dialogueCanvas.SetActive(false);

        if (audioSource == null)
        {
            Debug.LogError("AudioSource no está asignado en el inspector.");
        }
    }

    void Update()
    {
        if (!isPlaying) return;

        // Avanzar diálogo con R
        if (Input.GetKeyDown(KeyCode.R))
            {
                if (canAdvance || currentLine.speaker != "You:")
                {
                    DisplayNextLine();
                    canAdvance = false;  // Resetea para la próxima línea del jugador
                    lastSimilarity = 0f;
                }
                else
                {
                    Debug.Log("Debes pronunciar correctamente la frase antes de avanzar.");
                }
            }


        // Repetir audio con X
        if (Input.GetKeyDown(KeyCode.X))
        {
            RepeatAudio();
        }

        // Toggle micrófono con C
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentLine != null && currentLine.speaker.Contains("You:"))
            {
                ToggleMicrophone();
            }
            else
            {
                Debug.Log("Micrófono sólo activo para líneas del jugador.");
            }
        }
    }

    public void OnServerFeedbackReceived(float similarity)
{
    lastSimilarity = similarity;
    Debug.Log("✅ Similitud recibida: " + similarity);

    if (serverFeedbackText != null)
    {
        if (similarity >= 0.8f)
        {
            serverFeedbackText.text = $"✅ Bien dicho ({similarity * 100f:F1}%) ¡Puedes avanzar con R!";
            serverFeedbackText.color = Color.green;
        }
        else
        {
            serverFeedbackText.text = $"❌ Inténtalo de nuevo ({similarity * 100f:F1}%)";
            serverFeedbackText.color = Color.red;
        }
    }

    if (similarity >= 0.8f)
    {
        canAdvance = true;
        Debug.Log("🎉 ¡Puedes avanzar al siguiente diálogo con R!");
    }
    else
    {
        Debug.Log("❌ Similitud insuficiente. Intenta nuevamente.");
    }
}



   public void StartDialogue(List<DialogueLine> dialogueLines, string npcName)
{
    isPlaying = true;
    lines.Clear();

    foreach (var line in dialogueLines)
        lines.Enqueue(line);

    dialogueCanvas.SetActive(true);
    DisplayNextLine();

    if (playerController != null)
        playerController.canMove = false;

    currentNPC = npcName;
}

    public void EndDialogue()
    {
        isPlaying = false;
        dialogueCanvas.SetActive(false);
        StopMicrophone();
        audioSource.Stop();

        // Reanudar movimiento
        if (playerController != null)
            playerController.canMove = true;
         if (currentNPC == "TicketMachine")
    {
        EnableNextStep();
    }
}

void EnableNextStep()
{
    if (blockingCollider != null)
    {
        blockingCollider.enabled = false; // Quita el bloqueo
        Debug.Log("Camino desbloqueado para continuar la historia.");
    }
}

    void PlayAudio(string audioName)
{
    if (string.IsNullOrEmpty(audioName))
    {
        Debug.LogWarning("⚠️ audioName está vacío o nulo.");
        return;
    }

    Debug.Log("🔊 Intentando reproducir audio: " + audioName);

    AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioName);
    if (clip != null)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
        lastAudioClipName = audioName;
    }
    else
    {
        Debug.LogWarning("❌ Audio clip no encontrado: " + audioName);
    }
}

    public void RepeatAudio()
    {
        if (!string.IsNullOrEmpty(lastAudioClipName))
        {
            PlayAudio(lastAudioClipName);
        }
    }

    public void DisplayNextLine()
{
    if (lines.Count == 0)
    {
        EndDialogue();
        return;
    }

    DialogueLine line = lines.Dequeue();

    currentLine = line;

    speakerText.text = line.speaker;
    englishText.text = line.en;
    japaneseText.text = line.jp;
    romajiText.text = line.romaji;
    lastAudioClipName = line.audio;

    // Reproduce audio incluso si el hablante es "You:"
    PlayAudio(lastAudioClipName);
}
    void ToggleMicrophone()
{
    if (!isMicrophoneOn)
    {
        StartMicrophone();
    }
    else
    {
        StopMicrophone();

        Debug.Log("Procesar audio grabado para reconocimiento...");

        if (micClip != null && currentLine != null)
        {
            byte[] wavData = WavUtility.FromAudioClip(micClip);
            StartCoroutine(FindObjectOfType<ServerConnector>().SendAudioAndText(wavData, currentLine.jp, currentLine.romaji));

        }
        else
        {
            Debug.LogWarning("No hay audio grabado o línea actual para enviar.");
        }
    }
}


   void StartMicrophone()
{
    if (Microphone.devices.Length > 0)
    {
        string deviceName = Microphone.devices[0];
        micClip = Microphone.Start(deviceName, false, 10, 44100);
        isMicrophoneOn = true;
        Debug.Log("🎤 Micrófono activado.");
        micStatusText.text = "Micrófono: ON";
        micStatusText.color = Color.green;
    }
    else
    {
        Debug.LogWarning("⚠️ No hay micrófonos disponibles.");
        micStatusText.text = "Micrófono: NO DISPONIBLE";
        micStatusText.color = Color.red;
    }
}
    IEnumerator UploadAudioClip(AudioClip clip, string url)
{
    byte[] wavData = WavUtility.FromAudioClip(clip);
    WWWForm form = new WWWForm();
    form.AddBinaryData("file", wavData, "recording.wav", "audio/wav");

    using (UnityWebRequest www = UnityWebRequest.Post(url, form))
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error al enviar audio: " + www.error);
        }
        else
        {
            Debug.Log("Respuesta del servidor: " + www.downloadHandler.text);
            // Aquí puedes procesar la respuesta para mostrar feedback en UI o consola
        }
    }
}

   void StopMicrophone()
{
        if (isMicrophoneOn)
        {
            Microphone.End(null);
            isMicrophoneOn = false;
            Debug.Log("🎤 Micrófono desactivado.");
            micStatusText.text = "Micrófono: OFF";
            micStatusText.color = Color.gray;
byte[] wavData = WavUtility.FromAudioClip(micClip);
StartCoroutine(FindObjectOfType<ServerConnector>().SendAudioAndText(wavData, currentLine.jp, currentLine.romaji));


    }
}
    public static class JsonUtilityWrapper
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}");
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}

    public bool IsDialoguePlaying() => isPlaying;
}


