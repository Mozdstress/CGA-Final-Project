using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public int totalScore = 0; // Combined score and coins
    public int coins = 0; // Coin count
    public int score = 0; // Score from Levels script

    private void Awake()
    {
        // Ensure this GameObject persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        UpdateTotalScore();
    }

    public void UpdateCoins(int newCoins)
    {
        coins = newCoins;
        UpdateTotalScore();
    }

    private void UpdateTotalScore()
    {
        totalScore = score + coins;
    }
}
