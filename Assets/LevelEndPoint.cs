using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelEndPoint : MonoBehaviour
{
    bool reached = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (reached) return;
        if (!other.CompareTag("Player")) return;

        reached = true;
        LevelEndManager.Instance.StatueReached();
    }
}
