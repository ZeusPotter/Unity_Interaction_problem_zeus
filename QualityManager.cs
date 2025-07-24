using UnityEngine;

public class QualityManager : MonoBehaviour
{
    public void SetQualityLow()
    {
        QualitySettings.SetQualityLevel(0, true); // Bajo
    }

    public void SetQualityMedium()
    {
        QualitySettings.SetQualityLevel(1, true); // Medio
    }

    public void SetQualityHigh()
    {
        QualitySettings.SetQualityLevel(2, true); // Alto
    }
}
