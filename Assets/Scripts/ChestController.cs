using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChestController : MonoBehaviour
{
    [Header("Chest Settings")]
    public Sprite openSprite;
    public AudioClip openSound;        // ← assign your SFX here
    public string noKeyMessage = "You have no keys!";
    public string successMessage = "You Earned 500 Points!";
    public int scorePerChest = 500;

    // Internal state
    bool isOpen = false;
    SpriteRenderer sr;
    Collider2D[] allColliders;
    PlayerInventory inventory;
    KeyTextController uiText;
    HUDController hud;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        allColliders = GetComponents<Collider2D>();
        inventory = FindObjectOfType<PlayerInventory>();
        uiText = FindObjectOfType<KeyTextController>();
        hud = FindObjectOfType<HUDController>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isOpen || !other.CompareTag("Player")) return;
        if (Input.GetKeyDown(KeyCode.E))
            TryOpen();
    }

    void TryOpen()
    {
        if (inventory != null && inventory.UseKey())
        {
            OpenChest();
            uiText?.ShowMessage(successMessage); // Show success message if UI text is available
        }
            
        
        else if (uiText != null)
            uiText.ShowMessage(noKeyMessage);
    }

    void OpenChest()
    {
        isOpen = true;

        // 1) Play the open sound at the chest’s position
        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position);

        // 2) Swap to the “open” sprite
        sr.sprite = openSprite;

        // 3) Award score
        if (hud != null)
            hud.AddScore(scorePerChest);

        // 4) Disable only your trigger colliders
        foreach (var col in allColliders)
            if (col.isTrigger)
                col.enabled = false;
    }
}
