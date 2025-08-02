using UnityEngine;
using TMPro;
using System.Collections;

public class KeyTextController : MonoBehaviour
{
    [Tooltip("Drag your KeyMessageText TMP object here")]
    public TextMeshProUGUI keyMessageText;

    [Tooltip("How long the message stays on screen")]
    public float displayTime = 2f;

    Coroutine msgRoutine;

    public void ShowMessage(string message)
    {
        // If a previous display is running, stop it
        if (msgRoutine != null) StopCoroutine(msgRoutine);
        msgRoutine = StartCoroutine(DisplayCoroutine(message));
    }

    IEnumerator DisplayCoroutine(string message)
    {
        keyMessageText.text = message;
        keyMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        keyMessageText.gameObject.SetActive(false);
    }
}
