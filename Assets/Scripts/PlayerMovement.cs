using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Movement")]
    [SerializeField] private int speed = 5;

    [Header("Footsteps")]
    [Tooltip("Clips to play for each footstep")]
    public AudioClip[] footstepClips;
    [Tooltip("Seconds between steps when moving")]
    public float stepInterval = 0.4f;

    private float stepTimer = 0f;
    bool isDead = false;
    [Header("Death SFX")]
    [Tooltip("One‑shot sound when the player dies")]
    public AudioClip deathClip;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();

        bool isWalking = movement.x != 0 || movement.y != 0;
        animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
        }
    }

    private void Update()
    {
        // Handle footstep timing
        if (animator.GetBool("IsWalking"))
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset so first step plays immediately when you start again
            stepTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;    // don�t move if we�re dead
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

    }

    private void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;
        // Randomize clip & pitch if you like
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clip);
    }
    // call this to kill the player
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        // 1) play death SFX (one‑shot so it doesn’t interrupt looping audio, if any)
        if (deathClip != null)
            audioSource.PlayOneShot(deathClip);
        // 1) trigger the death animation
        animator.SetTrigger("Die");

        // 2) disable movement & input
        enabled = false;          // turns off this MonoBehaviour

        rb.linearVelocity = Vector2.zero;
        // (if you have a Collider2D and want them to fall through, disable it here)
    }
    // this will be called by an Animation Event at the end of your Death clip
    public void OnDeathAnimationEnd()
    {
        // 3) Show Game Over
        GameOverManager.Instance.ShowGameOver();
    }
}
