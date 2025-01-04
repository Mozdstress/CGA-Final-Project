using TMPro;
using UnityEngine;

public class CombinedScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI combinedScoreText; // Text to display combined value

    void Update()
    {
        if (GameManager.Instance != null)
        {
            int totalScore = GameManager.Instance.totalScore;
            combinedScoreText.text = "Total Score: " + totalScore;
        }
        else
        {
            Debug.LogWarning("GameManager instance not found!");
        }
    }
}
