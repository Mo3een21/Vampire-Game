using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Tooltip("The panel that holds your pause menu UI")]
    public GameObject pausePanel;

    bool isPaused = false;

    void Awake()
    {
        // singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // toggle on Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;  // freeze all physics & animations
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;  // unfreeze
        isPaused = false;
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;  // make sure time is normal before switching
        SceneManager.LoadScene("Main Menu");
    }
}
