using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class VoiceValidator : MonoBehaviour
{
    public string serverUrl = "http://localhost:5005/validate";
    public TMP_Text resultText;

    public void EnviarAudio(byte[] wavData, string expectedJapanese)
    {
        StartCoroutine(Enviar(wavData, expectedJapanese));
    }

    IEnumerator Enviar(byte[] audioData, string targetText)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", audioData, "voz.wav", "audio/wav");
        form.AddField("target", targetText);

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Error: " + www.error);
                resultText.text = "❌ Error: " + www.error;
            }
            else
            {
                var json = www.downloadHandler.text;
                var result = JsonUtility.FromJson<Respuesta>(json);
                if (result.ok)
                {
                    resultText.text = $"🗣️ {result.dicho}\n🔤 {result.romaji}\n📊 Similitud: {result.similitud * 100f:F2}%\n📝 {result.feedback}";

                    if (result.similitud >= 0.8f)
                    {
                        Debug.Log("✅ Aprobado");
                        // Activar animación, avanzar diálogo, etc.
                    }
                    else
                    {
                        Debug.Log("⚠️ Intenta de nuevo");
                    }
                }
                else
                {
                    Debug.LogError("❌ Servidor falló: " + result.error);
                }
            }
        }
    }

    [System.Serializable]
    public class Respuesta
    {
        public bool ok;
        public string dicho;
        public string romaji;
        public float similitud;
        public string feedback;
        public string error;
    }
}
