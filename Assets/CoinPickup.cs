using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinPickup : MonoBehaviour
{
    [Header("Coin Settings")]
    public int points = 100;
    public AudioClip collectSound;                    // ← assign your SFX here
    public string successMessage = "You acquired 100 points!";
    public float clipVolume = 1f;
    // internal state
    bool collected = false;
    Collider2D triggerCol;
    KeyTextController uiText;
    HUDController hud;
    AudioSource audioSource;

    void Awake()
    {
        triggerCol = GetComponent<Collider2D>();
        uiText = FindObjectOfType<KeyTextController>();
        hud = FindObjectOfType<HUDController>();

        // add or get an AudioSource for the collect SFX
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        if (Input.GetKeyDown(KeyCode.E))
            TryCollect();
    }

    void TryCollect()
    {
        collected = true;

        // create a temporary, independent AudioSource and fire the clip
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position, clipVolume);

        // score + UI
        hud?.AddScore(points);
        uiText?.ShowMessage(successMessage);

        // immediately remove the coin
        Destroy(gameObject);
    }
}
