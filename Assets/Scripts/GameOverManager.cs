using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI")]
    [Tooltip("The panel that appears on Game Over")]
    public GameObject gameOverPanel;

    // hidden death counter
    int deathCount = 0;
    /// <summary>
    /// How many times the player has died and retried this level.
    /// </summary>
    public int DeathCount => deathCount;

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
        }
    }

    /// <summary>
    /// Call this when the player dies to show the Game Over UI.
    /// </summary>
    public void ShowGameOver()
    {
        deathCount++;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Retry the current level (deathCount persists).
    /// </summary>
    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Return to Main Menu (reset deathCount for a fresh playthrough).
    /// </summary>
    public void OnMainMenu()
    {
        // reset the counter when heading back to menu
        deathCount = 0;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Ensure we do not carry deathCount across full application quits.
    /// </summary>
    void OnApplicationQuit()
    {
        deathCount = 0;
    }
}
