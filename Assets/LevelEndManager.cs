using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndManager : MonoBehaviour
{
    public static LevelEndManager Instance { get; private set; }

    [Header("Player Win Animation")]
    [Tooltip("Tag of your Player GameObject")]
    public string playerTag = "Player";
    [Tooltip("Name of the Trigger in your Player Animator")]
    public string winTrigger = "win";

    [Header("Laugh SFX")]
    [Tooltip("Laugh sound to play when the Win animation starts")]
    public AudioClip laughClip;
    [Range(0f, 1f)]
    [Tooltip("Volume of the laugh SFX")]
    public float laughVolume = 1f;

    [Header("Results Screen")]
    [Tooltip("Drag your ResultsPanel GameObject here")]
    public GameObject resultsPanel;

    [Header("UI to Hide on Level End")]
    [Tooltip("Drag all HUD, timer, slider, etc. GameObjects here (but NOT the ResultsPanel)")]
    public GameObject[] uiElementsToHide;

    int totalStatues;
    int reachedCount;
    AudioSource audioSource;

    void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        // count all endpoints
        totalStatues = FindObjectsOfType<LevelEndPoint>().Length;
        reachedCount = 0;
        if (totalStatues == 0)
            Debug.LogWarning("LevelEndManager: No LevelEndPoint found in scene!");

        // ensure results are hidden initially
        if (resultsPanel != null)
            resultsPanel.SetActive(false);
    }

    /// <summary>
    /// Called by each statue when the Player triggers it.
    /// </summary>
    public void StatueReached()
    {
        reachedCount++;
        if (reachedCount >= totalStatues)
            OnLevelComplete();
    }

    void OnLevelComplete()
    {
        // 1) Hide all other UI (but leave ResultsPanel alone)
        foreach (var ui in uiElementsToHide)
            if (ui != null && ui != resultsPanel)
                ui.SetActive(false);

        // 2) Stop player movement & play Win animation
        var player = GameObject.FindWithTag(playerTag);
        if (player != null)
        {
            var pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = false;

            var anim = player.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger(winTrigger);
        }

        // 3) Play laugh SFX immediately
        if (laughClip != null)
            audioSource.PlayOneShot(laughClip, laughVolume);

        // 4) Show results panel last
        if (resultsPanel != null)
            resultsPanel.SetActive(true);
        else
            Debug.LogWarning("LevelEndManager: ResultsPanel reference is missing!");
    }

    /// <summary>
    /// Called by the "LEVEL II" button.
    /// </summary>
    public void OnNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelTwo");
    }

    /// <summary>
    /// Called by the "Main Menu" button.
    /// </summary>
    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
