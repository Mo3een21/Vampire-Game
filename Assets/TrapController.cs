using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class TrapController : MonoBehaviour
{
    [Header("Death Settings")]
    [Tooltip("Trigger name on the Player Animator")]
    public string deathTrigger = "Die";
    [Tooltip("Length of your death animation in seconds")]
    public float deathAnimDuration = 1f;

    [Header("Proximity Audio")]
    [Tooltip("Clip to play when the player is near the trap")]
    public AudioClip proximityClip;
    [Range(0f, 1f)]
    [Tooltip("Volume of the proximity SFX")]
    public float proximityVolume = 1f;
    [Tooltip("How close the player must be to hear the SFX")]
    public float proximityRadius = 5f;
    [Tooltip("Seconds between each SFX play while in range")]
    public float proximityInterval = 2f;

    Collider2D _collider;
    bool _triggered = false;

    // audio
    AudioSource _audioSource;
    float _proximityTimer;
    Transform _player;

    void Awake()
    {
        // collider setup
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _collider.enabled = false;  // harmless until spikes up

        // audio setup
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        _proximityTimer = proximityInterval;
        _player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        // proximity SFX
        if (!_triggered && proximityClip != null && _player != null)
        {
            float dist = Vector2.Distance(transform.position, _player.position);
            if (dist <= proximityRadius)
            {
                _proximityTimer -= Time.deltaTime;
                if (_proximityTimer <= 0f)
                {
                    _audioSource.volume = proximityVolume;
                    _audioSource.PlayOneShot(proximityClip);
                    _proximityTimer = proximityInterval;
                }
            }
            else
            {
                // reset timer when out of range
                _proximityTimer = proximityInterval;
            }
        }
    }

    /// <summary>
    /// Called by Animation Event when spikes are fully up.
    /// </summary>
    public void EnableTrap()
    {
        _collider.enabled = true;
    }

    /// <summary>
    /// Called by Animation Event when spikes retract.
    /// </summary>
    public void DisableTrap()
    {
        _collider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        _triggered = true;

        // disable player movement
        var pm = other.GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        // trigger death animation
        var anim = other.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger(deathTrigger);

        // show Game Over after death anim
        StartCoroutine(DelayedGameOver());
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(deathAnimDuration);
        GameOverManager.Instance?.ShowGameOver();
    }
}
