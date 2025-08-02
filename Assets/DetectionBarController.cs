using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DetectionBarController : MonoBehaviour
{
    [Header("UI")]
    public Slider detectionSlider;

    [Header("Detection Settings")]
    public float maxDetection = 100f;
    public float detectionRate = 20f;
    public float decayRate = 10f;

    [Header("Death")]
    [Tooltip("Tag of your Player GameObject")]
    public string playerTag = "Player";
    [Tooltip("Trigger parameter on your Player Animator")]
    public string deathTrigger = "Die";

    float currentDetection = 0f;
    bool hasDied = false;

    void Update()
    {
        // --- existing fill/drain logic ---
        bool seen = FindObjectsOfType<FieldOfView2D>()
                    .Any(fov => fov.visibleTargets.Count > 0);

        currentDetection += (seen ? detectionRate : -decayRate) * Time.deltaTime;
        currentDetection = Mathf.Clamp(currentDetection, 0f, maxDetection);

        if (detectionSlider != null)
        {
            detectionSlider.maxValue = maxDetection;
            detectionSlider.value = currentDetection;
        }

        // --- new death check ---
        if (!hasDied && currentDetection >= maxDetection)
        {
            hasDied = true;
            var player = GameObject.FindWithTag(playerTag);
            if (player != null)
            {
                var pm = player.GetComponent<PlayerMovement>();
                if (pm != null)
                    pm.Die();
            }
        }
    }
}
