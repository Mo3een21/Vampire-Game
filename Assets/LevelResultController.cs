using UnityEngine;
using TMPro;
using System.Collections;

public class LevelResultsController : MonoBehaviour
{
    [Header("UI Lines (in order)")]
    public TMP_Text line0;
    public TMP_Text line1;
    public TMP_Text line2;
    public TMP_Text line3;
    public TMP_Text line4;
    public TMP_Text line5;

    [Header("Score & Rank Settings")]
    [Tooltip("Maximum possible final score (for S-rank)")]
    public float maxPossibleScore = 12400f;

    [Tooltip("Time under which you get 2× multiplier (seconds)")]
    public float timeFor2x = 6f * 60f;
    [Tooltip("Time under which you get 1.5× (seconds)")]
    public float timeFor1_5x = 8f * 60f;

    [Tooltip("Death penalty per death")]
    public int deathPenaltyPer = 200;

    [Tooltip("Delay between revealing each line")]
    public float revealDelay = 0.5f;

    void OnEnable()
    {
        // 1) gather raw values
        float elapsed = FindObjectOfType<TimerController>().ElapsedTime;
        int treasure = FindObjectOfType<HUDController>().Score;
        int deaths = GameOverManager.Instance.DeathCount;

        // 2) compute multiplier
        float mult = elapsed < timeFor2x ? 2f
                   : elapsed < timeFor1_5x ? 1.5f
                                            : 1f;

        // 3) compute final score
        float rawScore = treasure * mult;
        float penalty = deaths * deathPenaltyPer;
        float finalScoreF = Mathf.Max(0f, rawScore - penalty);
        int finalScore = Mathf.RoundToInt(finalScoreF);

        // 4) determine rank
        string rank;
        if (finalScoreF >= maxPossibleScore) rank = "S";
        else if (finalScoreF >= maxPossibleScore * 0.75f) rank = "A";
        else if (finalScoreF >= maxPossibleScore * 0.5f) rank = "B";
        else rank = "C";

        // 5) fill in each line’s text
        line0.text = "Level I Completed!";
        line1.text = $"Time: {FormatTime(elapsed)}";
        line2.text = $"Treasure Collected: {treasure}";
        line3.text = $"Times Caught: {deaths}";
        line4.text = $"Final Score: {finalScore}";
        line5.text = $"Final Rank: {rank}";

        // 6) hide them all, then start reveal
        line0.gameObject.SetActive(false);
        line1.gameObject.SetActive(false);
        line2.gameObject.SetActive(false);
        line3.gameObject.SetActive(false);
        line4.gameObject.SetActive(false);
        line5.gameObject.SetActive(false);

        StartCoroutine(RevealAll());
    }

    IEnumerator RevealAll()
    {
        yield return ShowLine(line0);
        yield return ShowLine(line1);
        yield return ShowLine(line2);
        yield return ShowLine(line3);
        yield return ShowLine(line4);
        yield return ShowLine(line5);
    }

    IEnumerator ShowLine(TMP_Text txt)
    {
        txt.gameObject.SetActive(true);
        yield return new WaitForSeconds(revealDelay);
    }

    string FormatTime(float sec)
    {
        int m = (int)(sec / 60f);
        int s = (int)(sec % 60f);
        int f = (int)((sec * 100f) % 100f);
        return $"{m:00}:{s:00}:{f:00}";
    }
}
