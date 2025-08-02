using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int keyCount = 0;
    HUDController hud;

    void Start()
    {
        hud = FindObjectOfType<HUDController>();
        hud.UpdateKeys(keyCount);
    }

    public void AddKey()
    {
        keyCount++;
        hud.UpdateKeys(keyCount);
    }

    public bool UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            hud.UpdateKeys(keyCount);
            return true;
        }
        return false;
    }
}
