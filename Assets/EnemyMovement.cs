using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    public enum Variation { Vertical, Horizontal, Idle }

    [Header("Behavior")]
    [Tooltip("Pick how this enemy moves.")]
    public Variation variation = Variation.Vertical;

    [Header("Start Direction")]
    [Tooltip("Horizontal: if checked, move Right first; otherwise Left.")]
    public bool startHorizontalPositive = true;
    [Tooltip("Vertical: if checked, move Up first; otherwise Down.")]
    public bool startVerticalPositive = true;

    [Header("Movement Settings")]
    [Tooltip("Units per second")]
    public float speed = 2f;
    [Tooltip("Seconds between reversing (V/H) or rotating idle facing")]
    public float directionChangeInterval = 3f;

    [Header("Ambient One-Shot Audio")]
    [Tooltip("First ambient clip")]
    public AudioClip clip1;
    [Tooltip("Seconds between plays of clip1")]
    public float clip1Interval = 5f;
    [Range(0f, 1f)]
    [Tooltip("Volume for clip1")]
    public float clip1Volume = 1f;

    [Tooltip("Second ambient clip")]
    public AudioClip clip2;
    [Tooltip("Seconds between plays of clip2")]
    public float clip2Interval = 8f;
    [Range(0f, 1f)]
    [Tooltip("Volume for clip2")]
    public float clip2Volume = 1f;

    [Tooltip("Max distance at which one-shot audio will play")]
    public float audioProximityRadius = 10f;

    [Header("Looping Audio")]
    [Tooltip("This clip will loop continuously when player is near")]
    public AudioClip loopClip;
    [Range(0f, 1f)]
    [Tooltip("Volume for looping clip")]
    public float loopVolume = 1f;
    [Tooltip("Max distance at which looping audio will play")]
    public float loopProximityRadius = 10f;

    // movement internals
    private float directionTimer;
    private Vector2 currentDir;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    // audio internals
    private AudioSource audioSource;  // for one-shots
    private AudioSource loopSource;   // for looping clip
    private Transform playerTransform;
    private float clip1Timer;
    private float clip2Timer;
    private bool audioBusy;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // One-shot AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Looping AudioSource
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.clip = loopClip;
        loopSource.loop = true;
        loopSource.playOnAwake = false;
        loopSource.volume = loopVolume;

        // find player by tag
        playerTransform = GameObject.FindWithTag("Player")?.transform;
    }

    void Start()
    {
        directionTimer = directionChangeInterval;
        clip1Timer = clip1Interval;
        clip2Timer = clip2Interval;

        // initialize facing based on your start‐direction toggles
        switch (variation)
        {
            case Variation.Vertical:
                currentDir = startVerticalPositive ? Vector2.up : Vector2.down;
                break;

            case Variation.Horizontal:
                currentDir = startHorizontalPositive ? Vector2.right : Vector2.left;
                break;

            default: // Idle
                currentDir = Vector2.up;
                break;
        }
    }

    void Update()
    {
        // —— Movement logic —— 
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f)
        {
            switch (variation)
            {
                case Variation.Vertical:
                    currentDir.y = -currentDir.y;
                    break;

                case Variation.Horizontal:
                    currentDir.x = -currentDir.x;
                    break;

                case Variation.Idle:
                    // cycle Up→Right→Down→Left
                    if (currentDir == Vector2.up) currentDir = Vector2.right;
                    else if (currentDir == Vector2.right) currentDir = Vector2.down;
                    else if (currentDir == Vector2.down) currentDir = Vector2.left;
                    else currentDir = Vector2.up;
                    break;
            }
            directionTimer = directionChangeInterval;
        }

        // apply movement & feed animator
        switch (variation)
        {
            case Variation.Vertical:
                movement = new Vector2(0, currentDir.y);
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", currentDir.y);
                break;

            case Variation.Horizontal:
                movement = new Vector2(currentDir.x, 0);
                animator.SetBool("IsMoving", true);
                animator.SetFloat("MoveX", currentDir.x);
                animator.SetFloat("MoveY", 0);
                break;

            default: // Idle
                movement = Vector2.zero;
                animator.SetBool("IsMoving", false);
                animator.SetFloat("MoveX", currentDir.x);
                animator.SetFloat("MoveY", currentDir.y);
                break;
        }
        // —— end Movement logic ——


        // —— Proximity & Audio logic —— 
        if (playerTransform == null) return;
        float dist = Vector2.Distance(transform.position, playerTransform.position);

        // One-shot ambience
        if (dist <= audioProximityRadius)
            HandleAmbientAudio();
        else
        {
            clip1Timer = clip1Interval;
            clip2Timer = clip2Interval;
        }

        // Looping audio on/off
        if (loopClip != null)
        {
            loopSource.volume = loopVolume;
            if (dist <= loopProximityRadius)
            {
                if (!loopSource.isPlaying)
                    loopSource.Play();
            }
            else if (loopSource.isPlaying)
            {
                loopSource.Stop();
            }
        }
    }

    private void HandleAmbientAudio()
    {
        // Clip 1
        clip1Timer -= Time.deltaTime;
        if (clip1Timer <= 0f && !audioBusy && clip1 != null)
        {
            audioSource.volume = clip1Volume;
            audioSource.clip = clip1;
            audioSource.Play();
            audioBusy = true;
            StartCoroutine(ResetAudioBusy(clip1.length));
            clip1Timer = clip1Interval;
        }

        // Clip 2
        clip2Timer -= Time.deltaTime;
        if (clip2Timer <= 0f && !audioBusy && clip2 != null)
        {
            audioSource.volume = clip2Volume;
            audioSource.clip = clip2;
            audioSource.Play();
            audioBusy = true;
            StartCoroutine(ResetAudioBusy(clip2.length));
            clip2Timer = clip2Interval;
        }
    }

    private IEnumerator ResetAudioBusy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioBusy = false;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
