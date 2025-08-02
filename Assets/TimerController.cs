using UnityEngine;
using TMPro;  // TextMeshPro namespace

public class TimerController : MonoBehaviour
{
    [Tooltip("Drag your TimerText (TMP) here")]
    public TMP_Text timerText;

    float elapsed = 0f;
    public float ElapsedTime => elapsed;

    void Update()
    {
        // 1) accumulate time
        elapsed += Time.deltaTime;

        // 2) break into minutes, seconds, hundredths
        int minutes = (int)(elapsed / 60f);
        int seconds = (int)(elapsed % 60f);
        int hundredths = (int)((elapsed * 100f) % 100f);

        // 3) format as MM:SS:FF
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}",
                                       minutes, seconds, hundredths);
    }

    /// <summary>Call this to reset the clock to zero.</summary>
    public void ResetTimer()
    {
        elapsed = 0f;
    }
}
