using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIButtonEffects : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Scale Settings")]
    [Tooltip("Scale when pointer is over the button")]
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    [Tooltip("Speed of the scale transition")]
    public float scaleSpeed = 10f;

    [Header("Sound Settings")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    RectTransform rect;
    Vector3 originalScale;
    Vector3 targetScale;
    AudioSource audioSource;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
        targetScale = originalScale;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    void Update()
    {
        rect.localScale = Vector3.Lerp(
            rect.localScale,
            targetScale,
            scaleSpeed * Time.deltaTime
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = hoverScale;
        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    // ← When the user presses the mouse button down on the UI element
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
