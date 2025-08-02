using UnityEngine;

public class FireZone : MonoBehaviour
{
    AudioSource sfx;

    void Awake()
    {
        sfx = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            sfx.Play();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            sfx.Stop();
    }
}
