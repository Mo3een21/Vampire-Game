using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Animator), typeof(AudioSource))]
public class DoorController : MonoBehaviour
{
    [Tooltip("Name of the trigger parameter in your Animator")]
    public string openTriggerName = "OpenDoor";

    bool isOpen = false;
    Collider2D col;
    Animator animator;
    AudioSource sfx;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sfx = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        // 1) play the door audio
        sfx.Play();

        // 2) trigger the open-door animation
        animator.SetTrigger(openTriggerName);

        // 3) disable the collider so the player can walk through
        col.enabled = false;
    }
}
