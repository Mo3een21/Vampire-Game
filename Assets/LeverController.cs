// LeverController.cs
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Tooltip("Assign the Door this lever should open")]
    public DoorController door;

    [Tooltip("Sprite when lever has been pulled")]
    public Sprite pulledSprite;

    [Tooltip("Message shown when you pull this lever")]
    public string openDoorMessage = "You hear a door opening somewhere!";

    bool pulled = false;
    SpriteRenderer sr;
    KeyTextController uiText;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        uiText = FindObjectOfType<KeyTextController>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (pulled) return;
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            pulled = true;
            sr.sprite = pulledSprite;

            if (door != null)
                door.OpenDoor();

            if (uiText != null)
                uiText.ShowMessage(openDoorMessage);
        }
    }
}
