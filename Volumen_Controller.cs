using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volumen_Controller : MonoBehaviour
{
    public AudioMixer audioMixer; // Arrastra aquí el MainAudioMixer
    public Slider volumenSlider;

    void Start()
    {
        volumenSlider.value = PlayerPrefs.GetFloat("Volumen", 0.75f);
        CambiarVolumen(volumenSlider.value);
    }

    public void CambiarVolumen(float volumen)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volumen) * 20); // Convierte lineal a logarítmico
        PlayerPrefs.SetFloat("Volumen", volumen);
    }
}
