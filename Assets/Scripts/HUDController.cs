using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Tooltip("Drag your Keys & Score Text (TMP) here")]
    public TextMeshProUGUI keysText;

    int keyCount = 0;
    int score = 0;

    /// <summary>
    /// Current score (for your results screen to read).
    /// </summary>
    public int Score => score;

    /// <summary>
    /// Current key count (if you ever need it).
    /// </summary>
    public int KeyCount => keyCount;

    void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// Call this whenever your key‐pickup code changes the key count.
    /// </summary>
    public void UpdateKeys(int newKeyCount)
    {
        keyCount = newKeyCount;
        UpdateUI();
    }

    /// <summary>
    /// Call this to add points to the player’s score.
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        // Show both values on one line
        keysText.text = $"Keys: {keyCount}   Score: {score}";
    }
}
