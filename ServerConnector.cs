using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class FeedbackResponse
{
    public string feedback;
    public string frase_ideal;
    public string frase_usuario;
    public string romaji_ref;
    public string romaji_usuario;
    public float similitud;
}


public class ServerConnector : MonoBehaviour
{
    public IEnumerator SendAudioAndText(byte[] wavBytes, string japaneseText, string romajiText)
{
    string serverUrl = "https://e794287218e7.ngrok-free.app/upload_audio";

    WWWForm form = new WWWForm();
    form.AddField("jp", japaneseText);               // Línea japonesa
    form.AddField("romaji", romajiText);             // Línea romanizada
    form.AddBinaryData("file", wavBytes, "recorded.wav", "audio/wav");

    Debug.Log($"Enviando al servidor:\nTexto: {japaneseText}\nRomaji: {romajiText}\nTamaño del audio: {wavBytes.Length}");

    UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
yield return www.SendWebRequest();

if (www.result != UnityWebRequest.Result.Success)
{
    Debug.LogError("Error enviando datos: " + www.error);
    Debug.LogError("Código de respuesta: " + www.responseCode);
    Debug.LogError("Respuesta: " + www.downloadHandler.text);  // ⬅️ Aquí
}
else
{
    Debug.Log("Datos enviados correctamente");
    Debug.Log("Respuesta: " + www.downloadHandler.text);

    string json = www.downloadHandler.text;
    

    // Usar JsonUtility no sirve aquí por los nombres unicode — usar SimpleJSON o JSON manual:
            try
            {
                var response = JsonUtility.FromJson<FeedbackResponse>(json);
                DialogueManager.Instance.OnServerFeedbackReceived(response.similitud);
            }
            catch
            {
                Debug.LogWarning("No se pudo parsear la respuesta correctamente.");
            }
}
}

}