using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Collider2D))]
public class KeyPickup : MonoBehaviour
{
    bool playerNearby = false;
    bool pickedUp = false;        // Prevents multiple pickups
    PlayerInventory inv;
    KeyTextController uiText;
    AudioSource sfx;

    void Awake()
    {
        // Grab the UI manager and this object's AudioSource
        uiText = FindObjectOfType<KeyTextController>();
        sfx = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            inv = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

    void Update()
    {
        // If already picked up, don’t do anything
        if (pickedUp) return;

        // Only pick up once when E is pressed in range
        if (playerNearby && inv != null && Input.GetKeyDown(KeyCode.E))
        {
            pickedUp = true;

            // Add to inventory
            inv.AddKey();

            // Show popup message
            if (uiText != null)
                uiText.ShowMessage("You acquired a key!");

            // Play pickup sound
            sfx.Play();

            // Hide the key immediately
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
            var col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            // Destroy this GameObject after the sound finishes
            Destroy(gameObject, sfx.clip.length);
        }
    }
}
